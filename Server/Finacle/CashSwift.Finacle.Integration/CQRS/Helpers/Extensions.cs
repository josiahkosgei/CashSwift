
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CashSwift.Finacle.Integration.CQRS.Helpers
{
    public static class Extensions
    {
        public static string AsJson(this object o) => JsonSerializer.Serialize(o);
        public static string ToJson(this object o) => JsonConvert.SerializeObject(o, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        public static string AppendAsURL(this string baseURL, params string[] segments)
        {
            return string.Join("/", new[] { baseURL.TrimEnd('/') }
                .Concat(segments?.Select(s => s?.Trim('/'))));
        }
    }
}
