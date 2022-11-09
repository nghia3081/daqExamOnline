using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace ExamOnline.DTOs
{
    public class ResponseModel
    {
        public static JObject Response<T>(int code, string message, T data)
        {
            JObject dataSeri = new JObject();
            var response = new JObject();
            response.Add("code", code);
            response.Add("message", message);
            response.Add("data",data.ToString());
            return response;
        }
    }
}
