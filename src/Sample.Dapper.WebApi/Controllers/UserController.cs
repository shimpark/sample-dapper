using Sample.Dapper.Persistence.Entities;
using Sample.Dapper.Persistence.Interfaces;

namespace Sample.Dapper.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;

    public UserController(IUserRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Posts the specified user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns></returns>
    [HttpPost("Add")]
    public async Task<IActionResult> Post([FromBody] User user)
    {
        var result = await _repository.AddAsync(user);
        return Ok(result);
    }

    /// <summary>
    /// Gets the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("Get")]
    public async Task<IActionResult> Get([FromQuery] int id)
    {
        var result = await _repository.GetAsync(id);
        return Ok(result);
    }


    /// <summary>
    /// Gets the list paged.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowPerPages">The row per pages.</param>
    /// <param name="conditions">The conditions.</param>
    /// <param name="orderby">The orderby.</param>
    /// <returns></returns>
    [HttpGet("GetPaged")]
    public async Task<IActionResult> GetListPaged(int pageNumber, int rowPerPages, string conditions, string orderby)
    {
        var list = await _repository.GetListPaged(pageNumber, rowPerPages, conditions, orderby);
        return Ok(list);
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }
}