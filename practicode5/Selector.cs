using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practicode5
{
    internal class Selector
    {
        public string TagName { get; set; }

        public string Id { get; set; }

        public List<string> Classes { get; set; }

        public Selector Parent { get; set; }

        public Selector Child { get; set; }

        public static Selector createSelector(string selectorString)
        {
            List<string> selectorsStrings = selectorString.Split(' ').ToList();
            HtmlHelper htmlHelper = HtmlHelper.Instance;
            Selector root = new Selector();
            Selector currentSelector = root;
            foreach (var sel in selectorsStrings)
            {          
                Selector selector = new Selector();
                var strings = sel.Split('#','.').ToList();
                foreach (var s in strings)
                {
                    if (s.Length>0&&s[0] == '.')
                    {
                        selector.Classes.Add(s);
                    }
                    else
                        if (s.Length > 0 && s[0] == '#')
                    {
                        selector.Id = s;
                    }
                    else
                    {
                        if (htmlHelper.HtmlTags.Contains(s) || htmlHelper.HtmlVoidTags.Contains(s))
                        {
                            selector.TagName = s;
                        }
                    }
                }
                currentSelector.Child = selector;
                selector.Parent = currentSelector;
                currentSelector = selector;
            }
            return root.Child;
        }
    }
}
