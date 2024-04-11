using CRM.Repository.IRepository;
using System.Net;
using System.Net.Mail;

namespace CRM.Repository
{
    public class MailServiceRepository : IMailServiceRepository
    {
        public void sendMail(string email, string subject, string mailBody)
        {
            string fromEmail = "mrkingmoradiya@gmail.com";
            MailMessage mailMessage = new MailMessage(fromEmail, email);
            mailMessage.From = new MailAddress("mrkingmoradiya@gmail.com", "Limpid Systems CRM");
            mailMessage.Subject = subject;
            mailMessage.Body = mailBody;
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(fromEmail, "xdgmrzfwhqhpwfek");
            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
