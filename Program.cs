using GrapeCity.Documents.Word;

namespace MesciusHBIBug
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestWithEmptyCollection();
            TestWithHbiEmptyCollection();

        }

        private static void TestWithEmptyCollection()
        {
            GcWordDocument doc = new GcWordDocument();
            doc.Body.AddParagraph(@" {{ds.Title}}
                                        {{#ds.Items}}  
                                            Name: {{ds.Items.Name}}
                                            I'm should not be here if items is empty
                                        {{/ds.Items}}");
            var model = new Model();
            doc.DataTemplate.DataSources["ds"] = model;
            doc.DataTemplate.Process();
            var text = doc.Body.Paragraphs.First.GetRange().Text;
            Console.WriteLine(text); // --------> Content is rendered with one item in list <------------
        }

        private static void TestWithHbiEmptyCollection()
        {
            GcWordDocument doc = new GcWordDocument();
            doc.Body.AddParagraph(@" {{ds.Title}}
                                        {{#ds.Items}:hbi-empty()}  
                                            Name: {{ds.Items.Name}}
                                            I'm should not be here if items is empty
                                        {{/ds.Items}}");

            var model = new Model();
            model.Items.Add(new Item() {Name = "Item 1"});
            model.Items.Add(new Item() { Name = "Item 2" }); // comment this LINE  --> Item 1 is not rendered with only one item in LIST document contains no paragraph
            doc.DataTemplate.DataSources["ds"] = model;
            doc.DataTemplate.Process();
            var text = doc.Body.Paragraphs.First.GetRange().Text;
            Console.WriteLine(text);
        }

        private class Model
        {
            public string Title { get; set; } = "Sequence Test";
            public List<Item> Items = new List<Item>();
        }

        private class Item
        {
            public string Name
            {
                get;
                set;
            }
        }
    }
}
