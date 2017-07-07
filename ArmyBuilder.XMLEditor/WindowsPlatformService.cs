using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ArmyBuilder.Core;
using ArmyBuilder.Core.Models;

namespace ArmyBuilder.Windows
{
    public class WindowsPlatformService : IPlatformService
    {
        public Task SerializeXml<T>(object obj, string path)
        {
            return Task.Run(() =>
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    using (var writer = XmlWriter.Create(stream))
                    {
                        var dsArmy = new XmlSerializer(typeof(T));
                        dsArmy.Serialize(writer, this);
                    }
                }
            });
        }

        public void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public Task<List<string>> DiscoverXmlFiles(string path)
        {
            return Task.Run(() =>
            {
                return Directory.EnumerateFiles(path).Where(f => Path.GetExtension(f).ToLower() == ".xml").ToList();
            });
        }

        public Task<T> DeserializeXml<T>(string path)
        {
            return Task.Run(() =>
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    using (var reader = XmlReader.Create(stream))
                    {
                        var dsArmy = new XmlSerializer(typeof(T));

                        return (T) dsArmy.Deserialize(reader);
                    }
                }
            });

        }
    }
}
