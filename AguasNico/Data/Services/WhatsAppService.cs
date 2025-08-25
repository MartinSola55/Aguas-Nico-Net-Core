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
            var (message1, message2, message3) = new WppMessages().ConfirmOrder(products, abonoProducts, debt);
            return await SendMessage("notificacion_bajada3", phoneNumber, message1, message2, message3);
        }

        private async Task<bool> SendMessage(string templateName, string to, string message1, string message2, string message3)
        {
            var token = _configuration["WppToken"];
            var phoneId = _configuration["WppPhoneId"];

            message1 = string.IsNullOrWhiteSpace(message1) ? "-" : message1;
            message2 = string.IsNullOrWhiteSpace(message2) ? "-" : message2;
            message3 = string.IsNullOrWhiteSpace(message3) ? "-" : message3;

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://graph.facebook.com/v21.0/{phoneId}/messages");

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
                                    text = message1
                                },
                                new
                                {
                                    type = "text",
                                    text = message2
                                },
                                new
                                {
                                    type = "text",
                                    text = message3
                                }
                            }
                        }
                    }
                }
            }), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

           //return response.IsSuccessStatusCode;
            if (!response.IsSuccessStatusCode){
                var responseContent = await response.Content.ReadAsStringAsync();
                // Registrar o mostrar el error para diagnóstico.
                Console.WriteLine($"Error: {responseContent}");
                return false;
            }

            return true;
        }
    }
}