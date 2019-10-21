using System;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scannit.Extensions
{
    [ContentProperty("Text")]
    public class LocaleExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
            {
                return string.Empty;
            }

            var translation = AppResources.ResourceManager.GetString(Text);
            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException($"Key '{Text}', not found in AppResources for the culture: {Thread.CurrentThread.CurrentUICulture.Name}.");
#else
                translation = $"#{Text}#"; // Displays the key to the user. Oh no!
#endif
            }

            return translation;
        }
    }
}
