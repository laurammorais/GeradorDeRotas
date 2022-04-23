using System.Collections.Generic;
using System.Linq;
using GeradorDeRotas.Models;
using GeradorDeRotas.Utils;
using MongoDB.Driver;

namespace GeradorDeRotas.Services
{
    public class EquipeService
    {
        private readonly IMongoCollection<Equipe> _equipes;
        public EquipeService(IMongoSettings settings)
        {
            var equipes = new MongoClient(settings.ConnectionString);
            var database = equipes.GetDatabase(settings.DatabaseName);
            _equipes = database.GetCollection<Equipe>(settings.EquipeCollectionName);
        }

        public List<Equipe> Get() => _equipes.Find(equipe => true).ToList();

        public Equipe Get(string id) => _equipes.Find(equipe => equipe.Id == id).FirstOrDefault();

        public Equipe GetByCpf(string cpf) => Get().FirstOrDefault(x => x.Pessoas.Any(x => x.Cpf == cpf));

        public void Create(Equipe equipe) => _equipes.InsertOne(equipe);

        public void Update(string id, Equipe equipe) => _equipes.ReplaceOne(x => x.Id == id, equipe);

        public void Remove(string id) => _equipes.DeleteOne(equipe => equipe.Id == id);
    }
}
