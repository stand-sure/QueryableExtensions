namespace Service.Data;

using ConsoleEF.Data;

using Microsoft.EntityFrameworkCore;

using Taazaa.Shared.DevKit.Framework.Repository;

/// <summary>
/// 
/// </summary>
public class StudentReadOnlyRepository : ReadOnlyRepository<SchoolContext, Student, int>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContextFactory"></param>
    public StudentReadOnlyRepository(IDbContextFactory<SchoolContext> dbContextFactory) : base(dbContextFactory)
    {
    }
}