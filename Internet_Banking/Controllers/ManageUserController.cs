using Core.Application.Dtos.Account;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.CuentaAhorros;
using Core.Application.ViewModels.Prestamos;
using Core.Application.ViewModels.TarjetaCreditos;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        public ManageUserController( IDashboradService dashboradService, ICuentaAhorroService cuentaAhorro,
            IProductService service, IManageUserService manageUserService, IPrestamoService servicePrestamo)
        {
            _dashboradService = dashboradService;
            _cuentaAhorro = cuentaAhorro;
            _productService = service;
            _manageUserService = manageUserService;
            _prestamoService = servicePrestamo;
        }
        
        public async Task<IActionResult> Index()
        {
            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();
            
            return View(new AuthenticationResponse());
        }
        
        public async Task<IActionResult> ActivateUser(string Id)
        {
            await _dashboradService.ActivateUser(Id);
            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();

            return View("Index", new AuthenticationResponse());
        }
        
        public async Task<IActionResult> DesactivateUser(string Id)
        {
            await _dashboradService.DesactiveUser(Id);
            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();

            return View("Index", new AuthenticationResponse());
        }

        [HttpPost]
        public async Task<IActionResult> GetUserByEmail( AuthenticationResponse userResponse )
        {
            AuthenticationResponse userGot = new();

            if ( userResponse.Email != null)
            {
                userGot = await _dashboradService.GetUserByEmail(userResponse.Email);
            }

            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();

            return View("Index", userGot);
        }

        public async Task<IActionResult> ProductSelector( string Id, string name, bool status )
        {
            ViewBag.UserId = Id;
            ViewBag.UserName = name;
            ViewBag.Status = status;
            
            ViewBag.lista = await _manageUserService.gettinProductsById(Id);
            
            return View();
        }

        public async Task<IActionResult> ManageClientProducts( string Id, string name, bool status )
        {
            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();
            
            if ( status == false ) // Si el usuario no esta activo no se le pueden agregar productos
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
        public async Task<IActionResult> ManageClientProducts( SaveCuentaAhorroViewModel cuentaVm )
        {
            await _cuentaAhorro.Add(cuentaVm, cuentaVm.UserId, false);
            
            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();
            
            return View("Index", new AuthenticationResponse());
        }

        public async Task<IActionResult> DeleteCuentaById( string Id, double CuentaMonto, bool tipo) //DELETE PRODUCT* BY ID
        {
            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();
            
            await _manageUserService.DeletingCuentaAhorro(Id, CuentaMonto, tipo);
            
            return View("Index", new AuthenticationResponse());
        }

        public async Task<IActionResult> ManageTarjetaCredito(string Id)
        {
            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();

            if ( Id != "")
            {
                ViewBag.UserId = Id; 
            }

            return View(new SaveTarjetaCreditoViewModel());
        }
        
        [HttpPost]
        public async Task<IActionResult> ManageTarjetaCredito(SaveTarjetaCreditoViewModel tarjetaVm)
        {
            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();
            
            await _manageUserService.ManageTarjetaCredito(tarjetaVm);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ManagePrestamo( string UserId )
        {
            var prestamoObj = new SavePrestamoViewModel();

            ViewBag.UserId = UserId;
            
            return View(prestamoObj);
        }
        
        [HttpPost]
        public async Task<IActionResult> ManagePrestamo( SavePrestamoViewModel prestamoVm, string UserId )
        {
            var prestamoObj = new SavePrestamoViewModel();

            var prestamoFinal = await _prestamoService.AddCuentaAhorro(prestamoVm, UserId);

            await _manageUserService.addPrestamoToUser(prestamoFinal, UserId);
            
            return View(prestamoObj);
        }
        
        public async Task<IActionResult> ErrorMessage()
        {
            return View("Index", new AuthenticationResponse());
        }
    }    
}


