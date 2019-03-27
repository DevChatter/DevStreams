using GraphQL.Language.AST;
using GraphQL.Types;
using NodaTime;
using NodaTime.Text;
using System;
using System.Globalization;

// With thanks to: https://github.com/shoooe/graphql-nodatime/tree/master/GraphQL.NodaTime/Types
// Removed ParserComposer and string values as we do not like the implementation.

namespace DevChatter.DevStreams.Infra.GraphQL.Types
{
    public class InstantGraphType : ScalarGraphType
    {
        public InstantGraphType()
        {
            Name = "Instant";
            Description = "Represents an instant on the global timeline.";
        }

        public override object Serialize(object value)
        {
            if (value is string)
            {
                return value;
            }

            if (value is DateTime dateTime)
            {
                return dateTime.ToString("o", CultureInfo.InvariantCulture);
            }

            if (value is Instant instant)
            {
                return InstantPattern.ExtendedIso
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Format(instant);
            }

            return null;
        }

        private static object FromDateTimeUtc(DateTime dateTime)
        {
            try
            {
                return Instant.FromDateTimeUtc(dateTime);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override object ParseValue(object value)
        {
            if (value is DateTime dateTimeValue)
            {
                return FromDateTimeUtc(dateTimeValue);
            }

            return null;
        }

        public override object ParseLiteral(IValue value)
        {
            if (value is DateTimeValue dateTimeValue)
            {
                return FromDateTimeUtc(dateTimeValue.Value);
            }
                       
            return null;
        }
    }
}
