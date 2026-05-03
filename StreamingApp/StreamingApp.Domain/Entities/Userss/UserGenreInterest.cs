using StreamingApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingApp.Domain.Entities.Userss
{
    public class UserGenreInterest : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid GenreId { get; set; }

        // Navigation Properties
        public User User { get; set; } = null!;

        public Genre Genre { get; set; } = null!;
    }
}
