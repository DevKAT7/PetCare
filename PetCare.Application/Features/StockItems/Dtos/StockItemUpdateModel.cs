namespace PetCare.Application.Features.StockItems.Dtos
{
    public class StockItemUpdateModel
    {
        public int CurrentStock { get; set; }
        public int ReorderLevel { get; set; }
    }
}
