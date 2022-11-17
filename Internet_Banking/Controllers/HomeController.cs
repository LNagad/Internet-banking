using Core.Application.Enums;
using Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Internet_Banking.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService; 
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}