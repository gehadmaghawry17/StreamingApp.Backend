using StreamingApp.Domain.Common;
using StreamingApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingApp.Domain.Entities.Userss
{
    public class OtpCode : BaseEntity
    {
        public Guid UserId { get; set; }

        public string Code { get; set; } = string.Empty;

        public OtpPurpose Purpose { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; } = false;

        public DateTime? UsedAt { get; set; }

        // Navigation
        public User User { get; set; } = null!;
    }
}
