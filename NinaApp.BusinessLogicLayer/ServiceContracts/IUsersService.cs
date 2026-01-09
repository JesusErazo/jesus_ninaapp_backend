using NinaApp.Core.Common;
using NinaApp.Core.DTO;

namespace NinaApp.Core.ServiceContracts
{
  public interface IUsersService
  {
    /// <summary>
    /// Retrieves a group of Users applying pagination logic.
    /// </summary>
    /// <param name="page">The desired page number.</param>
    /// <param name="pageSize">The amount of Users per page.</param>
    /// <returns>A ServiceResult object with the retrieved data (if any) and the detail of the operation.</returns>
    public Task<ServiceResult<PagedList<UserResponse>>> GetUsers(int page, int pageSize);
    /// <summary>
    /// Retrieves the specific User data searching by its User ID.
    /// </summary>
    /// <param name="userID">User ID to search.</param>
    /// <returns>A ServiceResult object with the retrieved data (if any) and the detail of the operation.</returns>
    public Task<ServiceResult<UserResponse?>> GetUserByID(int userID);
    /// <summary>
    /// Creates a new User in the system.
    /// </summary>
    /// <param name="user">User data object to be created.</param>
    /// <returns>A ServiceResult object with the retrieved data (if any) and the detail of the operation.</returns>
    public Task<ServiceResult<UserResponse>> CreateUser(UserCreation user);
    /// <summary>
    /// Updates an existing User with the specific new data.
    /// </summary>
    /// <param name="userID">User ID to update.</param>
    /// <param name="user">The new User data to update.</param>
    /// <returns>A ServiceResult object with the retrieved data (if any) and the detail of the operation.</returns>
    public Task<ServiceResult> UpdateUser(int userID, UserUpdation user);
    /// <summary>
    /// Deletes an existing User of the system, searching by its User ID.
    /// </summary>
    /// <param name="UserID">User ID to be deleted of the system.</param>
    /// <returns>A ServiceResult object with the retrieved data (if any) and the detail of the operation.</returns>
    public Task<ServiceResult> DeleteUser(int userID);
  }
}
