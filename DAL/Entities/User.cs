using CCL.Security.Identity;

namespace DAL.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Role Role { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}