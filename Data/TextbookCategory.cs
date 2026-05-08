namespace SomaShare.Data
{
    public class TextbookCategory
    {
        public int TextbookId { get; set; }
        public Textbook Textbook { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
