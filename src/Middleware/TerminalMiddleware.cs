using pandapache.src.RequestHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pandapache.src.Middleware
{
    public class TerminalMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("Last middleware");
        }
    }

}
