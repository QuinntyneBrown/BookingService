using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingService.Core
{
    public interface ICacheProvider
    {
        ICache GetCache();
    }
}
