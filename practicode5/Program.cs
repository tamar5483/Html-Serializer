// See https://aka.ms/new-console-template for more information

using practicode5;
using System.Text.RegularExpressions;

string url = "https://moodle.malkabruk.co.il/my/courses.php";

string htmlString = await Load(url);

string cleanHTML = new Regex("\\s+").Replace(htmlString, " ");

var htmlLines = new Regex("<(.*?)>").Split(cleanHTML).Where((s) => s != "" && s != " ")
    .ToList();

int index = htmlLines.FindIndex(s => s.StartsWith("html", StringComparison.OrdinalIgnoreCase));

if (index != -1)
{
    htmlLines = htmlLines.GetRange(index, htmlLines.Count - index);
}

HtmlElement html=createTree(htmlLines);
string selectorString = "a .icon";
Selector selector=Selector.createSelector(selectorString);
List<HtmlElement> a = new List<HtmlElement>();
var aa = html.searchInTree(1,selector,a);
Console.WriteLine(aa.Count);


Console.ReadKey();

HtmlElement createTree(List<string> htmlString)
{
    HtmlHelper htmlHelper = HtmlHelper.Instance;
    HtmlElement root = new HtmlElement();
    HtmlElement current = root;

    foreach (var line in htmlLines)
    {
        if (line[0] == '/'&& line[1] != '*')
            current = current.Parent;
        else
        {
            string tagName = line.Split(' ')[0];
            if (line[line.Length - 1] != '/' && htmlHelper.HtmlTags.Contains(tagName))
            {
                HtmlElement newEl = createNewElement(line, tagName);
                current.Children.Add(newEl);
                newEl.Parent = current;
                current = newEl;
            }

            else
     if (htmlHelper.HtmlVoidTags.Contains(tagName))
            {
                HtmlElement newEl = createNewElement(line, tagName);
                current.Children.Add(newEl);
                newEl.Parent = current;
            }

            else
                current.InnerHtml += line;
        }
    }
    return root.Children[0];
}

HtmlElement createNewElement(string line, string tagName)
{
    HtmlElement newEl = new HtmlElement();
    addArrtibutes(line, newEl);
    newEl.Name = tagName;
    return newEl;
}

void addArrtibutes(string htmlString, HtmlElement element)
{
    var arrtibutes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlString).ToList();
    List<string> arrtibutesStrings = new List<string>();
    foreach (Match match in arrtibutes)
    {
        arrtibutesStrings.Add(match.Value);
    }
    addClasses(arrtibutesStrings, element);
    addId(arrtibutesStrings, element);
    element.Arrtibutes = arrtibutesStrings;
}

void addClasses(List<string> arrtibutes, HtmlElement element)
{
    var classMatch = arrtibutes.FirstOrDefault(m => m.StartsWith("class", StringComparison.OrdinalIgnoreCase));
    if (classMatch != null)
    {
        var classes = new Regex(" ").Split(new Regex("\"(.*?)\"")
            .Match(classMatch).Groups[1].Value).ToList();
        if (classes[0] != "")
            element.Classes = classes;
        arrtibutes.Remove(classMatch);
    }
}

void addId(List<string> arrtibutes, HtmlElement element)
{
    var idMatch = arrtibutes.FirstOrDefault(m => m.StartsWith("id", StringComparison.OrdinalIgnoreCase));
    if (idMatch != null)
    {
        var id = new Regex("\"(.*?)\"")
            .Match(idMatch).Groups[1].Value;
        if (id != "")
            element.Id = id;
        arrtibutes.Remove(idMatch);
    }
}

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    return await response.Content.ReadAsStringAsync();
}