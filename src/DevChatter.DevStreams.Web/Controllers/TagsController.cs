using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChatter.DevStreams.Web.Data;
using DevChatter.DevStreams.Web.Data.ViewModel.Tags;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevChatter.DevStreams.Web.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public TagsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IList<TagViewModel>> Get(string filter)
        {
            var models = await _dbContext.Tags.Where(t => t.Name.Contains(filter)).ToListAsync();

            List<TagViewModel> viewModels = models.Select(x => x.ToViewModel()).ToList();
            return viewModels;
        }
    }
}