namespace ConsoleEF.Data;

public class Student : StudentCreateInfo
{
    public int StudentId { get; set; }
    
    public override string ToString()
    {
        return $"{nameof(Student)}{this.StudentId:_000}: {this.Name} {(this.FavoriteColor == null ? string.Empty : $"{this.FavoriteColor}({this.FavoriteColor:D})")}";
    }

    public static Student Create(StudentCreateInfo createInfo)
    {
        Student s = ApplyUpdateInfo(new Student(), createInfo)!;

        return s;
    }

    public static Student? ApplyUpdateInfo(Student? student, StudentUpdateInfo? updateInfo)
    {
        if (updateInfo is null || student is null)
        {
            return student;
        }
        
        student.FavoriteColor = updateInfo.FavoriteColor;
        student.IsEnrolled = updateInfo.IsEnrolled;
        student.Name = updateInfo.Name;

        if (updateInfo is StudentCreateInfo createInfo)
        {
            student.SchoolId = createInfo.SchoolId;
        }

        return student;
    }
}

public class StudentCreateInfo : StudentUpdateInfo
{
    public int? SchoolId { get; set; }
}

public class StudentUpdateInfo
{
    public FavoriteColor? FavoriteColor { get; set; }

    public string? Name { get; set; }
    
    public bool IsEnrolled { get; set; } 
}