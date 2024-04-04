using pandapache.src;
using pandapache.src.ConnectionManagement;
using pandapache.src.RequestHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace pandapache.src
{
    [Obsolete]
    public static class ConnectionUtils
    {

       
        public async static Task<Request> ParseRequestAsync(ISocketWrapper client)
        {

            byte[] bufferRequest = new byte[1024];
            int bytesRead = client.Receive(bufferRequest);
            string requestString = Encoding.UTF8.GetString(bufferRequest, 0, bytesRead);

            Request request = new Request(requestString);

            Console.WriteLine("Request");
            //Console.WriteLine(requestString);
            return request;
        }

        public async static Task SendResponseAsync(ISocketWrapper client, HttpResponse response)
        {
            try
            {
                byte[] msg = Encoding.UTF8.GetBytes(response.ToString());
                Console.WriteLine("Reponse");
                Console.WriteLine(response.ToString());
                //Console.WriteLine(response.formatReponse());
                await client.SendAsync(msg, SocketFlags.None);
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Error sending response: " + ex.Message);
            }


        }
    }
}
