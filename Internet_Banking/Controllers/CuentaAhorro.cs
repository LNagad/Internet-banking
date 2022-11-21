using Core.Application.Dtos.Account;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.CuentaAhorros;
using Core.Application.ViewModels.Prestamos;
using Core.Application.ViewModels.Products;
using Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Middlewares;

namespace Internet_Banking.Controllers
{
    public class CuentaAhorro : Controller
    {

        private readonly ICuentaAhorroService _cuentaAhorro;

        private readonly IProductService _productService;

        private readonly ValidateUserSession validateUserSession;

        public CuentaAhorro(ICuentaAhorroService cuentaAhorro, IProductService productService, ValidateUserSession validateUserSession)
        {
            _cuentaAhorro = cuentaAhorro;
            _productService = productService;
            this.validateUserSession = validateUserSession;
        }

        public IActionResult Index()
        {
            return View("SaveCuentaAhorroViewModel");
        }


        public IActionResult AgregarCuentaAhorro()
        {
            return View("SaveCuentaAhorroViewModel", new SaveCuentaAhorroViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AgregarCuentaAhorro(SaveCuentaAhorroViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveCuentaAhorroViewModel",vm);
            }

            await _cuentaAhorro.Add(vm);

            return RedirectToRoute(new { Controller = "Home", Action = "Index" });
        }
    }
}
