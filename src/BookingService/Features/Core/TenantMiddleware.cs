using Microsoft.Owin;
using BookingService.Data;
using System.Threading.Tasks;

namespace BookingService.Features.Core
{
    public class TenantMiddleware : OwinMiddleware
    {
        public TenantMiddleware(OwinMiddleware next)
            : base(next) { }

        public override async Task Invoke(IOwinContext context)
        {
            var quoteServiceContext = (BookingServiceContext)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(BookingServiceContext));
            
            var values = context.Request.Headers.GetValues("Tenant");
            if (values != null) {                
                context.Environment.Add("Tenant", ((string[])(values))[0]);                
            }
           
            await Next.Invoke(context);
        }
    }
}
