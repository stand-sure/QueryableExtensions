namespace ConsoleEF.Data;

public class Student : StudentCreateInfo
{
    public int StudentId { get; set; }
    
    public override string ToString()
    {
        return $"{nameof(Student)}{this.StudentId:_000}: {this.Name} {(this.FavoriteColor == null ? string.Empty : $"{this.FavoriteColor}({this.FavoriteColor:D})")}";
    }

    public static Student Create(StudentCreateInfo info)
    {
        return new Student
        {
            FavoriteColor = info.FavoriteColor,
            IsEnrolled = info.IsEnrolled,
            TenantId = info.TenantId,
            Name = info.Name,
        };
    }

    public static Student? UpdateStudent(Student? student, StudentUpdateInfo? info)
    {
        if (student is null)
        {
            return null;
        }

        if (info is null)
        {
            return student;
        }

        student.FavoriteColor = info.FavoriteColor;
        student.IsEnrolled = info.IsEnrolled;
        student.Name = info.Name;

        return student;
    }
}

public class StudentCreateInfo : StudentUpdateInfo
{
    public int TenantId { get; set; }
}

public class StudentUpdateInfo
{
    public FavoriteColor? FavoriteColor { get; set; }

    public string? Name { get; set; }

    public bool IsEnrolled { get; set; } 
}