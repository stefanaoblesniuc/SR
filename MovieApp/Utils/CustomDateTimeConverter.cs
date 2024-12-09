using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.TypeConversion;

namespace MovieApp.Utils;

public class CustomDateTimeConverter : DateTimeConverter
{
    private readonly string[] _dateFormats =
    {
        "MMMM d, yyyy", // Format with a comma
        "MMMM d. yyyy"  // Format with a dot
    };

    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        foreach (var format in _dateFormats)
        {
            if (DateTime.TryParseExact(text, format, null, System.Globalization.DateTimeStyles.None, out var date))
            {
                return date;
            }
        }
        throw new FormatException($"Could not parse date: '{text}' in the 'Premiere' column.");
    }
}
