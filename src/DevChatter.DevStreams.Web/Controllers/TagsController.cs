using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Core.Tagging;
using DevChatter.DevStreams.Web.Data.ViewModel.Tags;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : Controller
    {
        private readonly ITagSearchService _tagSearchService;

        public TagsController(ITagSearchService tagSearchService)
        {
            _tagSearchService = tagSearchService;
        }

        [HttpGet]
        public async Task<IList<TagViewModel>> Get(string filter)
        {
            List<TagWithCount> models = await _tagSearchService.FindTagsWithCount(filter);

            var viewModels = models
                .Select(x => x.ToViewModel())
                .ToList();

            return viewModels;
        }
    }
}