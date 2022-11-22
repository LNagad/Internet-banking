using AutoMapper;
using Core.Application.Dtos.Pagos;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.CuentaAhorros;
using Core.Application.ViewModels.Pagos;
using Core.Application.ViewModels.Pagos.PagosExpresos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public class PagosService : IPagosService
    {
        private readonly ICuentaAhorroService cuentaAhorroService;
        private readonly IProductService productService;
        private readonly IDashboradService userService;
        private readonly IMapper _mapper;

        public PagosService(ICuentaAhorroService cuentaAhorroService, IProductService productService, IDashboradService userService, IMapper mapper)
        {
            this.cuentaAhorroService = cuentaAhorroService;
            this.productService = productService;
            this.userService = userService;
            _mapper = mapper;
        }


        public async Task<PagoExpressResponse> PagoExpress(SavePagoExpresoViewModel vm)
        {
            PagoExpressResponse response = new();
            response.HasError = false;

            double parsedMonto = double.Parse(vm.Monto);
            parsedMonto = Math.Round(parsedMonto, 2);


            CuentaAhorroViewModel cuentaOrigen = await cuentaAhorroService.AccountExists(vm.NumeroCuentaOrigen);

            if (cuentaOrigen == null)
            {
                response.HasError = true;
                response.Error = "La cuenta de origen seleccionada no existe!";
                return response;
            }

            CuentaAhorroViewModel cuentaDestino = await cuentaAhorroService.AccountExists(vm.NumeroCuentaDestino);


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

            var userOrigen = await userService.getUserAndInformation(cuentaOrigen.Product.IdUser);
            var userDestino = await userService.getUserAndInformation(cuentaDestino.Product.IdUser);


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

            CuentaAhorroViewModel cuentaOrigen = await cuentaAhorroService.AccountExists(vm.NumeroCuentaOrigen);


            if (cuentaOrigen == null)
            {
                response.HasError = true;
                response.Error = "La cuenta de origen seleccionada no existe!";
                return response;
            }

            var cuentaOrigenVM = _mapper.Map<SaveCuentaAhorroViewModel>(cuentaOrigen);

            CuentaAhorroViewModel cuentaDestino = await cuentaAhorroService.AccountExists(vm.NumeroCuentaDestino);

            if (cuentaDestino == null)
            {
                response.HasError = true;
                response.Error = "Cuenta de destino no encontrada.";
                return response;
            }

            //Update balances

            var cuentaDestinoVM = _mapper.Map<SaveCuentaAhorroViewModel>(cuentaDestino);

            cuentaOrigenVM.Balance -= vm.Monto;

            await cuentaAhorroService.Update(cuentaOrigenVM, cuentaOrigenVM.Id);

            cuentaDestinoVM.Balance += vm.Monto;

            await cuentaAhorroService.Update(cuentaDestinoVM, cuentaDestinoVM.Id);


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

    }
  
}
