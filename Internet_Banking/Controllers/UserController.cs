﻿using Core.Application.Dtos.Account;
using Core.Application.Helpers;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Products;
using Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Middlewares;

namespace Internet_Banking.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ValidateUserSession _validateUser;
        private readonly IManageUserService _manageUserService;

        public UserController(IUserService userService, ValidateUserSession validateUser, IManageUserService manageService) 
        {
            _userService = userService;
            _validateUser = validateUser;
            _manageUserService = manageService;
        }
        
        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }


        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }

            AuthenticationResponse userVm = await _userService.LoginAsync(vm);

            if (userVm != null && userVm.HasError != true)
            {
                HttpContext.Session.Set<AuthenticationResponse>("user", userVm);
                return RedirectToRoute(new { Controller = "Home", Action = "Index" }); 
            } 
            else
            {
                vm.HasError = userVm.HasError;
                vm.Error = userVm.Error;
                return View(vm);
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { Controller = "User", Action = "Index" });
        }

        //[ServiceFilter(typeof(LoginAuthorize))]

        public IActionResult Register(bool editing)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }

            ViewBag.Editing = editing;

            return View(new SaveUserViewModel());
        }
        
        public async Task<IActionResult> Edit(string id)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }

            SaveUserViewModel vm = await _manageUserService.GetSaveuserViewModelById(id);

            return View("Register", vm);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(SaveUserViewModel vm)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }

            await _manageUserService.UpdateUser(vm);
                
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        //[ServiceFilter(typeof(LoginAuthorize))]

        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm)
        {
            if (!_validateUser.HasUser())
            {
                return RedirectToRoute(new { Controller = "User", Action = "Index" });
            }

            if (!_validateUser.IsAdmin())
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var origin = Request.Headers["origin"];

            RegisterResponse response = await _userService.RegisterAsync(vm, origin);

            if(response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }

            return RedirectToRoute(new { Controller = "Home", Action = "Index" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            string response = await _userService.ConfirmEmailAsync(userId, token);

            return View("ConfirmEmail", response);
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var origin = Request.Headers["origin"];

            ForgotPasswordResponse response = await _userService.ForgotPasswordAsync(vm, origin);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }

            return RedirectToRoute(new { Controller = "User", Action = "Index" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult ResetPassword(string Token)
        {
            return View( new ResetPasswordViewModel { Token = Token});
        } 
        
        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            ResetPasswordResponse response = await _userService.ResetPasswordAsync(vm);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }

            return RedirectToRoute(new { Controller = "User", Action = "Index" });
        }

        public IActionResult AccesDenied()
        {
            return View();
        }
    }
}
