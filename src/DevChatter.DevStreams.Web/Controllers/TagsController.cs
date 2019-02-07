using DevChatter.DevStreams.Web.Data;
using DevChatter.DevStreams.Web.Data.ViewModel.Tags;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;

namespace DevChatter.DevStreams.Web.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ITagSearchService _tagSearchService;

        public TagsController(ApplicationDbContext dbContext, ITagSearchService tagSearchService)
        {
            _dbContext = dbContext;
            _tagSearchService = tagSearchService;
        }

        [HttpGet]
        public async Task<IList<TagViewModel>> Get(string filter)
        {
            List<Tag> models = await _tagSearchService.Find(filter);

            //List<Tag> models = await _dbContext.Tags
            //    .Where(t => t.Name.Contains(filter)) // TODO: Much better filtering
            //    .ToListAsync();

            var viewModels = models
                .Select(x => x.ToViewModel())
                .ToList();

            return viewModels;
        }
    }
}