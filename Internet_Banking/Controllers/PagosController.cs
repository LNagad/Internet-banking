﻿using Core.Application.Dtos.Account;
using Core.Application.Dtos.Pagos;
using Core.Application.Enums;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Pagos;
using Core.Application.ViewModels.Pagos.PagosBeneficiarios;
using Core.Application.ViewModels.Pagos.PagosExpresos;
using Core.Application.ViewModels.Pagos.PagosTarjetaCredito;
using Core.Application.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;
using SocialMedia.Middlewares;

namespace Internet_Banking.Controllers
{
    [Authorize(Roles = "Basic, SuperAdmin")]
    public class PagosController : Controller
    {
        private readonly ICuentaAhorroService _cuentaAhorroService;
        private readonly IPrestamoService _prestamoService;
        private readonly ValidateUserSession _validateUser;
        private readonly ITarjetaCreditoService _tarjetaCreditoService;
        private readonly IPagosService _pagosService;
        private readonly IBeneficiarioService _beneficiarioService;

        AuthenticationResponse _user;
        public PagosController(ICuentaAhorroService cuentaAhorro, ValidateUserSession validateUserSession, 
            IPagosService pagosService, IPrestamoService prestamoService, ITarjetaCreditoService tarjetaCreditoService,
            IBeneficiarioService beneficiarioService)
        {
            _cuentaAhorroService = cuentaAhorro;
            _validateUser = validateUserSession;
            _pagosService = pagosService;
            _prestamoService = prestamoService;
            _beneficiarioService = beneficiarioService;

            _user = _validateUser.UserLoggedIn();
            _tarjetaCreditoService = tarjetaCreditoService;
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

            ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

            ViewBag.listaTarjetas = await _tarjetaCreditoService.GetAllTarjetaById(_user.Id);

            ViewBag.listaPrestamos = await _prestamoService.GetAllPrestamosById(_user.Id);

            ViewBag.listaBeneficiarios = await _beneficiarioService.GetAllViewModelWithInclude(_user.Id);

            return View();
        }

        #region pagoExpresos

        [HttpPost]
        public IActionResult PagosExpresosGetCuenta(string cuenta)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }


            SavePagoExpresoViewModel pago = new();
            pago.NumeroCuentaOrigen = cuenta;

            return View("PagosExpreso/PagosExpresos", pago);
        }

        [HttpPost]
        public async Task<IActionResult> PagosExpresos(SavePagoExpresoViewModel vm)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }



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
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }


            return View("PagosExpreso/PagoExpresoDetails", response);
        }

        [HttpPost]
        public async Task<IActionResult> PagosExpresoConfirm(PagoExpressResponse vm)
        {

            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }



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
        
        public async Task<IActionResult> GetAllTarjetas(string productId)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }



            ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

            SavePagoTarjetaViewModel vm = new();
            vm.idProduct = productId;

            return View("PagoTarjetaCredito/EnvioPagoTarjeta", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EnvioPagoTarjetaPost(SavePagoTarjetaViewModel vm)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }



            if (!ModelState.IsValid)
            {
                ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

                return View("PagoTarjetaCredito/EnvioPagoTarjeta", vm);
            }

            var response = await _pagosService.SendPaymentTarjeta(vm);

            if (response.HasError == true)
            {
                ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

                if (response.Error == "Usted aun no debe esta tarjeta!")
                {
                    return View("PagoTarjetaCredito/ErrorMessage", response);
                }

                return View("PagoTarjetaCredito/EnvioPagoTarjeta", vm);
            }

            PagoConfirmedViewModel resultado = new();

            resultado.isTarjetaCredito = true;

            resultado.FirstNameOrigen = response.FirstNameOrigen;
            resultado.LastNameOrigen = response.LastNameOrigen;
            resultado.LastTarjetaCredito = response.LastTarjetaCredito;
            resultado.Monto = response.Monto;
            resultado.NumeroCuentaOrigen = response.NumeroCuentaOrigen;
            resultado.TransactionId = response.TransactionId;

            return RedirectToAction("PagoConfirmed", resultado);
        }

        #endregion

        #region pagoPrestamo

        public async Task<IActionResult> GetAllPrestamos(string productId)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }




            ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

            SavePagoPrestamoViewModel vm = new();
            vm.idProduct = productId;

            return View("PagosPrestamo/EnvioPagoPrestamo", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EnvioPagoPrestamoPost(SavePagoPrestamoViewModel vm)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }



            if (!ModelState.IsValid)
            {
                ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

                return View("PagosPrestamo/EnvioPagoPrestamo", vm);
            }

            var response = await _pagosService.SendPaymentPrestamo(vm);

            if (response.HasError == true)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;

                ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

                return View("PagosPrestamo/EnvioPagoPrestamo", vm);
            }

            PagoConfirmedViewModel resultado = new();

            resultado.isPrestamo = true;

            resultado.FirstNameOrigen = response.FirstNameOrigen;
            resultado.LastNameOrigen = response.LastNameOrigen;
            resultado.NumeroPrestamo = response.NumeroPrestamo;
            resultado.Monto = response.Monto;
            resultado.NumeroCuentaOrigen = response.NumeroCuentaOrigen;
            resultado.TransactionId = response.TransactionId;

            return RedirectToAction("PagoConfirmed", resultado);
        }

        #endregion
        
        #region Beneficiarios
        
        public async Task<IActionResult> GetAllBeneficiarios(string numeroCuenta, string BeneficiarioName, string BeneficiarioLastname)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }




            ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

            SavePagoBeneficiariosViewModel vm = new();
            vm.NumeroCuentaDestino = numeroCuenta;
            vm.BeneficiarioName = BeneficiarioName;
            vm.BeneficiarioLastName = BeneficiarioLastname;

            return View("PagosBeneficiario/EnvioPagoBeneficiario", vm);
        }
        
        [HttpPost]
        public async Task<IActionResult> EnvioPagoBeneficiario(SavePagoBeneficiariosViewModel vm)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }



            if (!ModelState.IsValid)
            {
                ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

                return View("PagosBeneficiario/EnvioPagoBeneficiario", vm);
            }

            var response = await _pagosService.SendPaymentBeneficiario(vm);

            if (response.HasError == true)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;

                ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

                return View("PagosBeneficiario/EnvioPagoBeneficiario", vm);
            }

            PagoConfirmedViewModel resultado = new();

            resultado.isBeneficiario = true;

            resultado.NumeroCuentaOrigen = response.NumeroCuentaOrigen;
            resultado.FirstNameOrigen = response.FirstNameOrigen;
            resultado.LastNameOrigen = response.LastNameOrigen;
            resultado.Monto = response.Monto;
            resultado.LastNameOrigen = response.LastNameOrigen;

            resultado.FirstNameDestino = response.FirstNameDestino;
            resultado.LastNameDestino = response.LastNameDestino;
            resultado.NumeroCuentaDestino = response.NumeroCuentaDestino;
            resultado.TransactionId = response.TransactionId;

            return RedirectToAction("PagoConfirmed", resultado);
        }

        #endregion

        #region pagoEntreCuentas

        public async Task<IActionResult> EnvioPagoEntreCuentas()
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }



            ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

            return View("PagosEntreCuentas/EnvioPagoEntreCuentas", new SavePagoEntreCuentas());
        }

        [HttpPost]
        public async Task<IActionResult> EnvioPagoEntreCuentas(SavePagoEntreCuentas vm)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }



            if (!ModelState.IsValid)
            {
                ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

                return View("PagosEntreCuentas/EnvioPagoEntreCuentas", vm);
            }

            var response = await _pagosService.SendPaymentPagoEntreCuenta(vm);

            if (response.HasError == true)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;

                ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

                return View("PagosEntreCuentas/EnvioPagoEntreCuentas", vm);
            }

            //PagoConfirmedViewModel resultado = new();

            //resultado.isPrestamo = true;

            //resultado.FirstNameOrigen = response.FirstNameOrigen;
            //resultado.LastNameOrigen = response.LastNameOrigen;
            //resultado.NumeroPrestamo = response.NumeroPrestamo;
            //resultado.Monto = response.Monto;
            //resultado.NumeroCuentaOrigen = response.NumeroCuentaOrigen;
            //resultado.TransactionId = response.TransactionId;

            return RedirectToAction("PagoConfirmed", response);
        }

        #endregion

        public IActionResult PagoConfirmed(PagoConfirmedViewModel response)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }



            return View(response);
        }
    }
}
