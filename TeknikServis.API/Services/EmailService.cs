namespace TeknikServis.API.Services
{
    using MailKit.Net.Smtp;
    using MimeKit;

    public class EmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("erencemert24@gmail.com")); // Gmail adresiniz
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };


            try
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("erencemert24@gmail.com", "jpdhnmhcaafmefpb"); // 16 haneli app password
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }catch(Exception ex)
            {
                Console.WriteLine("Hata:" + ex.Message);
            }

     
        }
    }
}
