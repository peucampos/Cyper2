namespace Cyper2
{
    public interface IDrugProvider
    {
        Task<Drug[]> GetDrugsAsync();
    }
}