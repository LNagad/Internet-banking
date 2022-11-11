using Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Internet_Banking.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService; 
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}