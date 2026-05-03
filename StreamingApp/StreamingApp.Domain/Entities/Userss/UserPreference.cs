using StreamingApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingApp.Domain.Entities.Userss
{
    public class UserPreference : BaseEntity
    {
        // Stores user-specific settings and preferences.
        // One-to-one relationship with User.
        public Guid UserId { get; set; }

        public string PreferredLanguage { get; set; } = "en";

        public string PreferredSubtitleLanguage { get; set; } = "en";

        public bool AutoPlayNext { get; set; } = true;

        public bool EnableNotifications { get; set; } = true;

        public string VideoQuality { get; set; } = "Auto"; // Auto, 720p, 1080p, 4K

        // Navigation
        public User User { get; set; } = null!;
    }
}
