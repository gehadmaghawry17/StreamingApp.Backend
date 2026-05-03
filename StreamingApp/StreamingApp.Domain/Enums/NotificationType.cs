using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingApp.Domain.Enums
{
    public enum NotificationType
    {
        NewContent = 1,
        SubscriptionExpiring = 2,
        SubscriptionRenewed = 3,
        PaymentFailed = 4,
        NewEpisodeAvailable = 5,
        General = 6
    }
}
