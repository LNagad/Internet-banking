using Core.Application.Enums;
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

        public HomeController(IProductService productService, ValidateUserSession validateUserSession)
        {
            _productService = productService;
            _validateUserSession = validateUserSession;
        }

        public async Task<IActionResult> Index()
        {
            if(!_validateUserSession.HasUser())
            {
               return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            ViewBag.lista = await _productService.GetAllViewModelWithInclude();

            return View();
        }
    }
}