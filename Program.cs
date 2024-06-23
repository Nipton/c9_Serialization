using System.Text;
using System.Text.Json;
using System.Xml;

namespace c9_Serialization
{
    internal class Program
    {
        public static string? ConvertJsonToXml(string path)
        {
            if(!File.Exists(path))
            {
                Console.WriteLine("Файл не существует.");
                return null;
            }
            StringBuilder xmlResult = new StringBuilder();
            try
            {
                
                string jsonData = File.ReadAllText(path);
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = "   "
                };
                using(JsonDocument doc = JsonDocument.Parse(jsonData))
                {

                    using (XmlWriter xmlWriter = XmlWriter.Create(xmlResult, xmlWriterSettings))
                    {
                        xmlWriter.WriteStartDocument();
                        xmlWriter.WriteStartElement("root");
                        WriteJsonToXml(doc.RootElement, xmlWriter);
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndDocument();
                    }
                }           
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return xmlResult.ToString();
        }
        static void WriteJsonToXml(JsonElement jsonElement, XmlWriter xmlWriter)
        {
            if (jsonElement.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in jsonElement.EnumerateObject())
                {                
                    {
                        xmlWriter.WriteStartElement(property.Name);
                        WriteJsonToXml(property.Value, xmlWriter);
                        xmlWriter.WriteEndElement(); 
                    }
                }
            }
            else if (jsonElement.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement item in jsonElement.EnumerateArray())
                {                 
                    xmlWriter.WriteStartElement("item");
                    WriteJsonToXml(item, xmlWriter);
                    xmlWriter.WriteEndElement();
                    
                }
            }
            else
            {
                xmlWriter.WriteValue(jsonElement.ToString());               
            }
        }
        static void Main(string[] args)
        {
            string path = "sample.json";
            Console.WriteLine(ConvertJsonToXml(path));
        }
    }
}