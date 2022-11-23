using AutoMapper;
using Core.Application.Dtos.Pagos;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.CuentaAhorros;
using Core.Application.ViewModels.Pagos;
using Core.Application.ViewModels.Pagos.PagosExpresos;
using Core.Application.ViewModels.Pagos.PagosTarjetaCredito;
using Core.Application.ViewModels.Products;
using Core.Application.ViewModels.TarjetaCreditos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public class PagosService : IPagosService
    {
        private readonly ICuentaAhorroService _cuentaAhorroService;
        private readonly ITarjetaCreditoService _tarjetaCreditoService;
        private readonly IProductService _productService;
        private readonly IDashboradService _userService;
        private readonly IMapper _mapper;

        private readonly ITarjetaCreditoRepository _tarjetaRepo;

        public PagosService(ICuentaAhorroService cuentaAhorroService, IProductService productService, 
            IDashboradService userService, IMapper mapper, ITarjetaCreditoService tarjetaCreditoService, ITarjetaCreditoRepository tarjetaRepo)
        {
            _cuentaAhorroService = cuentaAhorroService;
            _productService = productService;
            _userService = userService;
            _mapper = mapper;
            _tarjetaCreditoService= tarjetaCreditoService;
            _tarjetaRepo = tarjetaRepo;
        }

        #region pagosExpresosos
        
        public async Task<PagoExpressResponse> PagoExpress(SavePagoExpresoViewModel vm)
        {
            PagoExpressResponse response = new();
            response.HasError = false;

            double parsedMonto = double.Parse(vm.Monto);
            parsedMonto = Math.Round(parsedMonto, 2);


            CuentaAhorroViewModel cuentaOrigen = await _cuentaAhorroService.AccountExists(vm.NumeroCuentaOrigen);

            if (cuentaOrigen == null)
            {
                response.HasError = true;
                response.Error = "La cuenta de origen seleccionada no existe!";
                return response;
            }

            CuentaAhorroViewModel cuentaDestino = await _cuentaAhorroService.AccountExists(vm.NumeroCuentaDestino);


            if (cuentaDestino == null)
            {
                response.HasError = true;
                response.Error = "Cuenta de destino no encontrada.";
                return response;
            }

            if (cuentaOrigen.Balance < parsedMonto)
            {
                response.HasError = true;
                response.Error = "Usted no cuenta con los fondos suficientes para esta transaccion.";
                return response;
            }

            var userOrigen = await _userService.getUserAndInformation(cuentaOrigen.Product.IdUser);
            var userDestino = await _userService.getUserAndInformation(cuentaDestino.Product.IdUser);


            response.FirstNameOrigen = userOrigen.FirstName;
            response.LastNameOrigen = userOrigen.LastName;
            response.NumeroCuentaOrigen = cuentaOrigen.NumeroCuenta;

            response.FirstNameDestino = userDestino.FirstName;
            response.LastNameDestino = userDestino.LastName;
            response.NumeroCuentaDestino = cuentaDestino.NumeroCuenta;

            response.Monto = parsedMonto;

            return response;
        }

        public async Task<PagoConfirmedViewModel> PagosExpresoConfirmed(PagoExpressResponse vm)
        {
            PagoConfirmedViewModel response = new();
            response.HasError = false;

            CuentaAhorroViewModel cuentaOrigen = await _cuentaAhorroService.AccountExists(vm.NumeroCuentaOrigen);


            if (cuentaOrigen == null)
            {
                response.HasError = true;
                response.Error = "La cuenta de origen seleccionada no existe!";
                return response;
            }

            var cuentaOrigenVM = _mapper.Map<SaveCuentaAhorroViewModel>(cuentaOrigen);

            CuentaAhorroViewModel cuentaDestino = await _cuentaAhorroService.AccountExists(vm.NumeroCuentaDestino);

            if (cuentaDestino == null)
            {
                response.HasError = true;
                response.Error = "Cuenta de destino no encontrada.";
                return response;
            }

            //Update balances

            var cuentaDestinoVM = _mapper.Map<SaveCuentaAhorroViewModel>(cuentaDestino);

            cuentaOrigenVM.Balance -= vm.Monto;

            await _cuentaAhorroService.Update(cuentaOrigenVM, cuentaOrigenVM.Id);

            cuentaDestinoVM.Balance += vm.Monto;

            await _cuentaAhorroService.Update(cuentaDestinoVM, cuentaDestinoVM.Id);


            //Transaction Create transactin Id

            //Return vw props

            response.FirstNameOrigen = vm.FirstNameOrigen;
            response.LastNameOrigen = vm.LastNameOrigen;
            response.NumeroCuentaOrigen = cuentaOrigen.NumeroCuenta;

            response.FirstNameDestino = vm.FirstNameDestino;
            response.LastNameDestino = vm.LastNameDestino;
            response.NumeroCuentaDestino = cuentaDestino.NumeroCuenta;

            response.Monto = vm.Monto;

            response.isCuentaAhorro = true;

            return response;
        }

        #endregion

        #region PagosTarjeta

        public async Task<List<TarjetaCreditoViewModel>> GetAllTarjetasProductViewModel(string id)
        {
            var products = await _productService.GetAllViewModelWithIncludeById(id);

            return products.Where(p => p.isTarjetaCredito == true).Select(p => new TarjetaCreditoViewModel
            {
                Id = p.TarjetaCreditos.Id,
                NumeroTarjeta = p.TarjetaCreditos.NumeroTarjeta,
                Limite = p.TarjetaCreditos.Limite,
                Pago = p.TarjetaCreditos.Pago,
                Debe = p.TarjetaCreditos.Debe,
                Idproduct = p.TarjetaCreditos.Idproduct,
                Product = p.TarjetaCreditos.Product
            }).ToList();
        }

        public async Task<PagoTarjetaResponse> ValidationPaymentTarjeta(SavePagoTarjetaViewModel pagoVm)
        {
            PagoTarjetaResponse response = new();
            response.HasError = false;

            var getTarjeta = await _tarjetaRepo.TarjetaExist(pagoVm.idProduct);

            double parsedMonto = double.Parse(pagoVm.Monto);


            if (getTarjeta == null)
            {
                response.HasError = true;
                response.Error = "La cuenta seleccionada no existe";

                return response;
            }

            double debeTarjeta = getTarjeta.Debe;

            CuentaAhorroViewModel cuentaOrigen = await _cuentaAhorroService.AccountExists(pagoVm.NumeroCuentaOrigen);
            double cuentaBalance = cuentaOrigen.Balance;

            if (parsedMonto > cuentaBalance)
            {
                response.HasError = true;
                response.Error = "Usted no cuenta con los fondos suficientes para esta transaccion";

                return response;
            }

            if (parsedMonto > debeTarjeta)
            {
                parsedMonto = debeTarjeta;

                cuentaBalance -= debeTarjeta;

                debeTarjeta = 0;

                
            } else
            {
                cuentaBalance -= parsedMonto;

                debeTarjeta -= parsedMonto;
            }

            //updating

            cuentaOrigen.Balance = cuentaBalance;

            var cuentaOrigenVm = _mapper.Map<SaveCuentaAhorroViewModel>(cuentaOrigen);

            await _cuentaAhorroService.Update(cuentaOrigenVm, cuentaOrigenVm.Id);

            SaveTarjetaCreditoViewModel tarjeta = new();
            tarjeta.Id = getTarjeta.Id;
            tarjeta.Debe = debeTarjeta;
            tarjeta.Pago = getTarjeta.Pago + parsedMonto;


            await _tarjetaCreditoService.Update(tarjeta, tarjeta.Id);

            var userOrigen = await _userService.getUserAndInformation(cuentaOrigen.Product.IdUser);
            response.FirstNameOrigen = userOrigen.FirstName;
            response.LastNameOrigen = userOrigen.LastName;
            response.LastTarjetaCredito = getTarjeta.NumeroTarjeta;
            response.Monto= parsedMonto;
            response.NumeroCuentaOrigen = cuentaOrigen.NumeroCuenta;

            return response;
        }


        #endregion
    }

}
