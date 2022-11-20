using Core.Application.Enums;
using Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Internet_Banking.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ManageUserController : Controller
    {
        private readonly IDashboradService _dashboradService;

        public ManageUserController( IDashboradService dashboradService)
        {
            _dashboradService = dashboradService; 
        }
        
        public async Task<IActionResult> Index()
        {
            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();
            
            return View();
        }
    }    
}


