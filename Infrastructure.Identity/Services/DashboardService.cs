using Core.Application.Dtos.Account;
using Core.Application.Interfaces.Services;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Services;

public class DashboardService : IDashboradService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailService _emailService;

    public DashboardService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }
    
    public async Task<int> getAllUsers()
    {
        var users = _userManager.Users.
            Where(x => x.FirstName != "SuperAdmin" && x.FirstName != "Basic");

        var userCount = users.Count();
            
        return userCount;
    }

    public async Task<int> usersActives()
    {
        var usersActive = _userManager.Users;
        var something = usersActive.
            Count(x => x.Status == true && x.FirstName != "SuperAdmin" && x.FirstName != "Basic");

        return something; 
    }

    public async Task<int> usersInactives()
    {
        var usersActive = _userManager.Users;
        var something2 = usersActive.
            Count(x => x.Status == false && x.FirstName != "SuperAdmin" && x.FirstName != "Basic");

        return something2;
    }

    public async Task<List<AuthenticationResponse>> getAllUsersAndInformation()
    {
        var getUser = _userManager.
            Users.Where(x => x.FirstName != "SuperAdmin" && x.FirstName != "Basic").ToList();

        
        
        List<AuthenticationResponse> userList = new();
        
        foreach ( var item in getUser)
        {
            var Roles = await _userManager.GetRolesAsync(item);
            
            AuthenticationResponse userCast = new()
            {
                Id = item.Id,
                FirstName = item.FirstName,
                Email = item.Email,
                UserName = item.UserName,
                IsVerified = item.EmailConfirmed,
                Status = item.Status,
                Roles = Roles.ToList()
            };
            userList.Add( userCast );
        }

        return userList;
    }


    public async Task<AuthenticationResponse> getUserAndInformation(string id)
    {
        AuthenticationResponse response = new();

        var getUser = _userManager.Users.FirstOrDefault(p=> p.Id == id);

        if (getUser != null)
        {
            response.Id= getUser.Id;
            response.FirstName= getUser.FirstName;
            response.LastName= getUser.LastName;
        }

        return response;
    }

    public async Task<string> ActivateUser( string userId )
    {
        var user = await _userManager.FindByIdAsync(userId);

        if ( user != null)
        {
            user.Status = true;
            await _userManager.UpdateAsync(user);
        }

        return "";
    }
    
    public async Task<string> DesactiveUser( string userId )
    {
        var user = await _userManager.FindByIdAsync(userId);

        if ( user != null)
        {
            user.Status = false;
            await _userManager.UpdateAsync(user);
        }

        return "";
    }

    public async Task<AuthenticationResponse> GetUserByEmail( string email )
    {
        
        AuthenticationResponse userResponse = new AuthenticationResponse(); 
        
        if( email != null || email != "" )
        {
            var user = await _userManager.FindByEmailAsync(email);

            if ( user != null )
            {
                var role = await _userManager.GetRolesAsync(user);
            
                userResponse.Id = user.Id;
                userResponse.FirstName = user.FirstName;
                userResponse.LastName = user.LastName;
                userResponse.Email = user.Email;
                userResponse.Status = user.Status;
                userResponse.Roles = role.ToList();
            }
            
        }

        return userResponse;
    }
}