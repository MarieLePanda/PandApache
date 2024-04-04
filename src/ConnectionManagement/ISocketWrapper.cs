using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace pandapache.src.ConnectionManagement
{

    public interface ISocketWrapper
    {
        int Receive(byte[] buffer);
        public void Dispose();

        //public Task<int> SendAsync(ArraySegment<byte> buffer, SocketFlags socketFlags);

        public Task<int> SendAsync(byte[] buffer, SocketFlags socketFlags);

    }
}
