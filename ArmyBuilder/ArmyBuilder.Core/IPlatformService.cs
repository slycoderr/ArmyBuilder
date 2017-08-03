using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyBuilder.Core
{
    public interface IPlatformService
    {
        Task SerializeXml<T>(object obj, string path, string oldName, string newName);
        void CreateDirectory(string path);
        Task<List<string>> DiscoverXmlFiles(string path);
        Task<T> DeserializeXml<T>(string path);
    }
}
