using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Globalization;

// Custom DateTime converter that handles various input formats
public class CustomDateTimeConverter : IsoDateTimeConverter
{
    public CustomDateTimeConverter()
    {
        DateTimeFormat = "dd-MMM-yyyy";
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.Value != null && reader.Value is string stringValue)
        {
            if (DateTime.TryParseExact(stringValue, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            else if (DateTime.TryParseExact(stringValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }
            else if (DateTime.TryParseExact(stringValue, "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }
            else if (DateTime.TryParseExact(stringValue, "ddd-MMMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }
            else if (DateTime.TryParseExact(stringValue, "yyyy-dd-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }
        }

        return base.ReadJson(reader, objectType, existingValue, serializer);
    }
}
