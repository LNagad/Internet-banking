using Core.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Internet_Banking.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ManageUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }    
}


