using StreamingApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingApp.Domain.Entities.Userss
{
    public class UserPin : BaseEntity
    {
        public Guid UserId { get; set; }
        public string PinHash {  get; set; }=string.Empty;
        public int FailedAttempts { get; set; } = 0;

        public DateTime? LockedUntil { get; set; }

        // Navigation
        public User User { get; set; } = null!;
    }
}
