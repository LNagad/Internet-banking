using AutoMapper;
using Core.Application.Dtos.Account;
using Core.Application.ViewModels.Beneficiarios;
using Core.Application.ViewModels.CuentaAhorros;
using Core.Application.ViewModels.Pagos.PagosTarjetaCredito;
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
            #region product

            CreateMap<Product, ProductViewModel>()
                .ForMember(P => P.User, opt => opt.Ignore())
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
                .ForMember(P => P.CuentaAhorros, opt => opt.Ignore())
                .ForMember(P => P.TarjetaCreditos, opt => opt.Ignore())
                .ForMember(P => P.Prestamos, opt => opt.Ignore());

            #endregion

            #region account
          
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

            #endregion

            #region tarjeta_credito

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


            //CreateMap<SaveTarjetaCreditoViewModel, SavePagoTarjetaViewModel>()
            //    .ReverseMap()
            //    .ForMember(P => P.Created, opt => opt.Ignore())
            //    .ForMember(P => P.CreatedBy, opt => opt.Ignore())
            //    .ForMember(P => P.LastModified, opt => opt.Ignore())
            //    .ForMember(P => P.LastModifiedBy, opt => opt.Ignore())
            //    .ForMember(P => P.Product, opt => opt.Ignore()); 

            #endregion

            #region prestamos

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

            #endregion

            #region cuentaAhorro

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


            CreateMap<CuentaAhorroViewModel, SaveCuentaAhorroViewModel>()
                .ReverseMap()
                .ForMember(P => P.Product, opt => opt.Ignore())
                .ForMember(P => P.Beneficiarios, opt => opt.Ignore());

            #endregion

            #region beneficiarios

            CreateMap<Beneficiario, BeneficiarioViewModel>()
                .ReverseMap()
                .ForMember(P => P.Created, opt => opt.Ignore())
                .ForMember(P => P.CreatedBy, opt => opt.Ignore())
                .ForMember(P => P.LastModified, opt => opt.Ignore())
                .ForMember(P => P.LastModifiedBy, opt => opt.Ignore());

            CreateMap<Beneficiario, SaveBeneficiarioViewModel>()
                .ForMember(P => P.HasError, opt => opt.Ignore())
                .ForMember(P => P.Error, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(P => P.Created, opt => opt.Ignore())
                .ForMember(P => P.CreatedBy, opt => opt.Ignore())
                .ForMember(P => P.LastModified, opt => opt.Ignore())
                .ForMember(P => P.LastModifiedBy, opt => opt.Ignore())
                .ForMember(P => P.CuentaAhorro, opt => opt.Ignore());

            #endregion
        }
    }
}
