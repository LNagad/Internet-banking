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

        public CuentaAhorro(ICuentaAhorroService cuentaAhorro, IProductService productService)
        {
            _cuentaAhorro = cuentaAhorro;
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
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

            string numeroCuenta = "";

            for (int i = 1; i < 11; i++)
            {
                var randomNumeroCuenta = new Random();

                numeroCuenta += String.Join("",randomNumeroCuenta.Next(0, 10).ToString());

            }

            vm.NumeroCuenta = numeroCuenta;
            vm.Principal = true;

            SaveCuentaAhorroViewModel resultado = await _cuentaAhorro.Add(vm);

            var productVM = new SaveProductViewModel();

            if (resultado != null)
            {
                productVM.IdProductType = resultado.Id;
                productVM.isCuentaAhorro = true;
                productVM.isTarjetaCredito = false;
                productVM.Primary = false;
                productVM.isPrestamo = false;

                //Reparar
                productVM.IdUser = "232323";

            }

            SaveProductViewModel productResult = await _productService.Add(productVM);

            if (productResult != null)
            {
                resultado.IdProduct = productResult.Id;
             
                await _cuentaAhorro.Update(resultado, resultado.Id);
            }


            return RedirectToRoute(new { Controller = "Home", Action = "Index" });
        }
    }
}
