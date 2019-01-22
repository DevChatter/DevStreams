using DevChatter.DevStreams.Core.Model;

namespace DevChatter.DevStreams.Web.Data.ViewModel.Tags
{
    public static class TagMappingExtensions
    {
        public static TagViewModel ToViewModel(this Tag src)
        {
            return new TagViewModel
            {
                Id = src.Id,
                Name = src.Name,
                Description = src.Description
            };
        }
    }
}