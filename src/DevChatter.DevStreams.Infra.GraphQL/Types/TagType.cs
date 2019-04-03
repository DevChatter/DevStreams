using DevChatter.DevStreams.Core.Model;
using GraphQL.Types;

namespace DevChatter.DevStreams.Infra.GraphQL.Types
{
    public class TagType : ObjectGraphType<Tag>
    {
        public TagType()
        {
            Field(f => f.Id).Description("Unique Identifier for Tag");
            Field(f => f.Name).Description("The tag");
            Field(f => f.Description).Description("Description of the tag");
        }
    }
}
