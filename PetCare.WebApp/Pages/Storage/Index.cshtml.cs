using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.StockItems.Dtos;
using PetCare.Application.Features.StockItems.Queries;
using PetCare.Application.Features.StockTransactions.Commands;
using PetCare.Application.Features.StockTransactions.Dtos;
using PetCare.Application.Features.StockTransactions.Queries;

namespace PetCare.WebApp.Pages.Storage
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<StockItemReadModel> StockItems { get; set; } = new();

        [BindProperty]
        public StockTransactionCreateModel TransactionInput { get; set; } = new();

        [BindProperty]
        public string TransactionType { get; set; } = "Add";

        public async Task OnGetAsync()
        {
            StockItems = await _mediator.Send(new GetStockItemsQuery());
        }

        public async Task<IActionResult> OnPostAdjustStockAsync()
        {
            if (!ModelState.IsValid)
            {
                StockItems = await _mediator.Send(new GetStockItemsQuery());
                return Page();
            }

            if (TransactionType == "Remove")
            {
                TransactionInput.QuantityChange = -Math.Abs(TransactionInput.QuantityChange);
            }
            else
            {
                TransactionInput.QuantityChange = Math.Abs(TransactionInput.QuantityChange);
            }

            try
            {
                await _mediator.Send(new CreateStockTransactionCommand { StockTransaction = TransactionInput });
                TempData["SuccessMessage"] = "Stock adjusted successfully.";
            }
            catch (BadRequestException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToPage();
        }

        //handler AJAX dla historii
        public async Task<JsonResult> OnGetHistoryAsync(int medicationId)
        {
            var transactions = await _mediator.Send(new GetStockTransactionsQuery { MedicationId = medicationId });
            return new JsonResult(transactions);
        }
    }
}