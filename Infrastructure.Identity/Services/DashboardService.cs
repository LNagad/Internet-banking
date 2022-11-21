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
            Count(x => x.EmailConfirmed == true && x.FirstName != "SuperAdmin" && x.FirstName != "Basic");

        return something; 
    }

    public async Task<int> usersInactives()
    {
        var usersActive = _userManager.Users;
        var something2 = usersActive.
            Count(x => x.EmailConfirmed == false && x.FirstName != "SuperAdmin" && x.FirstName != "Basic");

        return something2;
    }

    public async Task<List<AuthenticationResponse>> getAllUsersAndInformation()
    {
        var getUser = _userManager.
            Users.Where(x => x.FirstName != "SuperAdmin" && x.FirstName != "Basic").ToList();
        
        List<AuthenticationResponse> userList = new();
        
        foreach ( var item in getUser)
        {
            AuthenticationResponse userCast = new()
            {
                FirstName = item.FirstName,
                Email = item.Email,
                UserName = item.UserName,
                IsVerified = item.EmailConfirmed
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
}