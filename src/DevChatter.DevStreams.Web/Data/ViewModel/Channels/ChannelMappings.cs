using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DevChatter.DevStreams.Core.Model;
using TimeZoneNames;

namespace DevChatter.DevStreams.Web.Data.ViewModel.Channels
{
    public static class ChannelMappings
    {
        public static ChannelViewModel ToChannelViewModel(this Channel src)
        {
            return new ChannelViewModel
            {
                Id = src.Id,
                Name = src.Name,
                TimeZoneName = TZNames.GetNamesForTimeZone(src.TimeZoneId, CultureInfo.CurrentUICulture.Name).Generic,
                Uri = src.Uri,
                ScheduledStreamsCount = src.ScheduledStreams.Count
            };
        }

        public static void ApplyEditChanges(this Channel model,
            ChannelEditModel editModel)
        {
            model.Name = editModel.Name;
            model.Uri = editModel.Uri;
            model.CountryCode = editModel.CountryCode;
            model.TimeZoneId = editModel.TimeZoneId;
            ApplyTagChanges(model, editModel.TagIdString);
        }

        private static void ApplyTagChanges(Channel model, string tagIdsString)
        {
            var desiredTagIds = tagIdsString.Split(',').Select(s => Int32.Parse(s));
            var existingTagIds = model.Tags.Select(t=> t.TagId);
            var tagsToAdd = desiredTagIds.Except(existingTagIds);
            var tagsToRemove = existingTagIds.Except(desiredTagIds);

            model.Tags.AddRange(tagsToAdd.Select(id => 
                    new ChannelTag{
                        TagId = id,
                        ChannelId = model.Id
                    }));

            model.Tags.RemoveAll(t => tagsToRemove.Contains(t.TagId));
        }

        public static ChannelEditModel ToChannelEditModel(this Channel src)
        {
            return new ChannelEditModel
            {
                Id = src.Id,
                Name = src.Name,
                Uri = src.Uri,
                CountryCode = src.CountryCode,
                TimeZoneId = src.TimeZoneId,
            };
        }
    }
}