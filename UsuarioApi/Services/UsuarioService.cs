using System.Collections.Generic;
using Models;
using MongoDB.Driver;
using UsuarioApi.Utils;

namespace UsuarioApi.Services
{
    public class UsuarioService
    {
        private readonly IMongoCollection<Usuario> _usuario;
        public UsuarioService(IMongoSettings settings)
        {
            var usuario = new MongoClient(settings.ConnectionString);
            var database = usuario.GetDatabase(settings.DatabaseName);
            _usuario = database.GetCollection<Usuario>(settings.UsuarioCollectionName);
        }

        public List<Usuario> Get() => _usuario.Find(usuario => true).ToList();

        public Usuario GetByUsername(string username) => _usuario.Find(usuario => usuario.Username == username).FirstOrDefault();

        public void Create(Usuario usuario) => _usuario.InsertOne(usuario);

        public void Update(string id, Usuario usuario) => _usuario.ReplaceOne(x => x.Id == id, usuario);
    }
}
