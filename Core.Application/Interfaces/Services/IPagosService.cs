using Core.Application.Dtos.Pagos;
using Core.Application.ViewModels.Pagos;
using Core.Application.ViewModels.Pagos.PagosExpresos;

namespace Core.Application.Interfaces.Services
{
    public interface IPagosService
    {
        Task<PagoExpressResponse> PagoExpress(SavePagoExpresoViewModel vm);
        Task<PagoConfirmedViewModel> PagosExpresoConfirmed(PagoExpressResponse vm);
    }
}