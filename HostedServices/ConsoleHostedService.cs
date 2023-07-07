namespace ConsoleEF.HostedServices;

using ConsoleEF.Data;
using ConsoleEF.QueryableExtensions;
using ConsoleEF.SearchFramework;
using ConsoleEF.SearchFramework.SearchCriteria;
using ConsoleEF.SearchFramework.SearchCriteria.Aggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal class ConsoleHostedService : BackgroundService
{
    private const string MessageTemplate = $"{nameof(ConsoleHostedService)}.{{Method}}: {{Message}}";
    private readonly IDbContextFactory<SchoolContext> dbContextFactory;
    private readonly IHostApplicationLifetime lifetime;

    private readonly ILogger<ConsoleHostedService> logger;

    public ConsoleHostedService(
        ILogger<ConsoleHostedService> logger,
        IDbContextFactory<SchoolContext> dbContextFactory,
        IHostApplicationLifetime lifetime)
    {
        this.logger = logger;
        this.dbContextFactory = dbContextFactory;
        this.lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SchoolContext context = await this.dbContextFactory.CreateDbContextAsync(stoppingToken).ConfigureAwait(false);

        await EnsureCoursesAsync(context, stoppingToken);

        await foreach (Course course in context.Courses.AsAsyncEnumerable().ConfigureAwait(false))
        {
            this.logger.LogInformation(ConsoleHostedService.MessageTemplate, nameof(this.ExecuteAsync), $"Course: {course}");
        }

        await EnsureStudentsAsync(context, stoppingToken).ConfigureAwait(false);

        await foreach (Student student in GetStudentsAsync(context).ConfigureAwait(false))
        {
            this.logger.LogInformation(ConsoleHostedService.MessageTemplate, nameof(this.ExecuteAsync), $"{student}");
        }

        await foreach (Student student in this.SearchStudents(context).ConfigureAwait(false))
        {
            this.logger.LogInformation(ConsoleHostedService.MessageTemplate, nameof(this.ExecuteAsync), $"{student}");
        }

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

    private static async Task EnsureStudentsAsync(SchoolContext context, CancellationToken stoppingToken)
    {
        int count = await context.Students.CountAsync(stoppingToken).ConfigureAwait(false);

        if (count > 0)
        {
            return;
        }

        await MakeSomeStudentsAsync(context, stoppingToken).ConfigureAwait(false);
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

    private static async Task MakeSomeStudentsAsync(DbContext context, CancellationToken cancellationToken)
    {
        for (var i = 0; i < 10; i += 1)
        {
            FavoriteColor? favoriteColor = Enum.IsDefined(typeof(FavoriteColor), i) ? (FavoriteColor)i : null;

            Student s = MakeStudent(favoriteColor);

            await context.AddAsync(s, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private static Student MakeStudent(FavoriteColor? favoriteColor)
    {
        return new Student { Name = Guid.NewGuid().ToString("N"), FavoriteColor = favoriteColor };
    }

    private IAsyncEnumerable<Student> SearchStudents(SchoolContext context)
    {
        IQueryable<Student> students = context.Students.AsNoTracking();

        StudentSearchCriteria studentSearchCriteria1 = new()
        {
            StudentId = new ValueSearchCriteria<int>
            {
                Or = new ValueSearchCriteria<int>[]
                {
                    new() { EqualTo = 1, },
                    new() { EqualTo = 2 },
                },
            },
        };

        StudentSearchCriteria studentSearchCriteria2 = new()
        {
            Name = new StringSearchCriteria { StartsWith = "7" },
        };

        AndAggregateValueSearchCriteria<StudentSearchCriteria, Student> aggregateSearchCriteria = new()
        {
            Criteria = new[]
            {
                studentSearchCriteria1,
                studentSearchCriteria2,
            },
        };

        IAsyncEnumerable<Student>? result = null;

        try
        {
            result = students.Where(aggregateSearchCriteria).AsAsyncEnumerable();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, ConsoleHostedService.MessageTemplate, nameof(this.SearchStudents), e.Message);
        }

        return result ?? Empty<Student>();
    }

    private static async IAsyncEnumerable<T> Empty<T>()
    {
        await Task.Yield();
        yield break;
    }
}