using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace practicode5
{
    internal class HtmlHelper
    {
        private static readonly HtmlHelper instance = new HtmlHelper();

        public static HtmlHelper Instance => instance;

        public List<string>? HtmlTags { get; set; }

        public List<string>? HtmlVoidTags { get; set; }

        private HtmlHelper()
        {
            string jsonContent = File.ReadAllText("HtmlTags.json");
            HtmlTags = JsonSerializer.Deserialize<List<string>>(jsonContent);
            jsonContent = File.ReadAllText("HtmlVoidTags.json");
            HtmlVoidTags = JsonSerializer.Deserialize<List<string>>(jsonContent);
        }
    }
}
