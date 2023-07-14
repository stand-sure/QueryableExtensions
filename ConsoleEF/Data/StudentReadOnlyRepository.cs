namespace ConsoleEF.Data;

using Microsoft.EntityFrameworkCore;

using ReadOnlyRepository = Taazaa.Shared.DevKit.Framework.Repository.ReadOnlyRepository<SchoolContext, Student, int>;

public class StudentReadOnlyRepository : ReadOnlyRepository
{
    public StudentReadOnlyRepository(IDbContextFactory<SchoolContext> dbContextFactory) : base(dbContextFactory)
    {
    }
}