using Core.Application.Dtos.Account;
using Core.Application.Dtos.Pagos;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Pagos.PagoAvance;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Middlewares;

namespace Internet_Banking.Controllers;

public class AvanzeEfectivoController : Controller
{

    private readonly ITarjetaCreditoService _tarjetaCreditoService;
    private readonly ValidateUserSession _validateUserSession;
    private readonly IPagosService _pagosService;
    private readonly ICuentaAhorroService _cuentaAhorroService;
    
    AuthenticationResponse _user;
    
    public AvanzeEfectivoController(ICuentaAhorroService cuentaAhorroService, IPagosService pagosService,
        ValidateUserSession validateUserSession, ITarjetaCreditoService tarjetaCreditoService  )
    {
        _tarjetaCreditoService = tarjetaCreditoService;
        _pagosService = pagosService;
        _validateUserSession = validateUserSession;
        _cuentaAhorroService = cuentaAhorroService;
        
        _user = _validateUserSession.UserLoggedIn();
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.GetAllTarjetaCreditos = await _tarjetaCreditoService.GetAllTarjetaById(_user.Id);
        ViewBag.GetAllCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

        return View();
    }

    public async Task<IActionResult> AvanzeEfectivo(string numeroTarjeta, string idTarjeta, double LimiteTarjeta, string IdProduct)
    {
        if (!_validateUserSession.HasUser())
        {
            RedirectToRoute(new {controller = "User", action = "Index"});
        }

        ViewBag.GetAllTarjetaCreditos = await _tarjetaCreditoService.GetAllTarjetaById(_user.Id);

        ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

        SavePagoAvanceViewModel vm = new();
        vm.NumeroTarjetaCredito = numeroTarjeta;
        vm.idTarjetaCredito = idTarjeta;
        vm.TarjetaMonto = LimiteTarjeta;
        vm.IdProduct = IdProduct;

        return View("SaveAvanzeEfectivo", vm);
    }
    
    [HttpPost]
    public async Task<IActionResult> SaveAvanzeEfectivo(SavePagoAvanceViewModel vm)
    {
        if (!_validateUserSession.HasUser())
        {
            RedirectToAction("SaveAvanzeEfectivo", vm);
        }

        ViewBag.GetAllTarjetaCreditos = await _tarjetaCreditoService.GetAllTarjetaById(_user.Id);

        ViewBag.GetAllCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);
        
        var response = await _pagosService.GetAvancePago(vm);
        
        if (response.HasError == true)
        {
            vm.HasError = response.HasError;
            vm.Error = response.Error;

            ViewBag.listaCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

            return View("SaveAvanzeEfectivo", vm);
        }

        PagoAvanceEfectivoResponse resultado = new();
        
        resultado.FirstNameOrigen = response.FirstNameOrigen;
        resultado.LastNameOrigen = response.LastNameOrigen;
        resultado.NumeroTarjeta = response.NumeroTarjeta;
        resultado.Monto = response.Monto;
        resultado.NumeroCuenta = response.NumeroCuenta;
        resultado.MontoCargado = response.MontoCargado;

        return RedirectToAction("MontoRetiradoConfirmed", response);
    }
    
    public IActionResult MontoRetiradoConfirmed(PagoAvanceEfectivoResponse response)
    {
        return View(response);
    }
}