using Core.Application.Dtos.Pagos;
using Core.Application.ViewModels.Pagos;
using Core.Application.ViewModels.Pagos.PagosExpresos;
using Core.Application.ViewModels.Pagos.PagosTarjetaCredito;
using Core.Application.ViewModels.Products;
using Core.Application.ViewModels.TarjetaCreditos;

namespace Core.Application.Interfaces.Services
{
    public interface IPagosService
    {
        Task<PagoExpressResponse> PagoExpress(SavePagoExpresoViewModel vm);
        Task<PagoConfirmedViewModel> PagosExpresoConfirmed(PagoExpressResponse vm);

        Task<List<TarjetaCreditoViewModel>> GetAllTarjetasProductViewModel(string id);

        Task<PagoTarjetaResponse> ValidationPaymentTarjeta(SavePagoTarjetaViewModel pagoVm);
    }
}