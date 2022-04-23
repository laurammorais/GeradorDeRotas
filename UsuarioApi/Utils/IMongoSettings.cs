namespace UsuarioApi.Utils
{
	public interface IMongoSettings
	{
		string ConnectionString { get; set; }
		string UsuarioCollectionName { get; set; }
		string DatabaseName { get; set; }
	}
}
