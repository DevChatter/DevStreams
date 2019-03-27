using GraphQL.Language.AST;
using GraphQL.Types;
using NodaTime;
using NodaTime.Text;
using System;
using System.Globalization;

// With thanks to: https://github.com/shoooe/graphql-nodatime/tree/master/GraphQL.NodaTime/Types

namespace DevChatter.DevStreams.Infra.GraphQL.Types
{
    public class LocalTimeGraphType : ScalarGraphType
    {
        public LocalTimeGraphType()
        {
            Name = "LocalTime";
            Description = "Represents a time of day, with no reference to a particular calendar, time zone or date.";
        }

        public override object ParseLiteral(IValue value)
        {
            if (!(value is StringValue stringValue))
            {
                return null;
            }
            
            try
            {
                var ret = LocalTimePattern.ExtendedIso
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Parse(stringValue.Value).GetValueOrThrow();

                return ret;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override object ParseValue(object value)
        {
            if (!(value is string stringValue))
            {
                return null;
            }
            
            try
            {
                var ret = LocalTimePattern.ExtendedIso
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Parse(stringValue).GetValueOrThrow();

                return ret;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override object Serialize(object value)
        {
            if (value is string)
            {
                return value;
            }

            if (value is LocalTime localTime)
            {
                return LocalTimePattern.ExtendedIso
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Format(localTime);
            }

            return null;
        }
    }
}
