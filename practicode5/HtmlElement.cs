using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace practicode5
{
    internal class HtmlElement
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? InnerHtml { get; set; }

        public List<string>? Classes { get; set; }

        public List<string>? Arrtibutes { get; set; }

        public HtmlElement? Parent { get; set; }

        public List<HtmlElement>? Children { get; set; }

        public HtmlElement()
        {
            Classes = new List<string>();
            Arrtibutes = new List<string>();
            Children = new List<HtmlElement>();
            InnerHtml = "";
        }

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> q = new Queue<HtmlElement>();
            q.Enqueue(this);
            while (q.Count > 0)
            {
                HtmlElement current = q.Dequeue();
                foreach (var child in current.Children)
                {
                    q.Enqueue(child);
                }
                yield return current;
            }
        }

        public List<HtmlElement> Ancestors(HtmlElement element)
        {
            List<HtmlElement> parents = new List<HtmlElement>();
            var current = element.Parent;
            while (current != null)
            {
                parents.Add(current);
                current = current.Parent;
            }
            return parents;
        }

        public override string ToString()
        {
            var s= $"Name: {Name} Id: {Id} InnerHtml: {InnerHtml} ";
            foreach (var c in Classes)
            {
                s += c + " ";
            }
            foreach (var a in Arrtibutes)
            {
                s += a + " ";
            }
            return s;
        }

        public List<HtmlElement> searchInTree(int n, Selector selector,
            List<HtmlElement> list)

        {
            Console.WriteLine(this.ToString());
            if (selector == null)
                return list;


            List<HtmlElement>list2=new List<HtmlElement>();
                list = this.Descendants().ToList();
            if (selector != null)
                list = list.Where(el => ((selector.TagName != null && selector.TagName == el.Name) || (selector.Id != null && selector.Id == el.Id) ||
                (selector.Classes != null && selector.Classes.FindIndex
                (c1=>el.Classes.FindIndex(c2=>c1==c2)!=-1)!=-1))).ToList();
            foreach (var child in list)
            {
            list2.AddRange( child.searchInTree(n+1,selector.Child, list));
            }
            return list2;
        }
    }
}
