using Core.Application.Dtos.Account;
using Core.Application.Helpers;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.Services;
using Core.Application.Services;
using Core.Application.ViewModels.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Middlewares;
using System.Drawing.Printing;

namespace Internet_Banking.Controllers
{
    [Authorize(Roles = "SuperAdmin, Basic")]
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly IDashboradService _dashboradService;

        private readonly ITransactionService _transactionService;
        public HomeController(IProductService productService, ValidateUserSession validateUserSession,
            IDashboradService dashboradService, ITransactionService transactionService)
        {
            _productService = productService;
            _validateUserSession = validateUserSession;
            _dashboradService = dashboradService;
            _transactionService = transactionService;
        }

        public async Task<IActionResult> Index()
        {
            if(!_validateUserSession.HasUser())
            {
               return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            ViewBag.lista = await _productService.GetAllViewModelWithInclude();
            
            ViewBag.users = await _dashboradService.getAllUsers();
            ViewBag.usuariosActivos = await _dashboradService.usersActives();
            ViewBag.usuariosInactivos = await _dashboradService.usersInactives();

            int cantidadProductos = 0;
            int cantidadPagosTotal = 0;
            int cantidadPagosHoy = 0;
            int cantidadTransaccionesTotal = 0;
            int cantidadTransaccionesHoy = 0;

            int cantidadProductosAsignados = 0; 
                
            var productos =  await _productService.GetAllViewModel();

            cantidadProductosAsignados = productos.Count();

            var allTransactions = await _transactionService.GetAllViewModel();

            foreach (TransactionViewModel item in allTransactions)
            {
                
                if (item != null)
                {
                    if (item.isCuentaAhorro == true)
                    {
                        cantidadPagosTotal += 1;
                    }

                    cantidadTransaccionesTotal += 1;

                }

                if (item.Created.Value.Date == DateTime.Now.Date)
                {
                    if(item.isCuentaAhorro == true)
                    {
                        cantidadPagosHoy += 1;
                    }

                    cantidadTransaccionesHoy += 1;

                }

            }

            ViewBag.cantidadPagosTotal = cantidadPagosTotal;
            ViewBag.cantidadPagosHoy = cantidadPagosHoy;
            ViewBag.cantidadTransaccionesTotal = cantidadTransaccionesTotal;
            ViewBag.cantidadTransaccionesHoy = cantidadTransaccionesHoy;
            ViewBag.cantidadProductosAsignados = cantidadProductosAsignados;

            return View();
        }
    }
}