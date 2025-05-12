using ArticleApp.DTO;

namespace ArticleApp.Models
{
    public class Article
    {
        public Article()
        {

        }

        public Article(string title,string Content,string Author)
        {
            this.Title = title;
            this.Content = Content;
            this.Author = Author;
        }

        public Article(PostArticleDTO dto)
        {
            this.Title = dto.Title;
            this.Content = dto.Content;
            this.Author = dto.Author;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
    }

    
}
