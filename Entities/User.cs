using Play.Common.Service.Entities;

namespace Play.User.Service.Entities;

public class User : IEntity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTimeOffset CreatedDate { get; set; }
}
