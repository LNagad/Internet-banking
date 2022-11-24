using Core.Application.Dtos.Pagos;
using Core.Application.ViewModels.Pagos;
using Core.Application.ViewModels.Pagos.PagosBeneficiarios;
using Core.Application.ViewModels.Pagos.PagosExpresos;
using Core.Application.ViewModels.Pagos.PagosTarjetaCredito;
using Core.Application.ViewModels.Prestamos;
using Core.Application.ViewModels.TarjetaCreditos;

namespace Core.Application.Interfaces.Services
{
    public interface IPagosService
    {
        Task<PagoExpressResponse> PagoExpress(SavePagoExpresoViewModel vm);
        Task<PagoConfirmedViewModel> PagosExpresoConfirmed(PagoExpressResponse vm);
        Task<PagoTarjetaResponse> SendPaymentTarjeta(SavePagoTarjetaViewModel pagoVm);
        Task<PagoPrestamoResponse> SendPaymentPrestamo(SavePagoPrestamoViewModel prestamoVm);
        Task<PagoPrestamoResponse> SendPaymentBeneficiario(SavePagoBeneficiariosViewModel beneficiarioVm);

    }
}