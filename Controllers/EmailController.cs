using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace dbreezeDbApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class EmailController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World! This is Email Sender Api From Peter's Soft Network");
    }
    [HttpPost]
    public IActionResult Send(MailRequest request)
    {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(request.FromEmail));
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = request.Subject;
           // email.Body = new TextPart(TextFormat.Plain) { Text = request.Body };
            //To send Html Message
            //send Body string as Html element and tags
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body};

            // send email //this email is my new generated app password.
            using var smtp = new SmtpClient();//eekrpdngefpeexhl
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("peters.soft.network@gmail.com", "eekrpdngefpeexhl");//"lsnphzhluoecthqw");//lsnphzhluoecthqw
            smtp.Send(email);
            smtp.Disconnect(true);
            return Ok("Email sent to "+request.ToEmail);
    }
}
