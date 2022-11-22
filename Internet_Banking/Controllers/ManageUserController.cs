using Core.Application.Dtos.Account;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.CuentaAhorros;
using Core.Application.ViewModels.TarjetaCreditos;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Internet_Banking.Controllers
{
    //[Authorize(Roles = "SuperAdmin, Admin")]
    public class ManageUserController : Controller
    {
        private readonly IDashboradService _dashboradService;
        private readonly ICuentaAhorroService _cuentaAhorro;
        private readonly IProductService _productService;
        private readonly IManageUserService _manageUserService; 

        public ManageUserController( IDashboradService dashboradService, ICuentaAhorroService cuentaAhorro,
            IProductService service, IManageUserService manageUserService)
        {
            _dashboradService = dashboradService;
            _cuentaAhorro = cuentaAhorro;
            _productService = service;
            _manageUserService = manageUserService; 
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
            
            ViewBag.IdUser = Id;
            ViewBag.UserName = name;
            ViewBag.lista = await _manageUserService.gettinProductsById(Id);

            return View(new SaveCuentaAhorroViewModel());
        }
        
        [HttpPost]
        public async Task<IActionResult> ManageClientProducts( SaveCuentaAhorroViewModel cuentaVm )
        {
            await _cuentaAhorro.Add(cuentaVm, cuentaVm.UserId, false);
            
            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();
            
            return View("Index", new AuthenticationResponse());
        }

        public async Task<IActionResult> DeleteCuentaById( string Id )
        {
            ViewBag.usersList = await _dashboradService.getAllUsersAndInformation();

            await _productService.Delete(Id);
            
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

            return View(new SaveTarjetaCreditoViewModel());
        }

        public async Task<IActionResult> ErrorMessage()
        {
            return View("Index", new AuthenticationResponse());
        }
    }    
}


