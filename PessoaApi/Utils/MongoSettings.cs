using PessoaApi.Utils;

namespace UsuarioApi.Utils
{
    public class MongoSettings : IMongoSettings
	{
		public string ConnectionString { get; set; }
		public string PessoaCollectionName { get; set; }
		public string DatabaseName { get; set; }
	}
}