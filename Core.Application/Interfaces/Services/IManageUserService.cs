using Core.Application.ViewModels.Products;
using Core.Application.ViewModels.TarjetaCreditos;

namespace Core.Application.Interfaces.Services;

public interface IManageUserService
{
    Task<List<ProductViewModel>> gettinProductsById(string Id);
    Task<string> ManageTarjetaCredito( SaveTarjetaCreditoViewModel tarjetaVm);
}