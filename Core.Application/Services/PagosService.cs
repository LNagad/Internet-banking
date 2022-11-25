using AutoMapper;
using Core.Application.Dtos.Pagos;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.CuentaAhorros;
using Core.Application.ViewModels.Pagos;
using Core.Application.ViewModels.Pagos.PagoAvance;
using Core.Application.ViewModels.Pagos.PagosBeneficiarios;
using Core.Application.ViewModels.Pagos.PagosExpresos;
using Core.Application.ViewModels.Pagos.PagosTarjetaCredito;
using Core.Application.ViewModels.Prestamos;
using Core.Application.ViewModels.TarjetaCreditos;


namespace Core.Application.Services
{
    public class PagosService : IPagosService
    {
        private readonly ICuentaAhorroService _cuentaAhorroService;
        private readonly ITarjetaCreditoService _tarjetaCreditoService;
        private readonly IProductService _productService;
        private readonly IDashboradService _userService;
        private readonly IPrestamoService _prestamoService;
        private readonly IBeneficiarioService _beneficiarioService;

        private readonly IMapper _mapper;

        private readonly ITarjetaCreditoRepository _tarjetaRepo;
        private readonly IPrestamoRepository _prestamoRepo;
        private readonly ITransactionRepository _transactionRepo;

        public PagosService(ICuentaAhorroService cuentaAhorroService, IProductService productService, 
            IDashboradService userService, IMapper mapper, ITarjetaCreditoService tarjetaCreditoService, 
            ITarjetaCreditoRepository tarjetaRepo, IPrestamoRepository prestamoRepo, IPrestamoService prestamoService,
            IBeneficiarioService beneficiarioService, ITransactionRepository transactionRepo)
        {
            _cuentaAhorroService = cuentaAhorroService;
            _productService = productService;
            _userService = userService;
            _tarjetaCreditoService = tarjetaCreditoService;
            _prestamoService = prestamoService;
            _beneficiarioService = beneficiarioService;

            _tarjetaRepo = tarjetaRepo;
            _prestamoRepo = prestamoRepo;
            _mapper = mapper;

            _transactionRepo = transactionRepo;
        }

        #region pagosExpresosos
        
        public async Task<PagoExpressResponse> PagoExpress(SavePagoExpresoViewModel vm)
        {
            PagoExpressResponse response = new();
            response.HasError = false;

            double parsedMonto = double.Parse(vm.Monto);
            parsedMonto = Math.Round(parsedMonto, 2);

            if (vm.NumeroCuentaOrigen == vm.NumeroCuentaDestino)
            {
                response.HasError = true;
                response.Error = "No puede usar la misma cuenta de origen!";
                return response;
            }

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

            if (vm.NumeroCuentaOrigen == vm.NumeroCuentaDestino)
            {
                response.HasError = true;
                response.Error = "No puede usar la misma cuenta de origen!";
                return response;
            }

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
            
            var transaction = await _transactionRepo.AddAsync(new Domain.Entities.Transaction()
            {
                UserId = cuentaOrigen.Product.IdUser,
                FromId = cuentaOrigen.Id,
                ProductFromId = cuentaOrigen.Product.Id,
                ToId = cuentaDestino.Product.IdUser,
                ProductToId = cuentaDestino.Product.Id,
                isCuentaAhorro = true,
                isTarjetaCredito = false,
                isPrestamo = false
            });

            //Return vw props

            response.FirstNameOrigen = vm.FirstNameOrigen;
            response.LastNameOrigen = vm.LastNameOrigen;
            response.NumeroCuentaOrigen = cuentaOrigen.NumeroCuenta;

            response.FirstNameDestino = vm.FirstNameDestino;
            response.LastNameDestino = vm.LastNameDestino;
            response.NumeroCuentaDestino = cuentaDestino.NumeroCuenta;

            response.Monto = vm.Monto;

            response.isCuentaAhorro = true;
            response.TransactionId = transaction.Id;

            return response;
        }

        #endregion

        #region PagosTarjeta

        public async Task<PagoTarjetaResponse> SendPaymentTarjeta(SavePagoTarjetaViewModel pagoVm)
        {
            PagoTarjetaResponse response = new();
            response.HasError = false;

            var getTarjeta = await _tarjetaRepo.TarjetaExist(pagoVm.idProduct);

            double parsedMonto = double.Parse(pagoVm.Monto);


            if (getTarjeta == null)
            {
                response.HasError = true;
                response.Error = "La tarjeta seleccionada no existe!";

                return response;
            }

            if (getTarjeta.Debe == 0)
            {
                response.HasError = true;
                response.Error = "Usted aun no debe esta tarjeta!";

                return response;
            }


            double debeTarjeta = getTarjeta.Debe;

            CuentaAhorroViewModel cuentaOrigen = await _cuentaAhorroService.AccountExists(pagoVm.NumeroCuentaOrigen);

            if (cuentaOrigen == null)
            {
                response.HasError = true;
                response.Error = "La cuenta seleccionada no existe!";

                return response;
            }

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
            } 
            else
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

            //Transaction Create transactin Id

            var transaction = await _transactionRepo.AddAsync(new Domain.Entities.Transaction()
            {
                UserId = cuentaOrigen.Product.IdUser,
                FromId = cuentaOrigen.Id,
                ProductFromId = cuentaOrigen.Product.Id,
                ToId = getTarjeta.Id,
                ProductToId = getTarjeta.IdProduct,
                isCuentaAhorro = false,
                isTarjetaCredito = true,
                isPrestamo = false
            });

            response.FirstNameOrigen = userOrigen.FirstName;
            response.LastNameOrigen = userOrigen.LastName;
            response.LastTarjetaCredito = getTarjeta.NumeroTarjeta;
            response.Monto= parsedMonto;
            response.NumeroCuentaOrigen = cuentaOrigen.NumeroCuenta;

            response.TransactionId = transaction.Id;

            return response;
        }

     
        public async Task<PagoPrestamoResponse> SendPaymentPrestamo(SavePagoPrestamoViewModel prestamoVm)
        {
            var response = new PagoPrestamoResponse();
            response.HasError = false;

            var getPrestamo = await _prestamoRepo.PrestamoExist(prestamoVm.idProduct);


            if (getPrestamo == null)
            {
                response.HasError = true;
                response.Error = "El Prestamo a pagar no existe!";

                return response;
            }

            CuentaAhorroViewModel cuentaOrigen = await _cuentaAhorroService.AccountExists(prestamoVm.NumeroCuentaOrigen);

            if (cuentaOrigen == null)
            {
                response.HasError = true;
                response.Error = "La cuenta seleccionada no existe!";

                return response;
            }

            if (prestamoVm.Monto > cuentaOrigen.Balance)
            {
                response.HasError = true;
                response.Error = "Usted no cuenta con los fondos suficientes para esta transaccion";

                return response;
            }

            if (prestamoVm.Monto > getPrestamo.Debe)
            {
                prestamoVm.Monto = getPrestamo.Debe;

                cuentaOrigen.Balance -= getPrestamo.Debe;

                getPrestamo.Debe = 0;
            }
            else
            {
                cuentaOrigen.Balance -= prestamoVm.Monto;

                getPrestamo.Debe -= prestamoVm.Monto;
            }


            //updating

            var cuentaOrigenVm = _mapper.Map<SaveCuentaAhorroViewModel>(cuentaOrigen);

            await _cuentaAhorroService.Update(cuentaOrigenVm, cuentaOrigenVm.Id);

            SavePrestamoViewModel prestamo = new();
            prestamo.Id = getPrestamo.Id;
            prestamo.Debe = getPrestamo.Debe;
            prestamo.Pago = getPrestamo.Pago + prestamoVm.Monto;

            await _prestamoService.Update(prestamo, prestamo.Id);

            var userOrigen = await _userService.getUserAndInformation(cuentaOrigen.Product.IdUser);

            //Transaction Create transactin Id

            var transaction = await _transactionRepo.AddAsync(new Domain.Entities.Transaction()
            {
                UserId = cuentaOrigen.Product.IdUser,
                FromId = cuentaOrigen.Id,
                ProductFromId = cuentaOrigen.Product.Id,
                ToId = getPrestamo.Id,
                ProductToId = getPrestamo.IdProduct,
                isCuentaAhorro = false,
                isTarjetaCredito = false,
                isPrestamo = true
            });

            response.NumeroCuentaOrigen = cuentaOrigen.NumeroCuenta;
            response.FirstNameOrigen = userOrigen.FirstName;
            response.LastNameOrigen = userOrigen.LastName;
            response.NumeroPrestamo = getPrestamo.NumeroPrestamo;
            response.Monto = prestamoVm.Monto;

            response.TransactionId = transaction.Id;

            return response;
        }

        #endregion
        
        
        #region Beneficiario

        public async Task<PagoBeneficiarioResponse> SendPaymentBeneficiario(SavePagoBeneficiariosViewModel beneficiarioVm)
        {
            var response = new PagoBeneficiarioResponse();
            response.HasError = false;

            CuentaAhorroViewModel cuentaOrigen = await _cuentaAhorroService.AccountExists(beneficiarioVm.NumeroCuentaOrigen);
            
            if (cuentaOrigen == null)
            {
                response.HasError = true;
                response.Error = "La cuenta seleccionada no existe!";

                return response;
            }

            CuentaAhorroViewModel cuentaDestino = await _cuentaAhorroService.AccountExists(beneficiarioVm.NumeroCuentaDestino);
            
            
            if (cuentaDestino == null)
            {
                response.HasError = true;
                response.Error = "Cuenta de destino no encontrada.";
                return response;
            }
            
            if (cuentaOrigen.Balance < beneficiarioVm.Monto)
            {
                response.HasError = true;
                response.Error = "Usted no cuenta con los fondos suficientes para esta transaccion.";
                return response;
            }

            if (cuentaOrigen.Balance == 100 && cuentaOrigen.Principal == true)
            {
                response.HasError = true;
                response.Error = "Tu cuenta principal no puede quedar sin dinero.";
                return response;
            }
            
            if (beneficiarioVm.Monto == cuentaOrigen.Balance)
            {
                beneficiarioVm.Monto -= 100;

                cuentaOrigen.Balance -= beneficiarioVm.Monto;

                cuentaDestino.Balance += beneficiarioVm.Monto;
            }
            else
            {
                cuentaOrigen.Balance -= beneficiarioVm.Monto;

                cuentaDestino.Balance += beneficiarioVm.Monto;
            }

            //Transaction Create transactin Id

            var transaction = await _transactionRepo.AddAsync(new Domain.Entities.Transaction()
            {
                UserId = cuentaOrigen.Product.IdUser,
                FromId = cuentaOrigen.Id,
                ProductFromId = cuentaOrigen.Product.Id,
                ToId = cuentaDestino.Product.IdUser,
                ProductToId = cuentaDestino.Product.Id,
                isCuentaAhorro = true,
                isTarjetaCredito = false,
                isPrestamo = false
            });

            //updating

            var cuentaOrigenVm = _mapper.Map<SaveCuentaAhorroViewModel>(cuentaOrigen);

            await _cuentaAhorroService.Update(cuentaOrigenVm, cuentaOrigenVm.Id);

            var cuentaDestinoVm = _mapper.Map<SaveCuentaAhorroViewModel>(cuentaDestino);

            await _cuentaAhorroService.Update(cuentaDestinoVm, cuentaDestino.Id);

            var userOrigen = await _userService.getUserAndInformation(cuentaOrigen.Product.IdUser);

            var userDestino = await _userService.getUserAndInformation(cuentaDestino.Product.IdUser);

            response.NumeroCuentaOrigen = cuentaOrigen.NumeroCuenta;
            response.FirstNameOrigen = userOrigen.FirstName;
            response.LastNameOrigen = userOrigen.LastName;
            response.Monto = beneficiarioVm.Monto;
            response.LastNameOrigen = beneficiarioVm.BeneficiarioLastName;

            response.FirstNameDestino = userDestino.FirstName;
            response.LastNameDestino = userDestino.LastName;
            response.NumeroCuentaDestino = cuentaDestino.NumeroCuenta;

            response.TransactionId = transaction.Id;

            return response;
        }

        #endregion
        

        #region Avance_pago

        public async Task<PagoAvanceEfectivoResponse> GetAvancePago(SavePagoAvanceViewModel avancePagoVm)
        {
            var response = new PagoAvanceEfectivoResponse();
            response.HasError = false;
            
            CuentaAhorroViewModel cuentaOrigin = await _cuentaAhorroService.AccountExists(avancePagoVm.NumeroCuentaOrigen);

            response.oldMontoCuenta = cuentaOrigin.Balance;
            
            if (cuentaOrigin == null)
            {
                response.HasError = true;
                response.Error = "La cuenta seleccionada no existe!";

                return response;
            }
            
            var getTarjeta = await _tarjetaRepo.TarjetaExist(avancePagoVm.IdProduct);
            
            if (getTarjeta == null)
            {
                response.HasError = true;
                response.Error = "La tarjeta seleccionada no existe!";

                return response;
            }

            double tarjetaLimite = getTarjeta.Limite;

            if (avancePagoVm.Monto > tarjetaLimite)
            {
                response.HasError = true;
                response.Error = "La Tarjeta no posee el Monto Solicitado";

                return response;
            }

            double resultadoTasa = new();

            if ( avancePagoVm.Monto <= tarjetaLimite )
            {
                cuentaOrigin.Balance += avancePagoVm.Monto;

                double tasa = 6.25;
                
                resultadoTasa = ((tasa * avancePagoVm.Monto) / 100) + avancePagoVm.Monto ;

                getTarjeta.Debe += resultadoTasa;

                getTarjeta.Limite -= avancePagoVm.Monto;
            }
            
            //updating
            
            var cuentaOrigenVm = _mapper.Map<SaveCuentaAhorroViewModel>(cuentaOrigin);

            await _cuentaAhorroService.Update(cuentaOrigenVm, cuentaOrigenVm.Id);

            SaveTarjetaCreditoViewModel tarjeta = new();
            tarjeta.Id = getTarjeta.Id;
            tarjeta.Debe = getTarjeta.Debe;

            await _tarjetaCreditoService.Update(tarjeta, tarjeta.Id);

            var userOrigen = await _userService.getUserAndInformation(cuentaOrigin.Product.IdUser);
            response.FirstNameOrigen = userOrigen.FirstName;
            response.LastNameOrigen = userOrigen.LastName;
            response.NumeroTarjeta = getTarjeta.NumeroTarjeta;
            response.Monto = avancePagoVm.Monto;
            response.NumeroCuenta = avancePagoVm.NumeroCuentaOrigen;
            response.MontoCargado = resultadoTasa;
            response.newMontoCuenta = cuentaOrigin.Balance;

            return response;
        }

        #endregion

        #region pagoEntreCuentas

        public async Task<PagoConfirmedViewModel> SendPaymentPagoEntreCuenta(SavePagoEntreCuentas vm)
        {
            PagoConfirmedViewModel response = new();
            response.HasError = false;

            CuentaAhorroViewModel cuentaOrigen = await _cuentaAhorroService.AccountExists(vm.NumeroCuentaOrigen);

            if (vm.NumeroCuentaOrigen == vm.NumeroCuentaDestino)
            {
                response.HasError = true;
                response.Error = "No puede usar la misma cuenta de origen!";
                return response;
            }

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

            var transaction = await _transactionRepo.AddAsync(new Domain.Entities.Transaction()
            {
                UserId = cuentaOrigen.Product.IdUser,
                FromId = cuentaOrigen.Id,
                ProductFromId = cuentaOrigen.Product.Id,
                ToId = cuentaDestino.Product.IdUser,
                ProductToId = cuentaDestino.Product.Id,
                isCuentaAhorro = true,
                isTarjetaCredito = false,
                isPrestamo = false
            });

            var userOrigen = await _userService.getUserAndInformation(cuentaOrigen.Product.IdUser);

            var userDestino = await _userService.getUserAndInformation(cuentaDestino.Product.IdUser);

            //Return vw props

            response.FirstNameOrigen = userOrigen.FirstName;
            response.LastNameOrigen = userOrigen.LastName;
            response.NumeroCuentaOrigen = cuentaOrigen.NumeroCuenta;

            response.FirstNameDestino = userDestino.FirstName;
            response.LastNameDestino = userDestino.FirstName;
            response.NumeroCuentaDestino = cuentaDestino.NumeroCuenta;

            response.Monto = vm.Monto;

            response.isCuentaAhorro = true;
            response.TransactionId = transaction.Id;

            return response;
        }


        #endregion
    }

}
