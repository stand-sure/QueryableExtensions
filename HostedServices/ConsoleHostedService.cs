namespace ConsoleEF.HostedServices;

using ConsoleEF.Data;
using ConsoleEF.QueryableExtensions;

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

        await EnsureCourses(context, stoppingToken);

        await foreach (Course course in context.Courses.AsAsyncEnumerable().ConfigureAwait(false))
        {
            this.logger.LogInformation(ConsoleHostedService.MessageTemplate, nameof(this.ExecuteAsync), $"Course: {course}");
        }

        await EnsureStudents(context, stoppingToken).ConfigureAwait(false);
        
        await foreach (Student student in GetStudents(context).ConfigureAwait(false))
        {
            this.logger.LogInformation(ConsoleHostedService.MessageTemplate, nameof(this.ExecuteAsync), $"{student}");
        }
        
        this.lifetime.StopApplication();
    }

    private static IAsyncEnumerable<Student> GetStudents(SchoolContext context)
    {
        IQueryable<Student> students = context.Students.AsNoTracking();

        IQueryable<Student> query = students.OrderByEnumKeyNullsLast(s => s.FavoriteColor).ThenBy(s => s.StudentId);
        
        return query.AsAsyncEnumerable();
    }

    private static async Task CreateSomeCourses(SchoolContext context, CancellationToken stoppingToken)
    {
        for (var i = 0; i < 10; i += 1)
        {
            await context.Courses.AddAsync(MakeCourse(), stoppingToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(stoppingToken).ConfigureAwait(false);
    }

    private static async Task EnsureCourses(SchoolContext context, CancellationToken stoppingToken)
    {
        int courseCount = await context.Courses.CountAsync(stoppingToken).ConfigureAwait(false);

        if (courseCount > 0)
        {
            return;
        }

        await CreateSomeCourses(context, stoppingToken).ConfigureAwait(false);
    }

    private static async Task EnsureStudents(SchoolContext context, CancellationToken stoppingToken)
    {
        int count = await context.Students.CountAsync(stoppingToken).ConfigureAwait(false);

        if (count > 0)
        {
            return;
        }

        await MakeSomeStudents(context, stoppingToken).ConfigureAwait(false);
    }

    private static Course MakeCourse()
    {
        return new Course
        {
            CourseName = Guid.NewGuid().ToString("N"),
        };
    }

    private static async Task MakeSomeStudents(SchoolContext context, CancellationToken cancellationToken)
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