namespace ConsoleEF.Data;

using Microsoft.EntityFrameworkCore;

using Taazaa.Shared.DevKit.Framework.TryHelpers;

using ReadWriteRepository = Taazaa.Shared.DevKit.Framework.Repository.ReadWriteRepository<SchoolContext, Student, StudentCreateInfo, StudentUpdateInfo, int>;

public class StudentReadWriteRepository : ReadWriteRepository
{
    public StudentReadWriteRepository(
        IDbContextFactory<SchoolContext> dbContextFactory,
        StudentReadOnlyRepository readOnlyRepository) : base(dbContextFactory, readOnlyRepository)
    {
    }

    public Task<Result<Student?>> UpdateStudentName(int id, string name, CancellationToken cancellationToken = default)
    {
        return this.UpdateAsync(id, Updater, cancellationToken);

        Student? Updater(Student? student)
        {
            if (student != null)
            {
                student.Name = name;
            }

            return student;
        }
    }

    protected override Func<StudentCreateInfo, Student> DefaultCreateEntityFunction { get; } = Student.Create;

    protected override Func<Student?, StudentUpdateInfo?, Student?> DefaultUpdateEntityFunction { get; } = Student.ApplyUpdateInfo;
}