using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Install
{
    public class TextFileExtension : MarkupExtension
    {
        public string Source { get; set; }

        public TextFileExtension() { }

        public TextFileExtension(string source)
        {
            Source = source;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Source))
                return string.Empty;

            var uri = new Uri(Source, UriKind.RelativeOrAbsolute);
            var resourceStream = Application.GetResourceStream(uri);

            if (resourceStream == null)
                throw new FileNotFoundException("Resource not found", Source);

            using (var reader = new StreamReader(resourceStream.Stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
