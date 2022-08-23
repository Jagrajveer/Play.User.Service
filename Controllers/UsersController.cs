using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Service.Repositories;
using Play.User.Service.Dtos;
using Play.Contracts.User;

namespace Play.User.Service.Controllers;

[ApiController]
[Route("users")]
public class UsersController : Controller {
    private readonly IRepository<Entities.User> _usersRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public UsersController(IRepository<Entities.User> usersRepository, IPublishEndpoint publishEndpoint) {
        _usersRepository = usersRepository;
        _publishEndpoint = publishEndpoint;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAsync() {
        var users = (await _usersRepository.GetAllAsync())
             .Select(user => user.AsDto());

        return Ok(users);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetByIdAsync(Guid id) {
        var user = await _usersRepository.GetAsync(id);

        if (user == null) {
            return NotFound();
        }

        return user.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateAsync(UserCreateDto entity) {
        var user = new Entities.User {
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            UserName = entity.UserName,
            Email = entity.Email,
            CreatedDate = DateTimeOffset.UtcNow
        };

        await _usersRepository.CreateAsync(user);

        await _publishEndpoint.Publish(new UserCreated(user.Id, user.FirstName, user.LastName, user.UserName, user.Email ));

        // ReSharper disable once Mvc.ActionNotResolved
        return CreatedAtAction(nameof(GetByIdAsync), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UserUpdateDto entity)
    {
        var user = await _usersRepository.GetAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        user.FirstName = entity.FirstName;
        user.LastName = entity.LastName;
        user.UserName = entity.UserName;
        user.Email = entity.Email;

        await _usersRepository.UpdateAsync(user);


        await _publishEndpoint.Publish(new UserUpdated(user.Id, user.FirstName, user.LastName, user.UserName, user.Email));

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var user = await _usersRepository.GetAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        await _usersRepository.DeleteAsync(user.Id);

        await _publishEndpoint.Publish(new UserDeleted(user.Id));
        
        return NoContent();
    }
}
