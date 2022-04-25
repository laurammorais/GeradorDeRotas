using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GeradorDeRotas.Models;
using Newtonsoft.Json;

namespace GeradorDeRotas.Services
{
    public class PessoaService
    {
        private readonly HttpClient httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:44327/api/pessoas/") };

        public async Task<List<Pessoa>> Get()
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync("");

            if (!response.IsSuccessStatusCode)
                return null;

            var pessoaString = await response.Content.ReadAsStringAsync();
            var usuarios = JsonConvert.DeserializeObject<List<Pessoa>>(pessoaString);

            return usuarios;
        }

        public async Task<List<Pessoa>> GetDisponivel()
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync("Disponivel");

            if (!response.IsSuccessStatusCode)
                return null;

            var pessoaString = await response.Content.ReadAsStringAsync();
            var usuarios = JsonConvert.DeserializeObject<List<Pessoa>>(pessoaString);

            return usuarios;
        }

        public async Task<Pessoa> GetByCpf(string cpf)
        {

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync($"Cpf/{cpf}");

            if (!response.IsSuccessStatusCode)
                return null;

            var usuarioString = await response.Content.ReadAsStringAsync();
            var usuario = JsonConvert.DeserializeObject<Pessoa>(usuarioString);

            return usuario;
        }

        public async Task<Pessoa> Get(string id)
        {

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync(id);

            if (!response.IsSuccessStatusCode)
                return null;

            var usuarioString = await response.Content.ReadAsStringAsync();
            var usuario = JsonConvert.DeserializeObject<Pessoa>(usuarioString);

            return usuario;
        }

        public async Task<bool> Create(Pessoa pessoa)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.PostAsJsonAsync("", pessoa);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(string id, Pessoa pessoa)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.PutAsJsonAsync(id, pessoa);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Remove(string id)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.DeleteAsync(id);
            return response.IsSuccessStatusCode;
        }
    }
}