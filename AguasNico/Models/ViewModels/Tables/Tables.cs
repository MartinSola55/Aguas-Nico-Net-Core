
namespace AguasNico.Models.ViewModels.Tables
{
    public class SoldProductsTable
    {
        public string Name { get; set; } = null!;
        public int Dispatched { get; set; } = 0;
        public int Sold { get; set; } = 0;
        public int Returned { get; set; } = 0;
    }
}
