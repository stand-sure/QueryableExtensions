namespace ConsoleEF;

using ConsoleEF.Data;

using Taazaa.Shared.DevKit.Framework.Search.SortOrder;

public class StudentSortOrder : SortOrderBase<Student>
{
    public SortOrderDirective<string>? Name { get; set; }
    public SortOrderDirective<int>? StudentId { get; set; }

    protected override (string Name, ISortOrderDirective Directive) DefaultSort =>
        (nameof(this.StudentId), new SortOrderDirective<int> { Direction = SortOrderDirection.Ascending });
}