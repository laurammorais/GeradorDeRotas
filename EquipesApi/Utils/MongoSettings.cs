using EquipesApi.Utils;

namespace UsuarioApi.Utils
{
    public class MongoSettings : IMongoSettings
	{
		public string ConnectionString { get; set; }
		public string EquipeCollectionName { get; set; }
		public string DatabaseName { get; set; }
	}
}