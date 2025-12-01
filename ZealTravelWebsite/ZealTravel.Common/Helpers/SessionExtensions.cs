using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

public static class SessionExtensions
{
    // Save a complex object in the session
    public static void SetComplexObject<T>(this ISession session, string key, T value)
    {
        var serializedValue = JsonConvert.SerializeObject(value);
        session.Set(key, Encoding.UTF8.GetBytes(serializedValue));
    }

    // Retrieve a complex object from the session
    public static T GetComplexObject<T>(this ISession session, string key)
    {
        if (session.TryGetValue(key, out var data))
        {
            var serializedValue = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<T>(serializedValue);
        }
        return default;
    }
}
