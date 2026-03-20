using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace ClunyApp.Services
{
    public class MailjetHttpEmailSender : IEmailSender
    {
        private readonly HttpClient httpClient;
        private readonly string fromEmail;
        private readonly string fromName;

        public MailjetHttpEmailSender(HttpClient httpClient, IConfiguration config)
        {
            this.httpClient = httpClient;
            var apiKey = config["Mailjet:ApiKey"] ?? throw new InvalidOperationException("Mailjet:ApiKey missing");
            var apiSecret = config["Mailjet:ApiSecret"] ?? throw new InvalidOperationException("Mailjet:ApiSecret missing");
            fromEmail = config["Mailjet:SenderEmail"] ?? throw new InvalidOperationException("Mailjet:SenderEmail missing");
            fromName = config["Mailjet:SenderName"] ?? "No Reply";

            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiKey}:{apiSecret}"));
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
            => SendAsync(email, subject, htmlMessage);

        public async Task SendAsync(string to, string subject, string htmlBody)
        {
            var payload = new
            {
                Messages = new[]
                {
                    new
                    {
                        From = new { Email = fromEmail, Name = fromName },
                        To = new[] { new { Email = to, Name = to } },
                        Subject = subject,
                        HTMLPart = htmlBody
                    }
                }
            };

            var json = JsonSerializer.Serialize(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var resp = await httpClient.PostAsync("https://api.mailjet.com/v3.1/send", content).ConfigureAwait(false);
            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new InvalidOperationException($"Mailjet send failed ({(int)resp.StatusCode}): {body}");
            }
        }
    }
}