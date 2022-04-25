namespace EquipesApi.Utils
{
    public interface IMongoSettings
    {
        string ConnectionString { get; set; }
        string EquipeCollectionName { get; set; }
        string DatabaseName { get; set; }
    }
}