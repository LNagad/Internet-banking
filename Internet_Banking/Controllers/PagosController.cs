using Core.Application.Dtos.Account;
using Core.Application.Dtos.Pagos;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Pagos;
using Core.Application.ViewModels.Pagos.PagosExpresos;
using Core.Application.ViewModels.Pagos.PagosTarjetaCredito;
using Core.Application.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;
using SocialMedia.Middlewares;

namespace Internet_Banking.Controllers
{
    public class PagosController : Controller
    {
        private readonly ICuentaAhorroService _cuentaAhorroService;
        private readonly ValidateUserSession _validateUserSession;

        private readonly IPagosService _pagosService;

        AuthenticationResponse _user;
        public PagosController(ICuentaAhorroService cuentaAhorro, ValidateUserSession validateUserSession, IPagosService pagosService)
        {
            _cuentaAhorroService = cuentaAhorro;
            _validateUserSession = validateUserSession;
            _pagosService = pagosService;

            _user = _validateUserSession.UserLoggedIn();
        }

        public async Task<IActionResult> Index()
        {
           
            ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

            ViewBag.listaTarjetas = await _pagosService.GetAllTarjetasProductViewModel(_user.Id);

            return View();
        }

        #region pagoExpresos

        [HttpPost]
        public IActionResult PagosExpresosGetCuenta(string cuenta)
        {
            SavePagoExpresoViewModel pago = new();
            pago.NumeroCuentaOrigen = cuenta;

            return View("PagosExpreso/PagosExpresos", pago);
        }

        [HttpPost]
        public async Task<IActionResult> PagosExpresos(SavePagoExpresoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("PagosExpreso/PagosExpresos", vm);
            }

            PagoExpressResponse response = await _pagosService.PagoExpress(vm);

            if (response.HasError)
            {
                vm.HasError = true;
                vm.Error = response.Error;

                return View("PagosExpreso/PagosExpresos", vm);
            }

            return RedirectToAction("PagoExpresoDetails", response);
        }

        public IActionResult PagoExpresoDetails(PagoExpressResponse response)
        {
            return View("PagosExpreso/PagoExpresoDetails", response);
        }

        [HttpPost]
        public async Task<IActionResult> PagosExpresoConfirm(PagoExpressResponse vm)
        {

            PagoConfirmedViewModel response = await _pagosService.PagosExpresoConfirmed(vm);


            if (response.HasError)
            {
                vm.HasError = true;
                vm.Error = vm.Error;

                return View("PagosExpreso/PagoExpresoDetails", vm);
            }

            return RedirectToAction("PagoConfirmed", response);
        }

        #endregion


        #region pagoTarjetaCredito


        public async Task<IActionResult> GetTarjetas(string productId)
        {

            var tarjetas = await _pagosService.GetAllTarjetasProductViewModel(_user.Id);

            return RedirectToAction("EnvioPagoTarjeta", new { productId = productId });
        }

        
        public async Task<IActionResult> EnvioPagoTarjeta(string productId)
        {
            ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

            SavePagoTarjetaViewModel vm = new();
            vm.idProduct = productId;

            return View("PagoTarjetaCredito/EnvioPagoTarjeta", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EnvioPagoTarjetaPost(SavePagoTarjetaViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

                return View("PagoTarjetaCredito/EnvioPagoTarjeta", vm);
            }



            return null;
        }

        #endregion

        public IActionResult PagoConfirmed(PagoConfirmedViewModel response)
        {
            return View(response);
        }
    }
}
