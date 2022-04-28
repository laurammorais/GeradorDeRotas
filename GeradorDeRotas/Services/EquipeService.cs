using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;

namespace GeradorDeRotas.Services
{
    public class EquipeService
    {
        private readonly HttpClient httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:44387/api/equipes/") };

        public async Task<List<Equipe>> Get()
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync("");

            if (!response.IsSuccessStatusCode)
                return null;

            var equipeString = await response.Content.ReadAsStringAsync();
            var equipes = JsonConvert.DeserializeObject<List<Equipe>>(equipeString);

            return equipes;
        }

        public async Task<List<Equipe>> GetDisponivel()
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync("Disponivel");

            if (!response.IsSuccessStatusCode)
                return new List<Equipe>();

            var equipeString = await response.Content.ReadAsStringAsync();
            var equipes = JsonConvert.DeserializeObject<List<Equipe>>(equipeString);

            return equipes;
        }

        public async Task<Equipe> Get(string id)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync(id);

            if (!response.IsSuccessStatusCode)
                return null;

            var equipeString = await response.Content.ReadAsStringAsync();
            var equipe = JsonConvert.DeserializeObject<Equipe>(equipeString);

            return equipe;
        }

        public async Task<Equipe> GetByCpf(string cpf)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync($"Cpf/{cpf}");

            if (!response.IsSuccessStatusCode)
                return null;

            var equipeString = await response.Content.ReadAsStringAsync();
            var equipe = JsonConvert.DeserializeObject<Equipe>(equipeString);

            return equipe;
        }

        public async Task<List<Equipe>> GetDisponivelByCidade(string cidade)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync($"Cidade/{cidade}");

            if (!response.IsSuccessStatusCode)
                return null;

            var equipeString = await response.Content.ReadAsStringAsync();
            var equipe = JsonConvert.DeserializeObject<List<Equipe>>(equipeString);

            return equipe;
        }

        public async Task Create(Equipe equipe)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            await httpClient.PostAsJsonAsync("", equipe);
        }

        public async Task Update(string id, Equipe equipe)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            await httpClient.PutAsJsonAsync(id, equipe);
        }

        public async Task Remove(string id)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            await httpClient.DeleteAsync(id);
        }
    }
}
