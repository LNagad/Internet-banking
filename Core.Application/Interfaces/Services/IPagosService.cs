using Core.Application.Dtos.Pagos;
using Core.Application.ViewModels.Pagos;
using Core.Application.ViewModels.Pagos.PagoAvance;
using Core.Application.ViewModels.Pagos.PagosBeneficiarios;
using Core.Application.ViewModels.Pagos.PagosExpresos;
using Core.Application.ViewModels.Pagos.PagosTarjetaCredito;


namespace Core.Application.Interfaces.Services
{
    public interface IPagosService
    {
        Task<PagoExpressResponse> PagoExpress(SavePagoExpresoViewModel vm);
        Task<PagoConfirmedViewModel> PagosExpresoConfirmed(PagoExpressResponse vm);
        Task<PagoTarjetaResponse> SendPaymentTarjeta(SavePagoTarjetaViewModel pagoVm);
        Task<PagoPrestamoResponse> SendPaymentPrestamo(SavePagoPrestamoViewModel prestamoVm);

        Task<PagoBeneficiarioResponse> SendPaymentBeneficiario(SavePagoBeneficiariosViewModel beneficiarioVm);
        Task<PagoAvanceEfectivoResponse> GetAvancePago(SavePagoAvanceViewModel avancePagoVm);

        Task<PagoConfirmedViewModel> SendPaymentPagoEntreCuenta(SavePagoEntreCuentas vm);
    }
}