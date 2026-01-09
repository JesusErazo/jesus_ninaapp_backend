using NinaApp.Core.Common;
using NinaApp.Core.Entities;

namespace NinaApp.Core.RepositoryContracts
{
  public interface IUsersRepository
  {
    /// <summary>
    /// Retrieves users applying pagination logic.
    /// </summary>
    /// <param name="page">The desired page number.</param>
    /// <param name="pageSize">The desired amount of items per page.</param>
    /// <returns>A PagedList<User> object with the retrieved users data and pagination data.</returns>
    public Task<PagedList<User>> GetUsers(int page, int pageSize);
    /// <summary>
    /// Retrieves the specified User object searching by its ID.
    /// </summary>
    /// <param name="userID">User ID to search.</param>
    /// <returns>The specific User object if succeed or null if It is not found.</returns>
    public Task<User?> GetUserByID(int userID);
    /// <summary>
    /// Retrieves an specific user searching by its User Email.
    /// </summary>
    /// <param name="userEmail">User Email to search.</param>
    /// <returns>User object if success of null if It is not found.</returns>
    public Task<User?> GetUserByEmail(string userEmail);
    /// <summary>
    /// Search if the User Email is already being used by other User.
    /// </summary>
    /// <param name="userEmail">User Email to search</param>
    /// <returns>True if User Email is already being used or false if not.</returns>
    public Task<bool> QueryUserEmailIsBeingUsed(string userEmail);
    /// <summary>
    /// Creates a new User in the database.
    /// </summary>
    /// <param name="user">User object to be created.</param>
    /// <returns>The created User if is successfully created or null if occurs any error.</returns>
    public Task<User?> CreateUser(User user);
    /// <summary>
    /// Updates an existing User.
    /// </summary>
    /// <param name="user">User with the new information for the existing User.</param>
    /// <returns>True if User is successfully updated or false if occurs any error.</returns>
    public Task<bool> UpdateUser(User user);
    /// <summary>
    /// Deletes an existing User searching by its User ID.
    /// </summary>
    /// <param name="userID">User ID to be deleted.</param>
    /// <returns>True if User is successfully deleted or false if occurs any error.</returns>
    public Task<bool> DeleteUser(int userID);
  }
}
