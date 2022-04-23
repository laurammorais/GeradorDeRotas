using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;

namespace GeradorDeRotas.Services
{
	public class UsuarioService
    {
        HttpClient httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:44300/api/usuarios/") };
        public async Task<Usuario> GetByUsername(string username)
        {

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync(username);

            if (!response.IsSuccessStatusCode)
                return null;

            var usuarioString = await response.Content.ReadAsStringAsync();
            var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioString);

            return usuario;
        }

        public async Task<bool> Post(Usuario usuario)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.PostAsJsonAsync("", usuario);
            return response.IsSuccessStatusCode;
        }
    }
}