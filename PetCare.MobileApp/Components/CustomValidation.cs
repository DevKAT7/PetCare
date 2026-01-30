using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace PetCare.MobileApp.Components
{
    public class CustomValidation : ComponentBase
    {
        private ValidationMessageStore? _messageStore;

        [CascadingParameter]
        private EditContext? CurrentEditContext { get; set; }

        [Inject]
        private ILogger<CustomValidation> Logger { get; set; } = default!;

        protected override void OnInitialized()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException($"{nameof(CustomValidation)} requires a cascading parameter of type {nameof(EditContext)}.");
            }

            _messageStore = new ValidationMessageStore(CurrentEditContext);
            CurrentEditContext.OnValidationRequested += (s, e) => _messageStore.Clear();
            CurrentEditContext.OnFieldChanged += (s, e) => _messageStore.Clear(e.FieldIdentifier);
        }

        public void DisplayErrors(IDictionary<string, string[]> errors)
        {
            if (CurrentEditContext == null || _messageStore == null) return;

            _messageStore.Clear();

            foreach (var error in errors)
            {
                var incomingKey = error.Key;

                if (incomingKey.Contains('.'))
                {
                    incomingKey = incomingKey.Split('.').Last();
                }

                var property = CurrentEditContext.Model.GetType().GetProperties()
                    .FirstOrDefault(p => string.Equals(p.Name, incomingKey, StringComparison.OrdinalIgnoreCase));

                if (property != null)
                {
                    var fieldIdentifier = CurrentEditContext.Field(property.Name);
                    _messageStore.Add(fieldIdentifier, error.Value);
                }
                else
                {
                    Logger.LogWarning("Validation mismatch! API returned error for field '{ApiKey}' " +
                        "(trimmed: '{TrimmedKey}'), but no such property found in form model '{ModelType}'. Error: {ErrorMsg}",
                        error.Key,
                        incomingKey,
                        CurrentEditContext.Model.GetType().Name,
                        error.Value[0]);
                }
            }

            CurrentEditContext.NotifyValidationStateChanged();
        }

        public void ClearErrors()
        {
            _messageStore?.Clear();
            CurrentEditContext?.NotifyValidationStateChanged();
        }
    }
}
