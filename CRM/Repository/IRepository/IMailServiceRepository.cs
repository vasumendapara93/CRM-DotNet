using System.Runtime.CompilerServices;

namespace CRM.Repository.IRepository
{
    public interface IMailServiceRepository
    {
        public void sendMail(string email,string subject, string mailBody);
    }
}
