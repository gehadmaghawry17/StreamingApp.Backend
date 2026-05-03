using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingApp.Domain.Enums
{
    public enum SubscriptionStatus
    {
        Active = 1,
        Expired = 2,
        Cancelled = 3,
        PendingPayment = 4
    }
}
