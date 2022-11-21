using Core.Application.Dtos.Account;
using Core.Application.Dtos.Email;
using Core.Application.Enums;
using Core.Application.Interfaces.Services;
using Core.Domain.Entities;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Core.Application.ViewModels.CuentaAhorros;

namespace Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly ICuentaAhorroService _cuentaAhorro;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IEmailService emailService, ICuentaAhorroService cuentaAhorroService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _cuentaAhorro = cuentaAhorroService;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No se encontro ninguna cuenta registrada con el correo {request.Email}";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Credenciales invalidas para el correo {request.Email}";
                return response;
            }

            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"{request.Email} no ha sido confirmada/o activada ";
                return response;
            }

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.FirstName = user.FirstName;

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new();
            response.HasError = false;

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);

            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"UserName {request.UserName} ya esta registrado en el sistema";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Eail {request.Email} ya esta registrado en el sistema";
                return response;
            }

            //No errors, then send email and create user

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.Phone,
                UserName = request.UserName,
                Cedula = request.Cedula,
                Status = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {

                //dependiendo el tipo de usuario que se escogio en el formulario
                if ( request.UserType == (int)UserType.Cliente )
                {
                    SaveCuentaAhorroViewModel cuentaVm = new SaveCuentaAhorroViewModel(); 
                    cuentaVm.Balance = request.Monto.Value;
                    await _cuentaAhorro.Add(cuentaVm, user.Id);
                    await _userManager.AddToRoleAsync(user, Roles.Basic.ToString()); // se asigna el rol
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }

                var verificationUri = await SendVerificationEmailUrl(user, origin);

                //send email here
                await _emailService.SendAsync(new EmailRequest
                {
                    To = user.Email,
                    Body = $"Please confirm your account visithing this URL {verificationUri}",
                    Subject = "Confirm registration"
                });

            }
            else
            {
                response.HasError = true;
                response.Error = $"Un error ocurrio tratando de crear el usuario.";
                return response;
            }


            return response;
        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return "No se ha encontrado ninguna cuenta con este usuario";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                //AQUI ACTUALIZAR EL STATUS DEL USUARIO
                user.Status = true;
                await _userManager.UpdateAsync(user);
                return $"La cuenta ha sido confirmada para el usuario {user.Email}. Ya puedes iniciar sesion";
            }
            else
            {
                return $"Hubo un error al confirmar la cuenta {user.Email}";
            }
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            ForgotPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No se encontro ninguna cuenta registrada con el correo {request.Email}";

                return response;
            }

            var verificationUri = await SendForgotPasswordUri(user, origin);

            //send email here
            await _emailService.SendAsync(new EmailRequest
            {
                To = user.Email,
                Body = $"Reinicia tu contrase;a visitando esta url {verificationUri}",
                Subject = "Reinicio de contrase;a"
            });


            return response;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No se encontro ninguna cuenta registrada con el correo {request.Email}";

                return response;
            }

            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Hubo un error al intentar cambiar la contrase;a para el usuario {user.Email}";

                return response;
            }

            return response;
        }

        private async Task<string> SendVerificationEmailUrl(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var route = "User/ConfirmEmail";

            var Uri = new Uri(string.Concat($"{origin}/", route));

            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);

            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);


            return verificationUri;
        }

        private async Task<string> SendForgotPasswordUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var route = "User/ResetPassword";

            var Uri = new Uri(string.Concat($"{origin}/", route));

            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "Token", code);

            return verificationUri;
        }

        public async Task<int> getAllUsers()
        {
            var users = _userManager.Users;
            var userCount = users.Count();
            
            return userCount - 2;
        }

        public async Task<int> usersActives()
        {
            var usersActive = _userManager.Users;
            var something = usersActive.Count(x => x.EmailConfirmed == true);

            return something; 
        }

        public async Task<int> usersInactives()
        {
            var usersActive = _userManager.Users;
            var something2 = usersActive.Count(x => x.EmailConfirmed == false);

            return something2;
        }
    }
}
