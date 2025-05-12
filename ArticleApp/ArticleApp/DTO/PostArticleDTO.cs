using System.ComponentModel.DataAnnotations;

namespace ArticleApp.DTO
{
    public class PostArticleDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Author { get; set; }
    }
}
