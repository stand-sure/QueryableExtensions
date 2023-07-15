namespace Service.Controllers;

using ConsoleEF;
using ConsoleEF.Data;

using Microsoft.AspNetCore.Mvc;

using Service.Data;

using Taazaa.Shared.DevKit.Framework.Repository;
using Taazaa.Shared.DevKit.Framework.TryHelpers;

/// <summary>
/// </summary>
[Route("[controller]")]
public class StudentQueryController : ControllerBase
{
    private readonly StudentReadOnlyRepository repository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="StudentQueryController" /> class.
    /// </summary>
    public StudentQueryController(StudentReadOnlyRepository repository)
    {
        this.repository = repository;
    }

    /// <summary>
    ///     Gets  a student by id.
    /// </summary>
    /// <param name="id">
    ///     The student id.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     200 with Student, or 404
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Student), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetStudentByIdAsync(int id, CancellationToken cancellationToken)
    {
        Result<Student?> result = await this.repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        return result.Map(SuccessResultMapper, this.BadRequest);

        IActionResult SuccessResultMapper(Student? student)
        {
            return student == null ? this.NotFound() : this.Ok(student);
        }
    }

    /// <summary>
    ///     Returns students matching the criteria.
    /// </summary>
    /// <param name="searchParameters">
    ///     search, sorting, and paging criteria
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("search")]
    [ProducesResponseType(typeof(PagedResult<Student?>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<IActionResult> SearchAsync(
        [FromBody] SearchParameters<Student, StudentSearchCriteria, StudentSortOrder>? searchParameters = null,
        CancellationToken cancellationToken = default)
    {
        PagedResult<Student?> pagedResult = this.repository.Search(searchParameters, cancellationToken);

        IActionResult result = this.Ok(pagedResult);

        return Task.FromResult(result);
    }
}
