namespace UsuarioApi.Utils
{
	public class MongoSettings : IMongoSettings
	{
		public string ConnectionString { get; set; }
		public string UsuarioCollectionName { get; set; }
		public string DatabaseName { get; set; }
	}
}