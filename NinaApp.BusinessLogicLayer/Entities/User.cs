using NinaApp.Core.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace NinaApp.Core.Entities
{
  public class User
  {
    public int ID { get; private set; }

    [StringLength(80)]
    public string? Name { get; private set; }

    [EmailAddress]
    [StringLength(50)]
    public string? Email { get; private set; }

    [StringLength(255)]
    public string? Password { get; private set; }

    //Protected constructor for EF Core
    protected User() { }

    // Private constructor for creating new Users via Domain Logic
    // This enforces that you cannot create a new User() with empty data.
    private User(string name, string email, string password)
    {
      if (string.IsNullOrWhiteSpace(name))
        throw new DomainException("Name cannot be empty.");

      if (string.IsNullOrWhiteSpace(email))
        throw new DomainException("Email cannot be empty.");

      if (string.IsNullOrWhiteSpace(password))
        throw new DomainException("Password cannot be empty.");

      Name = name;
      Email = email;
      Password = password;
    }

    public static User Create(string name, string email, string password)
    {
      return new User(name, email, password);
    }

    public void UpdateDetails(string? name, string? email, string? password)
    {
      if(!string.IsNullOrWhiteSpace(name))
        Name = name;

      if(!string.IsNullOrWhiteSpace(email))
        Email = email;

      if(!string.IsNullOrWhiteSpace(password))
        Password = password;
    }

  }
}
