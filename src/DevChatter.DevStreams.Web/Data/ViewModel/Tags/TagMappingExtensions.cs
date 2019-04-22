using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Tagging;

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

        public static TagViewModel ToViewModel(this TagWithCount src)
        {
            return new TagViewModel
            {
                Id = src.Tag.Id,
                Name = src.Tag.Name,
                Description = src.Tag.Description,
                Count = src.Count
            };
        }
    }
}