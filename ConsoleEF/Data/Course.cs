namespace ConsoleEF.Data;

public class Course
{
    public int CourseId { get; set; }

    public string? CourseName { get; set; }

    public override string ToString()
    {
        return $"{this.CourseId}: {this.CourseName}";
    }
}