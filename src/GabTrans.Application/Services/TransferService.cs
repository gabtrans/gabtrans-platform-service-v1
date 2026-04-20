using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Notification;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Application.Interfaces.Services;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Enums;
using GabTrans.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Services
{
    public class TransferService(ILogService logService, IFeeService feeService, IEmailNotificationService emailService, IUserRepository userRepository, ISequenceService sequenceService, IInfinitusService infinitusService, IWalletRepository walletRepository, ITransferRepository transferRepository, IAccountRepository accountRepository, ICountryRepository countryRepository, IEncryptionService encryptionService, IGlobusBankService globusBankService, IRecipientRepository recipientRepository, ISettlementRepository settlementRepository, ITransactionPinRepository transactionPinRepository, ITransferProviderRepository transferProviderRepository) : ITransferService
    {
        private readonly ILogService _logService = logService;
        private readonly IFeeService _feeService = feeService;
        private readonly IEmailNotificationService _emailService = emailService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISequenceService _sequenceService = sequenceService;
        private readonly IInfinitusService _infinitusService = infinitusService;
        private readonly IWalletRepository _walletRepository = walletRepository;
        private readonly ITransferRepository _transferRepository = transferRepository;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly ICountryRepository _countryRepository = countryRepository;
        private readonly IEncryptionService _encryptionService = encryptionService;
        private readonly IGlobusBankService _globusBankService = globusBankService;
        private readonly IRecipientRepository _recipientRepository = recipientRepository;
        private readonly ISettlementRepository _settlementRepository = settlementRepository;
        private readonly ITransactionPinRepository _transactionPinRepository = transactionPinRepository;
        private readonly ITransferProviderRepository _transferProviderRepository = transferProviderRepository;

        public async Task<ApiResponse> CreateAsync(TransferRequest request, long userId)
        {
            var account = await _accountRepository.GetAccountAsync(userId);
            if (account == null)
            {
                return new ApiResponse
                {
                    Message = "Account details not found"
                };
            }

            if (!string.Equals(account.Status, AccountStatuses.Active, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Account is not active"
                };
            }

            var transactionPin = await _transactionPinRepository.GetAsync(account.UserId);
            if (transactionPin == null)
            {
                return new ApiResponse
                {
                    Message = "pin details not found"
                };
            }

            if (!_encryptionService.VerifyHash(request.TransactionPin, transactionPin.NewPin))
            {
                return new ApiResponse
                {
                    Message = "Your transaction pin is incorrect"
                };
            }

            var wallet = await _walletRepository.GetAsync(account.Id, request.Currency);
            if (wallet is null)
            {
                return new ApiResponse
                {
                    Message = "Invalid wallet selected"
                };
            }

            var fee = await _feeService.GetAsync(account.Id, TransactionTypes.Transfer, request.Currency, request.Amount);
            if (fee == 0)
            {
                return new ApiResponse
                {
                    Message = "No fee configured"
                };
            }

            if (wallet.Balance < Math.Round(request.Amount + fee, 2))
            {
                return new ApiResponse
                {
                    Message = "You have insufficient balance"
                };
            }

            var purpose = await _transferRepository.GetReasonAsync(request.Reason);
            if (purpose is null)
            {
                return new ApiResponse
                {
                    Message = "Please supply a valid payment reason"
                };
            }

            return await CreateAsync(request, account.Name, account.Id, wallet.Id, fee);
        }

        public async Task<ApiResponse> CreateAsync(TransferRequest request, string accountName, long accountId, long walletId, decimal fee)
        {
            string reference = _sequenceService.Settlement(2);

            var settlement = new SettlementModel
            {
                Currency = request.Currency,
                AccountId = accountId,
                Reference = reference,
                TransactionAmount = request.Amount,
                TransactionType = TransactionTypes.Transfer,
                WalletId = walletId,
                Note = $"{reference}|{request.Reason}",
                DebitCreditIndicator = DebitCreditIndicators.Debit
            };
            string debit = await _settlementRepository.ProcessAsync(settlement);
            if (string.IsNullOrEmpty(debit))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            if (string.Equals(debit, "duplicate", StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            if (!string.Equals(debit, TransactionStatuses.Successful, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = debit
                };
            }

            settlement = new SettlementModel
            {
                Currency = request.Currency,
                AccountId = accountId,
                Reference = reference,
                TransactionAmount = fee,
                TransactionType = TransactionTypes.Charges,
                WalletId = walletId,
                Note = $"{reference}|{request.Reason}",
                DebitCreditIndicator = DebitCreditIndicators.Debit
            };
            debit = await _settlementRepository.ProcessAsync(settlement);
            if (string.IsNullOrEmpty(debit))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            if (string.Equals(debit, "duplicate", StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            if (!string.Equals(debit, TransactionStatuses.Successful, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = debit
                };
            }

            var recipient = await _recipientRepository.GetAsync(accountId, request.AccountNumber, request.Iban, request.Currency, request.BankAccountType);
            if (recipient is not null) return await ExistingRecipientAsync(request, accountId, recipient.Id, reference, fee, walletId, accountName);

            return await NewRecipientAsync(request, accountId, reference, fee, walletId, accountName);
        }

        public async Task<ApiResponse> NewRecipientAsync(TransferRequest request, long accountId, string reference, decimal fee, long walletId, string accountName)
        {
            var recipient = new TransferRecipient
            {
                AccountId = accountId,
                City = request.AddressCity,
                Country = request.CountryCode,
                AccountName = request.AccountName,
                AccountNumber = request.AccountNumber,
                BankAccountType = request.BankAccountType,
                Currency = request.Currency,
                BankBranch = request.BankBranch,
                AccountRoutingType = request.AccountRoutingType,
                BankCity = request.BankCity,
                BankCountry = request.BankCountry,
                BankName = request.BankName,
                BankPostalCode = request.BankPostalCode,
                BankState = request.BankState,
                BankStreetAddress = request.BankStreetAddress,
                CreatedAt = DateTime.Now,
                DateOfBirth = request.DateOfBirth,
                Email = request.Email,
                IntermediaryBankCountry = request.IntermediaryBankCountry,
                IntermediaryBankName = request.IntermediaryBankName,
                IntermediaryCity = request.IntermediaryCity,
                IntermediaryPostalCode = request.IntermediaryPostalCode,
                IntermediaryRoutingCode = request.IntermediaryRoutingCode,
                IntermediaryState = request.IntermediaryState,
                IntermediaryStreet1 = request.IntermediaryStreet1,
                IntermediaryStreet2 = request.IntermediaryStreet2,
                InternationalBankName = request.InternationalBankName,
                Name = request.Name,
                PaymentMethod = request.PaymentMethod,
                PhoneNumber = request.PhoneNumber,
                PostCode = request.AddressPostCode,
                RoutingNumber = request.RoutingNumber,
                State = request.AddressState,
                SwiftCode = request.SwiftCode,
                StreetAddress = request.AddressStreetAddress,
                Type = request.AccountType
            };
            long receipientId = await _recipientRepository.CreateAsync(recipient);
            if (receipientId == 0)
            {
                return new ApiResponse
                {
                    Message = "Unable to register receipient"
                };
            }

            var transfer = new Transfer
            {
                AccountId = accountId,
                Amount = request.Amount,
                AmountPaid = request.Amount,
                CreatedAt = DateTime.Now,
                Currency = request.Currency,
                Fee = fee,
                Gateway = PaymentGateways.Infinitus,
                PaymentMethod = request.PaymentMethod,
                Status = TransactionStatuses.Submitted,
                TransferRecipientId = receipientId,
                ProcessingStatus = TransactionStatuses.Pending,
                Reason = request.Reason,
                Reference = reference,
                SourceCurrency = request.Currency
            };
            bool insert = await _transferRepository.InsertAsync(transfer);
            if (!insert)
            {
                await ReverseAsync(request, reference, accountId, walletId, fee);

                transfer.Status = TransactionStatuses.Failed;
                await _transferRepository.UpdateAsync(transfer);
                return new ApiResponse
                {
                    Message = "Unable to initiate transfer"
                };
            }

            await _emailService.TransferRequestAsync(accountName, request.Currency, request.Amount, transfer.CreatedAt);

            return new ApiResponse { Success = true, Message = "Transfer successfully logged for processing", Data = reference };
        }

        public async Task<ApiResponse> ExistingRecipientAsync(TransferRequest request, long accountId, long recipientId, string reference, decimal fee, long walletId, string accountName)
        {
            var transfer = new Transfer
            {
                AccountId = accountId,
                Amount = request.Amount,
                AmountPaid = request.Amount,
                CreatedAt = DateTime.Now,
                Currency = request.Currency,
                Fee = fee,
                Gateway = PaymentGateways.Infinitus,
                PaymentMethod = request.PaymentMethod,
                Status = TransactionStatuses.Submitted,
                TransferRecipientId = recipientId,
                ProcessingStatus = TransactionStatuses.Pending,
                Reason = request.Reason,
                Reference = reference,
                SourceCurrency = request.Currency
            };
            bool insert = await _transferRepository.InsertAsync(transfer);
            if (!insert)
            {
                await ReverseAsync(request, reference, accountId, walletId, fee);

                transfer.Status = TransactionStatuses.Failed;
                await _transferRepository.UpdateAsync(transfer);

                return new ApiResponse
                {
                    Message = "Unable to initiate transfer"
                };
            }

            await _emailService.TransferRequestAsync(accountName, request.Currency, request.Amount, transfer.CreatedAt);

            return new ApiResponse { Success = true, Message = "Transfer successfully logged for processing", Data = reference };
        }


        public async Task<ApiResponse> ReverseAsync(TransferRequest request, string reference, long accountId, long walletId, decimal fee)
        {
            var settlement = new SettlementModel
            {
                Currency = request.Currency,
                AccountId = accountId,
                Reference = reference,
                TransactionAmount = request.Amount + fee,
                TransactionType = TransactionTypes.Transfer,
                WalletId = walletId,
                Note = $"{reference}|{request.Reason}",
                DebitCreditIndicator = DebitCreditIndicators.Credit
            };
            string credit = await _settlementRepository.ProcessAsync(settlement);
            if (string.IsNullOrEmpty(credit))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            if (!string.Equals(credit, "duplicate", StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            if (!string.Equals(credit, TransactionStatuses.Successful, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = credit
                };
            }

            return new ApiResponse { Success = true, Message = "Unable to process payment" };
        }

        public async Task<ApiResponse> ReverseAsync(Transfer transfer)
        {
            var wallet = await _walletRepository.GetAsync(transfer.AccountId, transfer.Currency);
            if (wallet is null)
            {
                return new ApiResponse
                {
                    Message = "No wallet found for the account"
                };
            }

            var settlement = new SettlementModel
            {
                Currency = transfer.Currency,
                AccountId = transfer.AccountId,
                Reference = transfer.Reference,
                TransactionAmount = transfer.Amount + transfer.Fee,
                TransactionType = TransactionTypes.Transfer,
                WalletId = wallet.Id,
                Note = $"{transfer.Reference}|{transfer.Reason}",
                DebitCreditIndicator = DebitCreditIndicators.Credit
            };
            string credit = await _settlementRepository.ProcessAsync(settlement);
            if (string.IsNullOrEmpty(credit))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            if (!string.Equals(credit, "duplicate", StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            if (!string.Equals(credit, TransactionStatuses.Successful, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = credit
                };
            }

            transfer.Status = TransactionStatuses.Reversed;
            transfer.UpdatedAt = DateTime.Now;
            await _transferRepository.UpdateAsync(transfer);

            return new ApiResponse { Success = true, Message = "Unable to process payment" };
        }

        public async Task ProcessAsync()
        {
            try
            {
                _logService.LogInfo(nameof(TransferService), nameof(ProcessAsync), "Commence the transfer processing");

                var transfers = await _transferRepository.GetByStatusAsync(TransactionStatuses.Approved);
                foreach (var transfer in transfers)
                {
                    try
                    {
                        if (string.Equals(transfer.Currency, Currencies.NGN, StringComparison.OrdinalIgnoreCase)) await ProcessGlobusAsync(transfer);

                        if (string.Equals(transfer.Currency, Currencies.USD, StringComparison.OrdinalIgnoreCase)) await ProcessInfinitusAsync(transfer);
                    }
                    catch (Exception ex)
                    {
                        _logService.LogError(nameof(TransferService), nameof(ProcessAsync), ex);
                    }
                }

                _logService.LogInfo(nameof(TransferService), nameof(ProcessAsync), "Ended the transfer processing");
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(TransferService), nameof(ProcessAsync), ex);
            }
        }

        public async Task<ApiResponse> ProcessInfinitusAsync(Transfer transfer)
        {
            var recipient = await _recipientRepository.DetailsAsync(transfer.TransferRecipientId);
            if (recipient is null)
            {
                _logService.LogInfo(nameof(TransferService), nameof(ProcessInfinitusAsync), $"Unable to fetch recipient for payout ID {transfer.Id}");
                return new ApiResponse
                {
                    Message = $"Unable to fetch recipient for payout ID {transfer.Id}"
                };
            }

            if (string.IsNullOrEmpty(recipient.Uuid))
            {
                var newResponse = await _infinitusService.CreateRecipientAsync(recipient, transfer.AccountId);
                if (!newResponse.Success)
                {
                    _logService.LogInfo(nameof(TransferService), nameof(ProcessInfinitusAsync), $"Unable to save recipient for payout ID {transfer.Id}");
                    return new ApiResponse
                    {
                        Message = $"Unable to save recipient for payout ID {transfer.Id}"
                    };
                }

                newResponse = await _infinitusService.CreateTransferAsync(transfer, newResponse.Message);
                if (!newResponse.Success)
                {
                    _logService.LogInfo(nameof(TransferService), nameof(ProcessInfinitusAsync), $"Unable to save recipient for payout ID {transfer.Id}");
                    return new ApiResponse
                    {
                        Message = $"Unable to save recipient for payout ID {transfer.Id}"
                    };
                }
            }

            var existingResponse = await _infinitusService.CreateTransferAsync(transfer, recipient.Uuid);
            if (!existingResponse.Success)
            {
                _logService.LogInfo(nameof(TransferService), nameof(ProcessInfinitusAsync), $"Unable to save recipient for payout ID {transfer.Id}");
                return new ApiResponse
                {
                    Message = $"Unable to save recipient for payout ID {transfer.Id}"
                };
            }

            return existingResponse;
        }

        public async Task<ApiResponse> ProcessGlobusAsync(Transfer transfer)
        {
            try
            {
                var recipient = await _recipientRepository.DetailsAsync(transfer.TransferRecipientId);
                if (recipient is null)
                {
                    return new ApiResponse
                    {
                        Message = $"Unable to fetch recipient for payout ID {transfer.Id}"
                    };
                }

                var balanceResponse = await _globusBankService.GetBalanceAsync(StaticData.GlobusLinkedAccount);
                if (balanceResponse is null || string.IsNullOrEmpty(balanceResponse.responsecode))
                {
                    return new ApiResponse
                    {
                        Message = "Service not reachable"
                    };
                }

                if (balanceResponse.result.balance < (double)transfer.Amount)
                {
                    return new ApiResponse
                    {
                        Message = "Unable to process transfer at the moment"
                    };
                }

                var request = new DoTransferRequest
                {
                    Transfer = transfer,
                    TransferRecipient = recipient
                };
                var response = await _globusBankService.DoTransferAsync(request);

                transfer.GatewayResponse = JsonConvert.SerializeObject(response.Data);
                transfer.Status = TransactionStatuses.Processing;
                await _transferRepository.UpdateAsync(transfer);

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(TransferService), nameof(ProcessGlobusAsync), ex);
            }

            return new ApiResponse { Message = "Kindly try again later" };
        }

        public async Task Process111Async()
        {
            try
            {
                var transfers = await _transferRepository.GetAsync(TransactionStatuses.Approved, PaymentGateways.Infinitus);
                foreach (var transfer in transfers)
                {
                    try
                    {
                        var recipient = await _recipientRepository.DetailsAsync(transfer.TransferRecipientId);
                        if (recipient is null)
                        {
                            _logService.LogInfo(nameof(TransferService), nameof(ProcessAsync), $"Unable to fetch recipient for payout ID {transfer.Id}");
                            continue;
                        }

                        if (string.IsNullOrEmpty(recipient.Uuid))
                        {
                            var newResponse = await _infinitusService.CreateRecipientAsync(recipient, transfer.AccountId);
                            if (!newResponse.Success)
                            {
                                _logService.LogInfo(nameof(TransferService), nameof(ProcessAsync), $"Unable to save recipient for payout ID {transfer.Id}");
                                continue;
                            }

                            newResponse = await _infinitusService.CreateTransferAsync(transfer, newResponse.Message);
                            if (!newResponse.Success)
                            {
                                _logService.LogInfo(nameof(TransferService), nameof(ProcessAsync), $"Unable to save recipient for payout ID {transfer.Id}");
                                continue;
                            }
                        }

                        var existingResponse = await _infinitusService.CreateTransferAsync(transfer, recipient.Uuid);
                        if (!existingResponse.Success)
                        {
                            _logService.LogInfo(nameof(TransferService), nameof(ProcessAsync), $"Unable to save recipient for payout ID {transfer.Id}");
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logService.LogError(nameof(TransferService), nameof(ProcessAsync), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(TransferService), nameof(ProcessAsync), ex);
            }
        }

        public async Task<ApiResponse> ProcessAsync(Transfer transfer)
        {
            var recipient = await _recipientRepository.DetailsAsync(transfer.TransferRecipientId);
            if (recipient is null)
            {
                return new ApiResponse
                {
                    Message = "No details found for the recipient"
                };
            }

            if (string.IsNullOrEmpty(recipient.Uuid))
            {
                var response = await _infinitusService.CreateRecipientAsync(recipient, transfer.AccountId);
                if (!response.Success) return response;

                response = await _infinitusService.CreateTransferAsync(transfer, response.Message);
                if (!response.Success) return response;
            }

            return await _infinitusService.CreateTransferAsync(transfer, recipient.Uuid);
        }

        public async Task<PaginatedResponse> GetAsync(QueryTransaction queryTransaction)
        {
            var payouts = await _transferRepository.GetAsync(queryTransaction);

            int totalCount = payouts.Count;

            payouts = payouts.Skip((queryTransaction.PageNumber - 1) * queryTransaction.PageSize).Take(queryTransaction.PageSize).ToList();

            return new PaginatedResponse(payouts, queryTransaction.PageNumber, queryTransaction.PageSize, totalCount, "Records retrieved successfully", true);
        }

        public async Task QueryStatusAsync()
        {
            try
            {
                var transfers = await _transferRepository.GetByStatusAsync(TransactionStatuses.Processing);
                foreach (var transfer in transfers)
                {
                    try
                    {
                        var response = new ApiResponse();

                        var account = await _accountRepository.DetailsAsync(transfer.AccountId);

                        var user = await _userRepository.GetDetailsByUserIdAsync(account.UserId);

                        var receipient = await _recipientRepository.DetailsAsync(transfer.TransferRecipientId);

                        if (string.Equals(transfer.Gateway, PaymentGateways.Globus, StringComparison.OrdinalIgnoreCase)) response = await _globusBankService.TransactionQueryAsync(transfer);

                        if (string.Equals(transfer.Gateway, PaymentGateways.Infinitus, StringComparison.OrdinalIgnoreCase)) response = await _infinitusService.TransferAsync(transfer, account.Uuid);

                        if (!response.Success) continue;

                        if (string.Equals(response.Message, InfinitusTransactionStatuses.Failed, StringComparison.OrdinalIgnoreCase))
                        {
                            await ReverseAsync(transfer);
                            //Send notification for reversal
                            await _emailService.FailedTransferAsync(transfer, user.EmailAddress, receipient.AccountNumber, receipient.AccountName, receipient.BankName);
                            continue;
                        }

                        await _emailService.SuccessfulTransferAsync(transfer, user.EmailAddress, receipient.AccountNumber, receipient.AccountName, receipient.BankName);
                    }
                    catch (Exception ex)
                    {
                        _logService.LogError(nameof(TransferService), nameof(ProcessAsync), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(TransferService), nameof(ProcessAsync), ex);
            }
        }
    }
}
