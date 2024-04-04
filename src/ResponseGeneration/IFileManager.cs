using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pandapache.src.ResponseGeneration
{
    public interface IFileManager
    {
        Task<string> ReadAllTextAsync(string path, Encoding encoding);

        public Task<byte[]> ReadAllBytesAsync(string path);


        bool Exists(string path);
    }
}
