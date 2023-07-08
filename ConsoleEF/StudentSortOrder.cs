namespace ConsoleEF;

using SearchFramework.SortOrder;

public class StudentSortOrder : SortOrderBase
{
    public SortOrderDirection Name { get; set; }
    public SortOrderDirection StudentId { get; set; }

    protected override (string Name, SortOrderDirection Direction) DefaultSort => (nameof(this.StudentId), SortOrderDirection.Ascending);
}