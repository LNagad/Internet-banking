using AutoMapper;
using Core.Application.ViewModels.Beneficiarios;
using Core.Application.ViewModels.CuentaAhorros;
using Core.Application.ViewModels.Prestamos;
using Core.Application.ViewModels.Products;
using Core.Application.ViewModels.TarjetaCreditos;
using Core.Application.ViewModels.Users;
using Core.Domain.Entities;

namespace Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile() 
        {
            CreateMap<Product, ProductViewModel>()
                .ReverseMap()
                .ForMember(P => P.Created, opt => opt.Ignore())
                .ForMember(P => P.CreatedBy, opt => opt.Ignore())
                .ForMember(P => P.LastModified, opt => opt.Ignore())
                .ForMember(P => P.LastModifiedBy, opt => opt.Ignore());

            CreateMap<Product, SaveProductViewModel>()
                .ReverseMap()
                .ForMember(P => P.LastModified, opt => opt.Ignore())
                .ForMember(P => P.CreatedBy, opt => opt.Ignore())
                .ForMember(P => P.LastModified, opt => opt.Ignore())
                .ForMember(P => P.LastModifiedBy, opt => opt.Ignore())
                .ForMember(P => P.User, opt => opt.Ignore())
                .ForMember(P => P.CuentaAhorros, opt => opt.Ignore())
                .ForMember(P => P.TarjetaCreditos, opt => opt.Ignore())
                .ForMember(P => P.Prestamos, opt => opt.Ignore());

            //User
            CreateMap<User, UserViewModel>()
                .ReverseMap()
                .ForMember(P => P.Created, opt => opt.Ignore())
                .ForMember(P => P.CreatedBy, opt => opt.Ignore())
                .ForMember(P => P.LastModified, opt => opt.Ignore())
                .ForMember(P => P.LastModifiedBy, opt => opt.Ignore());

            CreateMap<User, SaveUserViewModel>()
                .ForMember(P => P.File, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(P => P.Created, opt => opt.Ignore())
                .ForMember(P => P.CreatedBy, opt => opt.Ignore())
                .ForMember(P => P.LastModified, opt => opt.Ignore())
                .ForMember(P => P.LastModifiedBy, opt => opt.Ignore())
                .ForMember(P => P.Products, opt => opt.Ignore())
                .ForMember(P => P.Beneficiarios, opt => opt.Ignore());

            //Tarjeta de Credito
            CreateMap<TarjetaCredito, TarjetaCreditoViewModel>()
                .ReverseMap()
                .ForMember(P => P.Created, opt => opt.Ignore())
                .ForMember(P => P.CreatedBy, opt => opt.Ignore())
                .ForMember(P => P.LastModified, opt => opt.Ignore())
                .ForMember(P => P.LastModifiedBy, opt => opt.Ignore());

            CreateMap<TarjetaCredito, SaveTarjetaCreditoViewModel>()
                .ReverseMap()
                .ForMember(P => P.Created, opt => opt.Ignore())
                .ForMember(P => P.CreatedBy, opt => opt.Ignore())
                .ForMember(P => P.LastModified, opt => opt.Ignore())
                .ForMember(P => P.LastModifiedBy, opt => opt.Ignore())
                .ForMember(P => P.Product, opt => opt.Ignore());

            //Prestamos
            CreateMap<Prestamo, PrestamoViewModel>()
                .ReverseMap()
                .ForMember(P => P.Created, opt => opt.Ignore())
                .ForMember(P => P.CreatedBy, opt => opt.Ignore())
                .ForMember(P => P.LastModified, opt => opt.Ignore())
                .ForMember(P => P.LastModifiedBy, opt => opt.Ignore());

            CreateMap<Prestamo, SavePrestamoViewModel>()
                .ReverseMap()
                .ForMember(P => P.Created, opt => opt.Ignore())
                .ForMember(P => P.CreatedBy, opt => opt.Ignore())
                .ForMember(P => P.LastModified, opt => opt.Ignore())
                .ForMember(P => P.LastModifiedBy, opt => opt.Ignore())
                .ForMember(P => P.Product, opt => opt.Ignore());

            //Cuenta de Ahorros
            CreateMap<CuentaAhorro, CuentaAhorroViewModel>()
                .ReverseMap()
                .ForMember(P => P.Created, opt => opt.Ignore())
                .ForMember(P => P.CreatedBy, opt => opt.Ignore())
                .ForMember(P => P.LastModified, opt => opt.Ignore())
                .ForMember(P => P.LastModifiedBy, opt => opt.Ignore());

            CreateMap<CuentaAhorro, SaveCuentaAhorroViewModel>()
                .ReverseMap()
                .ForMember(P => P.Created, opt => opt.Ignore())
                .ForMember(P => P.CreatedBy, opt => opt.Ignore())
                .ForMember(P => P.LastModified, opt => opt.Ignore())
                .ForMember(P => P.LastModifiedBy, opt => opt.Ignore())
                .ForMember(P => P.Product, opt => opt.Ignore())
                .ForMember(P => P.Beneficiarios, opt => opt.Ignore());

            //Beneficiarios
            CreateMap<Beneficiario, BeneficiarioViewModel>()
                .ReverseMap()
                .ForMember(P => P.Created, opt => opt.Ignore())
                .ForMember(P => P.CreatedBy, opt => opt.Ignore())
                .ForMember(P => P.LastModified, opt => opt.Ignore())
                .ForMember(P => P.LastModifiedBy, opt => opt.Ignore());

            CreateMap<Beneficiario, SaveBeneficiarioViewModel>()
                .ReverseMap()
                .ForMember(P => P.Created, opt => opt.Ignore())
                .ForMember(P => P.CreatedBy, opt => opt.Ignore())
                .ForMember(P => P.LastModified, opt => opt.Ignore())
                .ForMember(P => P.LastModifiedBy, opt => opt.Ignore())
                .ForMember(P => P.CuentaAhorro, opt => opt.Ignore());
        }
    }
}
