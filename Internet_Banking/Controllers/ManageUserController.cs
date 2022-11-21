using Core.Application.Interfaces.Services;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Internet_Banking.Controllers
{
    //[Authorize(Roles = "SuperAdmin, Admin")]
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
        
        public async Task<IActionResult> ActivateUser(string Id)
        {
            await _dashboradService.ActivateUser(Id);
            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();

            return View("Index");
        }
        
        public async Task<IActionResult> DesactivateUser(string Id)
        {
            await _dashboradService.DesactiveUser(Id);
            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();

            return View("Index");
        }
    }    
}


