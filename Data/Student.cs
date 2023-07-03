namespace ConsoleEF.Data;

public class Student
{
    public FavoriteColor? FavoriteColor { get; set; }

    public string? Name { get; set; }

    public int StudentId { get; set; }

    public override string ToString()
    {
        return $"{this.StudentId}: {this.Name} {this.FavoriteColor}({this.FavoriteColor:D})";
    }
}