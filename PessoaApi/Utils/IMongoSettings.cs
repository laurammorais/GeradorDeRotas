namespace PessoaApi.Utils
{
    public interface IMongoSettings
    {
        string ConnectionString { get; set; }
        string PessoaCollectionName { get; set; }
        string DatabaseName { get; set; }
    }
}
