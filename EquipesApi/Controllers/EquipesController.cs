using System.Collections.Generic;
using System.Linq;
using EquipesApi.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EquipesApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EquipesController : ControllerBase
    {
        private readonly EquipeService _equipeService;
        public EquipesController(EquipeService equipeService)
        {
            _equipeService = equipeService;
        }

        // GET: api/<PessoasController>
        [HttpGet]
        public ActionResult<List<Equipe>> GetAll() => _equipeService.Get();

        [HttpGet]
        [Route("Disponivel")]
        public ActionResult<List<Equipe>> GetDisponivel() => _equipeService.GetDisponivel();

        // GET api/<PessoasController>/5
        [HttpGet("{id}")]
        public ActionResult<Equipe> Get(string id)
        {
            var buscarEquipe = _equipeService.Get(id);
            if (buscarEquipe == null)
                return NotFound("Equipe não encontrada");
            return buscarEquipe;
        }

        [HttpGet("Cpf/{cpf}")]
        public ActionResult<Equipe> GetByCpf(string cpf)
        {
            var buscarEquipe = _equipeService.GetByCpf(cpf);
            if (buscarEquipe == null)
                return NotFound("Equipe não encontrada");
            return buscarEquipe;
        }

        [HttpGet("Cidade/{cidade}")]
        public ActionResult<List<Equipe>> GetByCidade(string cidade)
        {
            var equipes = _equipeService.GetByCidade(cidade);
            if (!equipes.Any())
                return NotFound("Nenhuma equipe encontrada");
            return equipes;
        }

        // POST api/<PessoasController>
        [HttpPost]
        public ActionResult Create(Equipe equipe)
        {
            _equipeService.Create(equipe);
            return Ok();
        }

        // PUT api/<PessoasController>/5
        [HttpPut("{id}")]
        public ActionResult<Equipe> Put(string id, Equipe equipe)
        {
            var buscarEquipe = _equipeService.Get(id);

            if (buscarEquipe == null)
                return NotFound();

            _equipeService.Update(id, equipe);
            return Ok();
        }

        // DELETE api/<PessoasController>/5
        [HttpDelete("{id}")]
        public ActionResult<Equipe> Delete(string id)
        {
            var buscarEquipe = _equipeService.Get(id);

            if (buscarEquipe == null)
                return NotFound();

            _equipeService.Remove(id);
            return Ok();
        }
    }
}
