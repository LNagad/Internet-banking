using Core.Application.Dtos.Pagos;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Pagos;
using Core.Application.ViewModels.Pagos.PagosExpresos;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Middlewares;

namespace Internet_Banking.Controllers
{
    public class PagosController : Controller
    {
        private readonly ICuentaAhorroService _cuentaAhorroService;
        private readonly ValidateUserSession validateUserSession;

        private readonly IPagosService pagosService;


        public PagosController(ICuentaAhorroService cuentaAhorro, ValidateUserSession validateUserSession, IPagosService pagosService)
        {
            _cuentaAhorroService = cuentaAhorro;
            this.validateUserSession = validateUserSession;
            this.pagosService = pagosService;
        }

        public async Task<IActionResult> Index()
        {
            var user = validateUserSession.UserLoggedIn().Id;
            ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(user);

            return View("PagosExpreso/Index");
        }

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
                return View(vm);
            }

            PagoExpressResponse response = await pagosService.PagoExpress(vm);

            if (response.HasError)
            {
                vm.HasError= true;
                vm.Error= response.Error;

                return View(vm); 
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

            PagoConfirmedViewModel response = await pagosService.PagosExpresoConfirmed(vm);

            if (response.HasError)
            {
                vm.HasError = true;
                vm.Error = vm.Error;

                return View("PagosExpreso/PagoExpresoDetails", response);
            }

         return RedirectToAction("PagoConfirmed", response);
        }

        public IActionResult PagoConfirmed(PagoConfirmedViewModel response)
        {
            return View(response);
        }
    }
}
