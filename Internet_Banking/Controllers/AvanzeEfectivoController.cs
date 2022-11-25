using Core.Application.Dtos.Account;
using Core.Application.Interfaces.Services;
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
        if (!_validateUserSession.HasUser())
        {
            RedirectToRoute(new {controller = "User", action = "Index"});
        }

        ViewBag.GetAllTarjetaCreditos = await _tarjetaCreditoService.GetAllTarjetaById(_user.Id);

        ViewBag.GetAllCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);

        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> SaveAvanzeEfectivo()
    {
        if (!_validateUserSession.HasUser())
        {
            RedirectToRoute(new {controller = "User", action = "Index"});
        }

        ViewBag.GetAllTarjetaCreditos = await _tarjetaCreditoService.GetAllTarjetaById(_user.Id);

        ViewBag.GetAllCuentasAhorro = await _cuentaAhorroService.GetAllViewModelWithInclude(_user.Id);
        
        
        return View();
    }
}