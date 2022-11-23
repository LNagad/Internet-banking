using Core.Application.ViewModels.Prestamos;
using Core.Application.ViewModels.Products;
using Core.Application.ViewModels.TarjetaCreditos;
using Core.Domain.Entities;

namespace Core.Application.Interfaces.Services;

public interface IManageUserService
{
    Task<List<ProductViewModel>> gettinProductsById(string Id);
    Task<string> ManageTarjetaCredito( SaveTarjetaCreditoViewModel tarjetaVm);
    Task<string> DeletingCuentaAhorro(string Id, double montoCuenta, bool tipo);
    Task<SavePrestamoViewModel> addPrestamoToUser(SavePrestamoViewModel prestamoVm, string Id);
}