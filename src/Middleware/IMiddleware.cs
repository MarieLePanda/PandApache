using pandapache.src.RequestHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pandapache.src.Middleware
{
    public interface IMiddleware
    {
        Task InvokeAsync(HttpContext context);
    }

}
