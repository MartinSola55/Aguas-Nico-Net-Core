using System.Text;
using AguasNico.Models;
using Newtonsoft.Json;

namespace AguasNico.Data.Services
{
    public class WhatsAppService(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<bool> ConfirmOrder(string phoneNumber, List<Dictionary<int, string>> products, List<Dictionary<int, string>> abonoProducts, decimal debt)
        {
            var message = new WppMessages().ConfirmOrder(products, abonoProducts, debt);
            return await SendMessage("notificacion_bajada", phoneNumber, message);
        }

        private async Task<bool> SendMessage(string templateName, string to, string message)
        {
            var token = _configuration["WppToken"];
            var phoneId = _configuration["WppPhoneId"];

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://graph.facebook.com/v20.0/{phoneId}/messages");

            request.Headers.Add("Authorization", $"Bearer {token}");

            request.Content = new StringContent(JsonConvert.SerializeObject(new
            {
                messaging_product = "whatsapp",
                to = $"54{to}",
                type = "template",
                template = new
                {
                    name = templateName,
                    language = new
                    {
                        code = "es_AR"
                    },
                    components = new[]
                    {
                        new
                        {
                            type = "body",
                            parameters = new[]
                            {
                                new
                                {
                                    type = "text",
                                    text = message
                                }
                            }
                        }
                    }
                }
            }), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
    }
}