using Core.Application.ViewModels.Prestamos;
using Core.Application.ViewModels.Products;
using Core.Application.ViewModels.TarjetaCreditos;
using Core.Application.ViewModels.Users;
using Core.Domain.Entities;

namespace Core.Application.Interfaces.Services;

public interface IManageUserService
{
    Task<List<ProductViewModel>> gettinProductsById(string Id);
    Task<string> ManageTarjetaCredito( SaveTarjetaCreditoViewModel tarjetaVm);
    Task<string> DeletingCuentaAhorro(string Id, double montoCuenta, string tipo);
    Task<SavePrestamoViewModel> addPrestamoToUser(SavePrestamoViewModel prestamoVm, string Id);
    Task<SaveUserViewModel> GetSaveuserViewModelById(string id);
    Task<string> UpdateUser(SaveUserViewModel vm);
}