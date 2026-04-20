using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Domain.Enums;
using System.Text;

namespace GabTrans.Application.Services
{
    public class SequenceService : ISequenceService
    {
        private readonly ILogService _logService;
        private readonly ISignUpRepository _signUpRepository;
        private readonly ISequenceRepository _sequenceRepository;
        public SequenceService(ILogService logService, ISignUpRepository signUpRepository, ISequenceRepository sequenceRepository)
        {
            _logService = logService;
            _signUpRepository = signUpRepository;
            _sequenceRepository = sequenceRepository;
        }

        static readonly object transactionLock = new object();

        public string GenerateRandomNumber()
        {
            Random random = new Random();
            string referenceNo = random.Next(1, 100000).ToString().PadLeft(6, '0');
            return referenceNo;
        }

        public string GenerateRandomNumber(int length = 6)
        {
            Random random = new();

            StringBuilder result = new(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(random.Next(0, 10)); // Append a random digit (0-9)
            }

            return result.ToString();
        }

        public string GenerateRandomString()
        {
            return Guid.NewGuid().ToString();
        }

        public string Settlement(long transactionType)
        {
            lock (transactionLock)
            {
                Thread.Sleep(10);
                Random generator = new Random();
                String randomDigit = generator.Next(0, 100000).ToString("D6");
                var referenceNumber = string.Concat(DateTime.Now.ToString("yyMMddHHmmssff"), transactionType, randomDigit.ToString());
                return referenceNumber;
            }
        }


        public string GenerateAccountNumber()
        {
            lock (transactionLock)
            {
                Thread.Sleep(10);
                Random generator = new Random();
                String randomDigit = generator.Next(0, 10000000).ToString("D6");
                var referenceNumber = string.Concat("999", randomDigit.ToString());
                return referenceNumber;
            }
        }
    }
}
