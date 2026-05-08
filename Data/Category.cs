using System.ComponentModel.DataAnnotations;

namespace SomaShare.Data
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; } = string.Empty;

        public ICollection<TextbookCategory> TextbookCategories { get; set; } = new List<TextbookCategory>();
    }
}
