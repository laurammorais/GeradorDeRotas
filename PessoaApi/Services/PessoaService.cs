using System.Collections.Generic;
using Models;
using MongoDB.Driver;
using PessoaApi.Utils;

namespace PessoaApi.Services
{
	public class PessoaService
    {
        private readonly IMongoCollection<Pessoa> _pessoa;
        public PessoaService(IMongoSettings settings)
        {
            var pessoa = new MongoClient(settings.ConnectionString);
            var database = pessoa.GetDatabase(settings.DatabaseName);
            _pessoa = database.GetCollection<Pessoa>(settings.PessoaCollectionName);
        }

        public List<Pessoa> Get() => _pessoa.Find(pessoa => true).ToList();

        public List<Pessoa> GetDisponivel() => _pessoa.Find(pessoa => pessoa.Disponivel).ToList();

        public Pessoa Get(string id) => _pessoa.Find(pessoa => pessoa.Id == id).FirstOrDefault();

        public Pessoa GetByCpf(string cpf) => _pessoa.Find(pessoa => pessoa.Cpf == cpf).FirstOrDefault();

        public void Create(Pessoa pessoa) => _pessoa.InsertOne(pessoa);

        public void Update(string id, Pessoa pessoa) => _pessoa.ReplaceOne(x => x.Id == id, pessoa);

        public void Remove(string id) => _pessoa.DeleteOne(pessoa => pessoa.Id == id);
    }
}
