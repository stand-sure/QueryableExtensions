namespace Service.Data;

using ConsoleEF.Data;

using Microsoft.EntityFrameworkCore;

using Taazaa.Shared.DevKit.Framework.Repository;

public class StudentReadWriteRepository : ReadWriteRepository<SchoolContext, Student, StudentCreateInfo, StudentUpdateInfo, int>
{
    public StudentReadWriteRepository(IDbContextFactory<SchoolContext> dbContextFactory, StudentReadOnlyRepository readOnlyRepository) : base(dbContextFactory, readOnlyRepository)
    {
    }

    protected override Func<StudentCreateInfo, Student> DefaultCreateEntityFunction { get; } = Student.Create;
    protected override Func<Student?, StudentUpdateInfo?, Student?> DefaultUpdateEntityFunction { get; } = Student.UpdateStudent;
}