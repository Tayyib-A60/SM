using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit;

namespace SkineroMotors.Controllers
{
    [ApiController]
    [Route("/api/email")]
    public class EmailController: Controller
    {
        public EmailController()
        {

        }
        [HttpPost("sendEmail")]
        public async Task<IActionResult> SendMail()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Adesokan Tayyib", "adesokantayyib@gmail.com"));
            message.To.Add(new MailboxAddress("LABULE", "liadi.omotola@gmail.com"));
            message.Subject = "Sending an email with Dotnet is super cool";
            message.Body = new TextPart("plain"){
                Text = "This is my very first Dotnet email message"
            };
            using (var client = new SmtpClient()) {
                client.Connect("smtp.gmail.com", 587);
                await client.AuthenticateAsync("adesokantayyib@gmail.com", "Adenike1913");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            return Ok(message);
        }
    }
}