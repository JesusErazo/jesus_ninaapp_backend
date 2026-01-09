using Microsoft.EntityFrameworkCore;
using NinaApp.Core.Common;
using NinaApp.Core.Entities;
using NinaApp.Core.RepositoryContracts;
using NinaApp.Infrastructure.Database;
using NinaApp.Infrastructure.Extensions;

namespace NinaApp.Infrastructure.Repositories
{
  public class UsersRepository : IUsersRepository
  {
    private readonly ApplicationDbContext _dbContext;

    public UsersRepository(ApplicationDbContext dbContext) {
      _dbContext = dbContext;
    }

    public async Task<User?> CreateUser(User user)
    {
      if (user is null) return null;

      _dbContext.Users.Add(user);
      int usersCreated = await _dbContext.SaveChangesAsync();

      if(usersCreated > 0) return user;

      return null;
    }

    public async Task<bool> DeleteUser(int userID)
    {
      if(userID <= 0) return false;

      int deletedUsers = await _dbContext.Users.Where(u => u.ID == userID).ExecuteDeleteAsync();

      if (deletedUsers > 0) return true;
      
      return false;
    }

    public async Task<PagedList<User>> GetUsers(int page, int pageSize)
    {
      return await _dbContext.Users
        .AsNoTracking()
        .OrderBy(u => u.ID)
        .ToPagedListAsync(page, pageSize);
    }

    public async Task<User?> GetUserByEmail(string userEmail)
    {
      return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
    }

    public async Task<User?> GetUserByID(int userID)
    {
      if(userID <= 0) return null;

      User? existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.ID == userID);

      return existingUser;
    }

    public async Task<bool> QueryUserEmailIsBeingUsed(string userEmail)
    {
      return await _dbContext.Users.AnyAsync(u => u.Email == userEmail);
    }

    public async Task<bool> UpdateUser(User user)
    {
      if (user is null || user.ID <= 0) return false;

      try
      {
        _dbContext.Users.Update(user);
        int updatedUsers = await _dbContext.SaveChangesAsync();
        return true;
      }
      catch (DbUpdateConcurrencyException)
      {
        return false;
      }
    }
  }
}
