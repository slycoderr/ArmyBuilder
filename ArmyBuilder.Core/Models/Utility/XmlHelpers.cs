using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ArmyBuilder.Core.Models.Utility
{
    class XmlHelpers
    {
        public static Task SerializeXml<T>(object obj, string path, string oldName, string newName)
        {
            return Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(oldName) && oldName != newName && File.Exists(Path.Combine(path, oldName + ".xml")))
                {
                    File.Delete(Path.Combine(path, oldName + ".xml"));
                }

                using (var stream = new FileStream(Path.Combine(path, newName) + ".xml", FileMode.Create))
                {
                    using (var writer = XmlWriter.Create(stream, new XmlWriterSettings() { Indent = true }))
                    {

                        var dsArmy = new XmlSerializer(typeof(T));
                        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                        ns.Add("", "");
                        dsArmy.Serialize(writer, obj, ns);

                    }
                }
            });
        }

        public static Task<List<string>> DiscoverXmlFiles(string path)
        {
            return Task.Run(() =>
            {
                return Directory.EnumerateFiles(path).Where(f => Path.GetExtension(f).ToLower() == ".xml").ToList();
            });
        }

        public static Task<T> DeserializeXml<T>(string path)
        {
            return Task.Run(() =>
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    using (var reader = XmlReader.Create(stream))
                    {
                        var dsArmy = new XmlSerializer(typeof(T));

                        return (T)dsArmy.Deserialize(reader);
                    }
                }
            });

        }
    }
}
