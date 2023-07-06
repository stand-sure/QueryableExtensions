namespace ConsoleEF.HostedServices;

using ConsoleEF.Data;
using ConsoleEF.QueryableExtensions;
using ConsoleEF.SearchFramework;

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

        IEnumerable<Student> foo = this.SearchStudents(context);

        foreach (Student student in foo)
        {
            this.logger.LogInformation(ConsoleHostedService.MessageTemplate, nameof(this.ExecuteAsync), $"{student}");
        }

        this.lifetime.StopApplication();
    }

    private IEnumerable<Student> SearchStudents(SchoolContext context)
    {
        IQueryable<Student> students = context.Students.AsNoTracking();

        var c = new StudentSearchCriteria()
        {
            StudentId = new ValueSearchCriteria<int>
            {
                Or = new ValueSearchCriteria<int>[]
                {
                    new() { EqualTo = 1 },
                    new() { EqualTo = 2 },
                },
            },

            Name = new StringSearchCriteria
            {
                EndsWith = "7",
            },
        };

        var d = new OrAggregateValueSearchCriteria<StudentSearchCriteria, Student>
        {
            Criteria = new[]
            {
                c,
                new StudentSearchCriteria
                {
                    Name = new StringSearchCriteria { StartsWith = "1" },
                },
            },
        };

        IEnumerable<Student>? result = null;

        try
        {
            result = students.Where(d).ToList();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, ConsoleHostedService.MessageTemplate, nameof(this.SearchStudents), e.Message);
        }

        return result ?? Enumerable.Empty<Student>();
    }

    private static IAsyncEnumerable<Student> GetStudentsAsync(SchoolContext context)
    {
        IQueryable<Student> students = context.Students.AsNoTracking();

        IQueryable<Student> query = students.OrderByEnumKeyDescendingNullsFirst(s => s.FavoriteColor).ThenBy(s => s.StudentId);

        return query.AsAsyncEnumerable();
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
        return new Student() { Name = Guid.NewGuid().ToString("N"), FavoriteColor = favoriteColor };
    }
}