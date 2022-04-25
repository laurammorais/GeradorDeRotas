using System.Collections.Generic;
using GeradorDeRotas.Models;
using Microsoft.AspNetCore.Mvc;
using PessoaApi.Services;

namespace PessoaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly PessoaService _pessoaService;
        public PessoasController(PessoaService pessoaService) => _pessoaService = pessoaService;


        [HttpGet]
        public ActionResult<List<Pessoa>> GetAll() => _pessoaService.Get();

     
        [HttpGet]
        [Route("Cpf/{cpf}")]
        public ActionResult<Pessoa> GetByCpf(string cpf)
        {
            var buscarPessoa = _pessoaService.GetByCpf(cpf);
            if (buscarPessoa == null)
                return NotFound("Pessoa não encontrada");
            return buscarPessoa;
        }

        [HttpGet]
        [Route("Disponivel")]
        public ActionResult<List<Pessoa>> GetDisponivel() => _pessoaService.GetDisponivel();


        [HttpGet("{id}")]
        public ActionResult<Pessoa> Get(string id)
        {
            var buscarPessoa = _pessoaService.Get(id);
            if (buscarPessoa == null)
                return NotFound("Pessoa não encontrada");
            return buscarPessoa;
        }

        [HttpPost]
        public ActionResult<Pessoa> Create(Pessoa pessoa)
        {
            var buscarPessoa = _pessoaService.GetByCpf(pessoa.Cpf);

            if (buscarPessoa != null)
                return BadRequest("Pessoa já cadastrada");

            _pessoaService.Create(pessoa);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult<Pessoa> Put(string id, Pessoa pessoa)
        {
            var buscarPessoa = _pessoaService.Get(id);

            if (buscarPessoa == null)
                return NotFound();

            _pessoaService.Update(id, pessoa);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Pessoa> Delete(string id)
        {
            var buscarPessoa = _pessoaService.Get(id);

            if (buscarPessoa == null)
                return NotFound();

            _pessoaService.Remove(id);
            return Ok();
        }
    }
}