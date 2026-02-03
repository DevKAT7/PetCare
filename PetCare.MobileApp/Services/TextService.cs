namespace PetCare.MobileApp.Services
{
    public class TextService
    {
        private readonly IApiService _apiService;

        private Dictionary<string, string> _texts = new(StringComparer.OrdinalIgnoreCase);

        public bool IsLoaded { get; private set; } = false;

        public TextService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task LoadTextsAsync()
        {
            try
            {
                var texts = await _apiService.GetPageTextsAsync();
                if (texts != null && texts.Any())
                {
                    _texts = texts;
                    IsLoaded = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load texts: {ex.Message}");
            }
        }

        public string this[string key]
        {
            get
            {
                if (_texts.TryGetValue(key, out var value))
                {
                    return value;
                }

                return $"[{key}]";
            }
        }
    }
}
