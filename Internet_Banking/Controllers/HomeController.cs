using Core.Application.Dtos.Account;
using Core.Application.Helpers;
using Core.Application.Interfaces.Repositories;
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
        private readonly IDashboradService _dashboradService;

        public HomeController(IProductService productService, 
            ValidateUserSession validateUserSession,
            IDashboradService dashboradService)
        {
            _productService = productService;
            _validateUserSession = validateUserSession;
            _dashboradService = dashboradService;
        }

        public async Task<IActionResult> Index()
        {
            if(!_validateUserSession.HasUser())
            {
               return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            ViewBag.lista = await _productService.GetAllViewModelWithInclude();
            
            ViewBag.users = await _dashboradService.getAllUsers();
            ViewBag.usuariosActivos = await _dashboradService.usersActives();
            ViewBag.usuariosInactivos = await _dashboradService.usersInactives();
            ViewBag.Products = await _productService.GetAllTransactions();
            
            return View();
        }
    }
}