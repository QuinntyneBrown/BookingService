using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingService.Core
{
    public interface ICacheProvider
    {
        ICache GetCache();
    }
}
