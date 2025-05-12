using ArticleApp.Data;
using ArticleApp.DTO;
using ArticleApp.Extentions;
using ArticleApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ArticleApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IDistributedCache cache;

        public ArticleController(AppDbContext context, IDistributedCache cache)
        {
            this.context = context;
            this.cache = cache;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostArticleDTO articleDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var article = new Article(articleDto);



            //// add article to database 
            article.PublishDate= DateTime.UtcNow;
            await context.Articles.AddAsync(article);
            if (await context.SaveChangesAsync() == 0)
            {
                return BadRequest("Create Proccess has problem!");
            }

            string cacheKey = $"article_{article.Id}";


            await AddArticleToCache(cacheKey,article);

            return Ok(article);
        }

        [HttpGet("{id?}")]
        public async Task<IActionResult> GetById(int? id)
        {
            if (id is null) return NotFound();

            string cacheKey = $"article_{id}";

            var article = await DistributedCacheExtentions.GetRecordAsync<Article>(cache, cacheKey);

            if (article == null)
            {
                article = await context.Articles.Where(x => x.Id == id).FirstOrDefaultAsync();

                if(article == null) return NotFound();

                await AddArticleToCache(cacheKey, article);

            }

            return Ok(article);
        }


        private async Task AddArticleToCache(string cacheKey, Article article,TimeSpan? time=null)
        {
            
            await DistributedCacheExtentions.SetRecordAsync(cache,cacheKey, article,time);
        }
    }
}
