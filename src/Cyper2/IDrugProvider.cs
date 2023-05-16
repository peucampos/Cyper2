namespace Cyper2
{
    public interface IDrugProvider
    {
        Task<List<Drug>> GetDrugsAsync();
        Task<List<string>> DrugsInteractAsync(string d1, string d2);
        Task<List<string>> GetCypsForDrug(string d1);
    }
}