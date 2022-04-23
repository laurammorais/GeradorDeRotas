namespace GeradorDeRotas.Utils
{
    public interface IMongoSettings
    {
        string ConnectionString { get; set; }
        string EquipeCollectionName { get; set; }
        string PessoaCollectionName { get; set; }
        string ExcelCollectionName { get; set; }
        string UsuarioCollectionName { get; set; }
        string DatabaseName { get; set; }
    }
}
