using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;

namespace NSE.WebApp.MVC.Controllers
{
    public class IdentidadeController : MainController
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public IdentidadeController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet]
        [Route("nova-conta")]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return View(usuarioRegistro);

            // API - Registro
            var resposta = await _autenticacaoService.Registro(usuarioRegistro);

            if (ResponsePossuiErros(resposta.ResponseResult)) return View(usuarioRegistro);

            // Realizar Login
            await RealizarLogin(resposta);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string ReturnUrl = null)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UsuarioLogin usuarioLogin, string ReturnUrl = null)
        {
            ViewData["ReturnUrl"] = ReturnUrl;

            if (!ModelState.IsValid) return View(usuarioLogin);

            var resposta = await _autenticacaoService.Login(usuarioLogin);

            if (ResponsePossuiErros(resposta.ResponseResult)) return View(usuarioLogin);

            // Realizar Login
            await RealizarLogin(resposta);

            if (string.IsNullOrEmpty(ReturnUrl)) return RedirectToAction("Index", "Home");

            return LocalRedirect(ReturnUrl);
        }

        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task RealizarLogin(UsuarioRespostaLogin resposta)
        {
            var token = ObtemTokenFormatado(resposta.AccessToken);

            var clains = new List<Claim>();
            clains.Add(new Claim("JWT", resposta.AccessToken));
            clains.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(clains, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        private static JwtSecurityToken ObtemTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }
    }
}
