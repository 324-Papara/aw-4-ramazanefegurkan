using System.Net;
using System.Net.Mail;

namespace Para.Bussiness.Notification;

public class NotificationService  : INotificationService
{
    public void SendEmail(string subject, string email, string content)
    {

        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
        smtpClient.EnableSsl = true;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential("odevtest802@gmail.com", "zylx cnzf qukd oysb");

        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("odevtest802@gmail.com", "Test");
        mail.To.Add(new MailAddress(email, "Kullanýcý Test"));
        mail.Subject = subject;
        mail.SubjectEncoding = System.Text.Encoding.UTF8;
        mail.Body = content;
        mail.BodyEncoding = System.Text.Encoding.UTF8;
        mail.IsBodyHtml = true;

        smtpClient.Send(mail);
    }
}