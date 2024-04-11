using CRM.Repository.IRepository;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Cryptography;

namespace CRM.Repository
{
    public class OTPRepository : IOTPRepository
    {
        private readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        IDistributedCache _distributedCache;

        public OTPRepository(IDistributedCache distributedCache)
        {
        _distributedCache = distributedCache;
        }


        public string GenerateOTP(string userId)
        {
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            var otp = BitConverter.ToUInt32(bytes, 0).ToString().Substring(0, 6).ToString();
            _distributedCache.SetString(userId, otp, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(2),
            });
            return otp;
        }

        public bool VerifyOTP(string userId, string otp)
        {
            var catchedOtp = _distributedCache.GetString(userId);
            if(catchedOtp == null)
            {
                return false;
            }

            return catchedOtp.Equals(otp);
        }
    }
}
