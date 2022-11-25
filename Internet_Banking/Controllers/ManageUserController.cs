using Core.Application.Dtos.Account;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.CuentaAhorros;
using Core.Application.ViewModels.Prestamos;
using Core.Application.ViewModels.TarjetaCreditos;
using Core.Application.ViewModels.Transactions;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Middlewares;

namespace Internet_Banking.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ManageUserController : Controller
    {
        private readonly IDashboradService _dashboradService;
        private readonly ICuentaAhorroService _cuentaAhorro;
        private readonly IProductService _productService;
        private readonly IManageUserService _manageUserService;
        private readonly IPrestamoService _prestamoService;
        private readonly ValidateUserSession _validateUser;

        public ManageUserController(IDashboradService dashboradService, ICuentaAhorroService cuentaAhorro,
            IProductService service, IManageUserService manageUserService, IPrestamoService servicePrestamo
          ,ValidateUserSession userSession)
        {
            _dashboradService = dashboradService;
            _cuentaAhorro = cuentaAhorro;
            _productService = service;
            _manageUserService = manageUserService;
            _prestamoService = servicePrestamo;
            _validateUser = userSession;
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }



            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();
          

            return View(new AuthenticationResponse());
        }

        public async Task<IActionResult> ActivateUser(string Id)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }


            await _dashboradService.ActivateUser(Id);
            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();

            return View("Index", new AuthenticationResponse());
        }

        public async Task<IActionResult> DesactivateUser(string Id)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }

            await _dashboradService.DesactiveUser(Id);
            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();

            return View("Index", new AuthenticationResponse());
        }

        [HttpPost]
        public async Task<IActionResult> GetUserByEmail(AuthenticationResponse userResponse)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }

            AuthenticationResponse userGot = new();

            if (userResponse.Email != null)
            {
                userGot = await _dashboradService.GetUserByEmail(userResponse.Email);
            }

            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();

            return View("Index", userGot);
        }

        public async Task<IActionResult> ProductSelector(string Id, string name, bool status)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }


            ViewBag.UserId = Id;
            ViewBag.UserName = name;
            ViewBag.Status = status;

            ViewBag.lista = await _manageUserService.gettinProductsById(Id);

            return View("ProductSelector");
        }

        public async Task<IActionResult> ManageClientProducts(string Id, string name, bool status)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }


            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();

            if (status == false) // Si el usuario no esta activo no se le pueden agregar productos
            {
                RedirectToAction("ErrorMessage");
            }
            if (!ModelState.IsValid)
            {
                return View("ManageClientProducts");
            }

            var cuenta = new SaveCuentaAhorroViewModel();

            cuenta.UserId = Id;
            ViewBag.UserName = name;

            return View(cuenta);
        }

        [HttpPost]
        public async Task<IActionResult> ManageClientProducts(SaveCuentaAhorroViewModel cuentaVm)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }

            await _cuentaAhorro.Add(cuentaVm, cuentaVm.UserId, false);

            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();

            return View("Index", new AuthenticationResponse());
        }

        public async Task<IActionResult> DeleteCuentaById(string Id, double CuentaMonto, string tipo) //DELETE PRODUCT* BY ID
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }


            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();

            await _manageUserService.DeletingCuentaAhorro(Id, CuentaMonto, tipo);

            return View("Index", new AuthenticationResponse());
        }

        public async Task<IActionResult> ManageTarjetaCredito(string Id)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }

            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();

            if (Id != "")
            {
                ViewBag.UserId = Id;
            }

            return View(new SaveTarjetaCreditoViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ManageTarjetaCredito(SaveTarjetaCreditoViewModel tarjetaVm)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }

            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();

            await _manageUserService.ManageTarjetaCredito(tarjetaVm);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ManagePrestamo(string UserId)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }

            var prestamoObj = new SavePrestamoViewModel();

            ViewBag.UserId = UserId;

            return View(prestamoObj);
        }

        [HttpPost]
        public async Task<IActionResult> ManagePrestamo(SavePrestamoViewModel prestamoVm, string UserId)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }

            var prestamoObj = new SavePrestamoViewModel();

            var prestamoFinal = await _prestamoService.AddCuentaAhorro(prestamoVm, UserId);

            await _manageUserService.addPrestamoToUser(prestamoFinal, UserId);

            return View(prestamoObj);
        }

        public async Task<IActionResult> ErrorMessage()
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }

            return View("Index", new AuthenticationResponse());
        }
    }
}


