using GabTrans.Application.Abstractions.Notification;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Services
{
    public class DisputeService(IEmailNotificationService emailService, IUserRepository userRepository, IAccountRepository accountRepository, IDisputeRepository disputeRepository, ISettlementRepository settlementRepository) : IDisputeService
    {
        private readonly IEmailNotificationService _emailService = emailService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly IDisputeRepository _disputeRepository = disputeRepository;
        private readonly ISettlementRepository _settlementRepository = settlementRepository;

        public async Task<ApiResponse> CreateAsync(CreateDisputeRequest request, long userId, string browser, string ipAddress)
        {
            var details = await _disputeRepository.DetailsAsync(request.Reference);
            if (details is not null)
            {
                details.Status = DisputeStatuses.Open;
                //Send email to admin
                return new ApiResponse
                {
                    Success = true,
                    Message = "Your dispute has been registered successfully"
                };
            }

            var settlement = await _settlementRepository.DetailsAsync(request.Reference, DebitCreditIndicators.Debit);
            if (settlement is null)
            {
                return new ApiResponse
                {
                    Message = "Reference does not exist"
                };
            }

            var account = await _accountRepository.DetailsAsync(settlement.AccountId);
            if (account is null)
            {
                return new ApiResponse
                {
                    Message = "No details found for the account"
                };
            }

            var user = await _userRepository.GetDetailsByUserIdAsync(account.UserId);
            if (user is null)
            {
                return new ApiResponse
                {
                    Message = "No details found for the user"
                };
            }

            var dispute = new Dispute
            {
                Comment = request.Comment,
                Reference = request.Reference,
                AccountId = settlement.AccountId,
                Type = settlement.Type
            };
            bool insert = await _disputeRepository.InsertAsync(dispute);
            if (!insert)
            {
                return new ApiResponse
                {
                    Message = "Unable to create dispute"
                };
            }

            //Send email to customer
            await _emailService.NewDisputeAsync(account.Name, request.Reference, settlement.Type, settlement.Currency, settlement.Amount, request.Comment, settlement.CreatedAt);

            return new ApiResponse { Message = "Dispute created successfully", Success = true };
        }

        public async Task<ApiResponse> UpdateAsync(UpdateDisputeRequest request, long id, long userId, string browser, string ipAddress)
        {
            var dispute = await _disputeRepository.DetailsAsync(id);
            if (dispute is null)
            {
                return new ApiResponse
                {
                    Message = "No details found for the dispute"
                };
            }

            var settlement = await _settlementRepository.DetailsAsync(dispute.Reference, DebitCreditIndicators.Debit);
            if (settlement is null)
            {
                return new ApiResponse
                {
                    Message = "Reference does not exist"
                };
            }

            var account = await _accountRepository.DetailsAsync(dispute.AccountId);
            if (account is null)
            {
                return new ApiResponse
                {
                    Message = "No details found for the account"
                };
            }

            var user = await _userRepository.GetDetailsByUserIdAsync(account.UserId);
            if (user is null)
            {
                return new ApiResponse
                {
                    Message = "No details found for the user"
                };
            }

            dispute.Status = request.Status;
            bool update = await _disputeRepository.UpdateAsync(dispute);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update dispute"
                };
            }

            //Send email to customer
            await _emailService.UpdateDisputeAsync(user.EmailAddress, dispute.Reference, settlement.Type, settlement.Currency, settlement.Amount, dispute.Comment, settlement.CreatedAt);

            return new ApiResponse { Message = "Updated the dispute successfully", Success = true };
        }
    }
}
