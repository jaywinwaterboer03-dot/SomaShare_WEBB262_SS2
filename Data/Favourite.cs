using System;
using System.ComponentModel.DataAnnotations;

namespace SomaShare.Data
{
    public class Favourite
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        public int TextbookId { get; set; }
        public virtual Textbook Textbook { get; set; } = null!;

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
