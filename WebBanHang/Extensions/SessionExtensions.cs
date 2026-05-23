using System.Text.Json;

namespace WebBanHang.Extensions
{
    public static class SessionExtensions
    {
        // Lưu đối tượng thành chuỗi JSON
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Đọc chuỗi JSON và chuyển lại thành đối tượng
        public static T? GetJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);
            return sessionData == null ? default(T) : JsonSerializer.Deserialize<T>(sessionData);
        }
    }
}