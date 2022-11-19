using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Beneficiarios;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Middlewares;

namespace Internet_Banking.Controllers
{
    public class BeneficiariosController : Controller
    {
        private readonly IBeneficiarioService _beneficiarioService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly ICuentaAhorroService _cuentaAhorroService;

        public BeneficiariosController(IBeneficiarioService beneficiarioService, ValidateUserSession validateUserSession,
            ICuentaAhorroService cuentaAhorroService)
        {
            _beneficiarioService = beneficiarioService;
            _validateUserSession = validateUserSession;
            _cuentaAhorroService = cuentaAhorroService;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult AddBeneficiario()
        {
            return View(new SaveBeneficiarioViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddBeneficiario(SaveBeneficiarioViewModel vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }

            var cuentaExist = await _cuentaAhorroService.AccountExists(vm.IdAccount);


            if (cuentaExist == null)
            {
                vm.HasError = true;
                vm.Error = "Esta cuenta de ahorro no existe!";

                return View(vm);
            };


            vm.IdUser = _validateUserSession.UserLoggedIn().Id;



            return View();
        }
    }
}
