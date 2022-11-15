using AutoMapper;
using Core.Application.Dtos.Account;
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

            //Account
            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(p => p.HasError, opt => opt.Ignore())
                .ForMember(p => p.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(p => p.HasError, opt => opt.Ignore())
                .ForMember(p => p.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ForgotPasswordRequest, ForgotPasswordViewModel>()
                .ForMember(p => p.HasError, opt => opt.Ignore())
                .ForMember(p => p.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ResetPasswordRequest, ResetPasswordViewModel>()
                .ForMember(p => p.HasError, opt => opt.Ignore())
                .ForMember(p => p.Error, opt => opt.Ignore())
                .ReverseMap();


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

            //Beneficiario
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
