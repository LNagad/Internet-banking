using Core.Application.Enums;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Beneficiarios;
using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Middlewares;

namespace Internet_Banking.Controllers
{
    [Authorize(Roles = "Basic, SuperAdmin")]
    public class BeneficiariosController : Controller
    {
        private readonly IBeneficiarioService _beneficiarioService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly ICuentaAhorroService _cuentaAhorroService;
        private readonly IDashboradService _dashboradService;

        public BeneficiariosController(IBeneficiarioService beneficiarioService, ValidateUserSession validateUserSession,
            ICuentaAhorroService cuentaAhorroService, IDashboradService dashboradService)
        {
            _beneficiarioService = beneficiarioService;
            _validateUserSession = validateUserSession;
            _cuentaAhorroService = cuentaAhorroService;
            _dashboradService = dashboradService;
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            string user = _validateUserSession.UserLoggedIn().Id;

            ViewBag.BeneficiariosList = await _beneficiarioService.GetAllViewModelWithInclude(user);

            return View("Index");
        }


        public IActionResult AddBeneficiario()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            return View("AddBeneficiario", new SaveBeneficiarioViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddBeneficiario(SaveBeneficiarioViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var cuentaExist = await _cuentaAhorroService.AccountExists(vm.NumeroCuenta);


            if (cuentaExist == null)
            {
                vm.HasError = true;
                vm.Error = "Esta cuenta de ahorro no existe!";

                return View("AddBeneficiario", vm);
            };


            vm.IdUser = _validateUserSession.UserLoggedIn().Id;
            vm.IdBeneficiario = cuentaExist.Product.IdUser;
            vm.IdCuentaAhorro = cuentaExist.Id;

            if(vm.IdUser == vm.IdBeneficiario)
            {
                vm.HasError = true;
                vm.Error = "No puede agregar su propia cuenta como beneficiario!";

                return View("AddBeneficiario", vm);
            }

            await _beneficiarioService.Add(vm);

            return RedirectToRoute(new { Controller = "Home", Action = "Index" });
        }

        public async Task<IActionResult> EliminarBeneficiario(string id)
        {
            await _beneficiarioService.Delete(id);

            return RedirectToRoute(new { Controller = "Home", Action = "Index" });
        }
    }
}
