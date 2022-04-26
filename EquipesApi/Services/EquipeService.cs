using System.Collections.Generic;
using System.Linq;
using EquipesApi.Utils;
using Models;
using MongoDB.Driver;

namespace EquipesApi.Services
{
	public class EquipeService
    {
        private readonly IMongoCollection<Equipe> _equipe;
        public EquipeService(IMongoSettings settings)
        {
            var equipe = new MongoClient(settings.ConnectionString);
            var database = equipe.GetDatabase(settings.DatabaseName);
            _equipe = database.GetCollection<Equipe>(settings.EquipeCollectionName);
        }

        public List<Equipe> Get() => _equipe.Find(equipe => true).ToList();

        public Equipe Get(string id) => _equipe.Find(equipe => equipe.Id == id).FirstOrDefault();

        public Equipe GetByCpf(string cpf) => Get().FirstOrDefault(x => x.Pessoas.Any(x => x.Cpf == cpf));

        public void Create(Equipe equipe) => _equipe.InsertOne(equipe);

        public void Update(string id, Equipe equipe) => _equipe.ReplaceOne(x => x.Id == id, equipe);

        public void Remove(string id) => _equipe.DeleteOne(equipe => equipe.Id == id);
    }
}