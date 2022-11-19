using Core.Application.Dtos.Account;
using Core.Application.Helpers;
using Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Middlewares;

namespace Internet_Banking.Controllers
{
    [Authorize(Roles = "SuperAdmin, Basic")]
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly IAccountService _accountService; 

        public HomeController(IProductService productService, ValidateUserSession validateUserSession,IAccountService accountService)
        {
            _productService = productService;
            _validateUserSession = validateUserSession;
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            if(!_validateUserSession.HasUser())
            {
               return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            ViewBag.lista = await _productService.GetAllViewModelWithInclude();
            ViewBag.users = await _accountService.getAllUsers();
            ViewBag.usuariosActivos = await _accountService.usersActives();
            ViewBag.usuariosInactivos = await _accountService.usersInactives();

            return View();
        }
    }
}