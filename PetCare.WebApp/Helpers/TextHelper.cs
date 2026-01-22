using Microsoft.AspNetCore.Mvc.Rendering;

namespace PetCare.WebApp.Helpers
{
    public static class TextHelper
    {
        public static string T(this IHtmlHelper html, string key)
        {
            var texts = html.ViewContext.ViewData["PageTexts"] as Dictionary<string, string>;

            return texts != null && texts.TryGetValue(key, out var value) ? value : $"[{key}]";
        }
    }
}
