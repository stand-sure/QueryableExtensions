namespace ConsoleEF.Data;

public class Student
{
    public FavoriteColor? FavoriteColor { get; set; }

    public string? Name { get; set; }

    public int StudentId { get; set; }

    public override string ToString()
    {
        return $"{nameof(Student)}{this.StudentId:_000}: {this.Name} {(this.FavoriteColor == null ? string.Empty : $"{this.FavoriteColor}({this.FavoriteColor:D})")}";
    }
}