using Newtonsoft.Json.Linq;

namespace ExamOnline.DTOs
{
    public class ResponseModel
    {
        public static JObject Response<T>(int code, string message, T data)
        {
            var response = new JObject();
            response.Add("code", code);
            response.Add("message", message);
            response.Add("data", data is null?null:JObject.FromObject(data));
            return response;
        }
    }
}
