namespace ConsoleEF.HostedServices;

using System.Text.Json;

using ConsoleEF.Data;
using ConsoleEF.QueryableExtensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Taazaa.Shared.DevKit.Framework.Repository;
using Taazaa.Shared.DevKit.Framework.Search;
using Taazaa.Shared.DevKit.Framework.Search.SearchCriteria;
using Taazaa.Shared.DevKit.Framework.Search.SortOrder;

internal class ConsoleHostedService : BackgroundService
{
    private const string MessageTemplate = $"{nameof(ConsoleHostedService)}.{{Method}}: {{Message}}";
    private readonly IDbContextFactory<SchoolContext> dbContextFactory;
    private readonly IHostApplicationLifetime lifetime;
    private readonly StudentReadWriteRepository repository;

    private readonly ILogger<ConsoleHostedService> logger;

    public ConsoleHostedService(
        ILogger<ConsoleHostedService> logger,
        IDbContextFactory<SchoolContext> dbContextFactory,
        IHostApplicationLifetime lifetime,
        StudentReadWriteRepository repository)
    {
        this.logger = logger;
        this.dbContextFactory = dbContextFactory;
        this.lifetime = lifetime;
        this.repository = repository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SchoolContext context = await this.dbContextFactory.CreateDbContextAsync(stoppingToken).ConfigureAwait(false);

        await EnsureCoursesAsync(context, stoppingToken);

        await foreach (Course course in context.Courses.AsAsyncEnumerable().ConfigureAwait(false))
        {
            this.logger.LogInformation(ConsoleHostedService.MessageTemplate, nameof(this.ExecuteAsync), $"Course: {course}");
        }

        await this.EnsureStudentsAsync(context, stoppingToken).ConfigureAwait(false);

        await foreach (Student student in GetStudentsAsync(context).ConfigureAwait(false))
        {
            this.logger.LogInformation(ConsoleHostedService.MessageTemplate, nameof(this.ExecuteAsync), $"{student}");
        }

        await foreach (Student student in SearchStudents(context).ConfigureAwait(false))
        {
            this.logger.LogInformation(ConsoleHostedService.MessageTemplate, nameof(this.ExecuteAsync), $"{student}");
        }

        (int totalCount, IAsyncEnumerable<Student>? students) = await SearchStudentsJson(context).ConfigureAwait(false);

        this.logger.LogInformation(ConsoleHostedService.MessageTemplate, nameof(totalCount), totalCount);

        await foreach (Student student in students.ConfigureAwait(false))
        {
            this.logger.LogInformation(ConsoleHostedService.MessageTemplate, nameof(students), $"{student}");
        }

        PagedResult<Student?> pagedResult = this.SearchStudentRepo(stoppingToken);

        foreach (Student? student in pagedResult.Data!)
        {
            this.logger.LogInformation(ConsoleHostedService.MessageTemplate, nameof(pagedResult), $"{student}");
        }

        this.logger.LogInformation(ConsoleHostedService.MessageTemplate, nameof(pagedResult.HasMoreData), $"{pagedResult.HasMoreData}");

        this.lifetime.StopApplication();
    }

    private static async Task CreateSomeCoursesAsync(SchoolContext context, CancellationToken stoppingToken)
    {
        for (var i = 0; i < 10; i += 1)
        {
            await context.Courses.AddAsync(MakeCourse(), stoppingToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(stoppingToken).ConfigureAwait(false);
    }

    private static async Task EnsureCoursesAsync(SchoolContext context, CancellationToken stoppingToken)
    {
        int courseCount = await context.Courses.CountAsync(stoppingToken).ConfigureAwait(false);

        if (courseCount > 0)
        {
            return;
        }

        await CreateSomeCoursesAsync(context, stoppingToken).ConfigureAwait(false);
    }

    private async Task EnsureStudentsAsync(SchoolContext context, CancellationToken stoppingToken)
    {
        int count = await context.Students.CountAsync(stoppingToken).ConfigureAwait(false);

        if (count > 20)
        {
            return;
        }

        await this.MakeSomeStudentsAsync(stoppingToken).ConfigureAwait(false);
    }

    private static IAsyncEnumerable<Student> GetStudentsAsync(SchoolContext context)
    {
        IQueryable<Student> students = context.Students.AsNoTracking();

        IQueryable<Student> query = students.OrderByEnumKeyDescendingNullsFirst(s => s.FavoriteColor).ThenBy(s => s.StudentId);

        return query.AsAsyncEnumerable();
    }

    private static Course MakeCourse()
    {
        return new Course
        {
            CourseName = Guid.NewGuid().ToString("N"),
        };
    }

    private async Task MakeSomeStudentsAsync(CancellationToken cancellationToken)
    {
        for (var i = 0; i < 10; i += 1)
        {
            FavoriteColor? favoriteColor = Enum.IsDefined(typeof(FavoriteColor), i) ? (FavoriteColor)i : null;

            StudentCreateInfo createInfo = MakeStudentCreateInfo(favoriteColor);

            await this.repository.CreateAsync(createInfo, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }

    private static StudentCreateInfo MakeStudentCreateInfo(FavoriteColor? favoriteColor)
    {
        return new Student { Name = Guid.NewGuid().ToString("N"), FavoriteColor = favoriteColor };
    }

    private static IAsyncEnumerable<Student> SearchStudents(SchoolContext context)
    {
        IQueryable<Student> students = context.Students.AsNoTracking();

        StudentSearchCriteria searchCriteria = new()
        {
            StudentId = new ValueSearchCriteria<int>
            {
                NotEqualTo = 2,
            },
        };

        var sortOrder0 = new StudentSortOrder
        {
            StudentId = SortOrderDirection.Descending,
        };

        var sortOrder1 = new StudentSortOrder
        {
            Name = SortOrderDirection.Ascending,
        };

        StudentSortOrder[] order = { sortOrder1, sortOrder0 };

        IAsyncEnumerable<Student> result = students.Where(searchCriteria).ApplySort(order).AsAsyncEnumerable();

        return result;
    }

    private PagedResult<Student?> SearchStudentRepo(CancellationToken cancellationToken)
    {
        StudentSearchCriteria searchCriteria = new()
        {
            StudentId = new ValueSearchCriteria<int>
            {
                NotEqualTo = 2,
            },
        };

        var sortOrder0 = new StudentSortOrder
        {
            StudentId = SortOrderDirection.Descending,
        };

        var sortOrder1 = new StudentSortOrder
        {
            Name = SortOrderDirection.Ascending,
        };

        StudentSortOrder[] order = { sortOrder1, sortOrder0 };
        
        return this.repository.Search(searchCriteria, order, cancellationToken: cancellationToken);
    }

    private static async Task<(int totalCount, IAsyncEnumerable<Student> students)> SearchStudentsJson(SchoolContext context)
    {
        IQueryable<Student> students = context.Students.AsNoTracking();

        const string jsonSearchCriteria = @"{""StudentId"":{""GreaterThan"":3}}";

        var searchCriteria = JsonSerializer.Deserialize<StudentSearchCriteria>(jsonSearchCriteria)!;

        const string jsonSortOrder = @"[{""Name"":""Ascending""},{""StudentId"":""Descending""}]";

        var sortOrder = JsonSerializer.Deserialize<IEnumerable<StudentSortOrder>>(jsonSortOrder)!;

        IQueryable<Student> query = students.Where(searchCriteria).ApplySort(sortOrder);

        IAsyncEnumerable<Student> result = query.AsAsyncEnumerable();

        int totalCount = await query.CountAsync().ConfigureAwait(false);

        return (totalCount, result);
    }

    ////private static async IAsyncEnumerable<T> Empty<T>()
    ////{
    ////    await Task.Yield();
    ////    yield break;
    ////}
}