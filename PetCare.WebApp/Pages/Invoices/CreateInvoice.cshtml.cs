using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Invoices.Commands;
using PetCare.Application.Features.Invoices.Dto;
using PetCare.Application.Features.Invoices.Queries;
using ValidationException = PetCare.Application.Exceptions.ValidationException;

namespace PetCare.WebApp.Pages.Invoices
{
    public class CreateInvoiceModel : PageModel
    {
        private readonly IMediator _mediator;

        public CreateInvoiceModel(IMediator mediator) => _mediator = mediator;

        [BindProperty]
        public InvoiceCreateModel Input { get; set; } = new();

        public string OwnerName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int appointmentId)
        {
            var prepareData = await _mediator.Send(new GetInvoicePrepareModelQuery(appointmentId));

            Input.AppointmentId = prepareData.AppointmentId;
            Input.PetOwnerId = prepareData.PetOwnerId;
            Input.InvoiceDate = DateTime.Today;
            Input.DueDate = DateTime.Today.AddDays(14);
            Input.Items = prepareData.SuggestedItems;

            OwnerName = prepareData.PetOwnerName;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            for (int i = 0; i < Input.Items.Count; i++)
            {
                var item = Input.Items[i];

                //dajê tutaj dodatkow¹ walidacjê, aby pomin¹æ weryfikacjê usuniêtych wierszy
                //musia³am usunaæ walidacjê po stronie klienta, poniewa¿ nie dzia³a³a poprawnie z dynamicznie dodawanymi/usuwanymi wierszami
                bool isDeletedRow = item.Description == "DELETED" && item.Quantity == 0;

                if (!isDeletedRow)
                {

                    if (string.IsNullOrWhiteSpace(item.Description))
                    {
                        ModelState.AddModelError($"Input.Items[{i}].Description", "Description is required.");
                    }

                    if (item.Quantity <= 0)
                    {
                        ModelState.AddModelError($"Input.Items[{i}].Quantity", "Quantity must be at least 1.");
                    }

                    if (item.UnitPrice < 0)
                    {
                        ModelState.AddModelError($"Input.Items[{i}].UnitPrice", "Price cannot be negative.");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                await ReloadViewData();
                return Page();
            }

            try
            {
                var invoiceId = await _mediator.Send(new CreateInvoiceCommand { Invoice = Input });

                TempData["SuccessMessage"] = "Invoice issued successfully.";
                return RedirectToPage("/Invoices/Details", new { id = invoiceId });
            }
            catch (ValidationException ex)
            {
                foreach (var entry in ex.Errors)
                {
                    string propertyName = entry.Key;
                    string[] errorMessages = entry.Value;

                    foreach (var errorMessage in errorMessages)
                    {
                        
                        ModelState.AddModelError($"Input.{propertyName}", errorMessage);
                    }
                }
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, $"Data error: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while creating the invoice. " + ex.Message);
            }

            await ReloadViewData();
            return Page();
        }

        private async Task ReloadViewData()
        {
            try
            {
                var prepareData = await _mediator.Send(new GetInvoicePrepareModelQuery(Input.AppointmentId));
                OwnerName = prepareData.PetOwnerName;
            }
            catch
            {
                OwnerName = "Unknown (Error loading data)";
            }
        }
    }
}
