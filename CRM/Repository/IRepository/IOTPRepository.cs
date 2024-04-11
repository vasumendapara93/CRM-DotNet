namespace CRM.Repository.IRepository
{
    public interface IOTPRepository
    {
        public string GenerateOTP(string userId);

        public bool VerifyOTP(string userId, string otp);
    }
}
