using Core.Application.Dtos.Account;

namespace Core.Application.Interfaces.Services;

public interface IDashboradService
{
    Task<int> getAllUsers();
    Task<int> usersActives();
    Task<int> usersInactives();
    Task<List<AuthenticationResponse>> getAllUsersAndInformation();

    Task<AuthenticationResponse> getUserAndInformation(string id);
    Task<string> ActivateUser(string userId);
    Task<string> DesactiveUser(string userId);
}