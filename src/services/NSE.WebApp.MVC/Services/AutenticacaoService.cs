using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;
        private string URL_BASE;

        public AutenticacaoService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AutenticacaoURL);
            _httpClient = httpClient;
            URL_BASE = settings.Value.AutenticacaoURL;
        }

        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
        {
            var loginContent = ObterConteudo(usuarioLogin);

            var response = await _httpClient.PostAsync($"{URL_BASE}/identidade/autenticar", loginContent);
            
            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjeto<ResponseResult>(response)
                };
            }

            return await DeserializarObjeto<UsuarioRespostaLogin>(response);
        }

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro)
        {
            var registroContent = ObterConteudo(usuarioRegistro);

            var response = await _httpClient.PostAsync($"{URL_BASE}/identidade/nova-conta", registroContent);

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjeto<ResponseResult>(response)
                };
            }

            return await DeserializarObjeto<UsuarioRespostaLogin>(response);
        }
    }
}