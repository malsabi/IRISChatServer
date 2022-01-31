using Newtonsoft.Json;

namespace IRISChatServer.DataAccessLayer
{
    public class Serialization
    {
        public static string Serialize<T>(T Input)
        {
            try
            {
                return JsonConvert.SerializeObject(Input);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static T Deserialize<T>(string Input)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(Input);
            }
            catch
            {
                return default;
            }
        }
    }
}