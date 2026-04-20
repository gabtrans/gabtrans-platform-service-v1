using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Models;
using MailKit.Security;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace GabTrans.Infrastructure.Integrations
{
    public class MailClientIntegration : IMailClientIntegration
    {
        private readonly ILogService _logService;

        public MailClientIntegration(ILogService logService)
        {
            _logService = logService;
        }

        public async Task<bool> SendAsync(SendMailRequest sendMailRequest)
        {
            string[] cc = null;
            string[] bcc = null;

            try
            {
                using var message = new MimeMessage();
                message.From.Add(new MailboxAddress(StaticData.SmtpDisplayName, StaticData.SmtpSender));

                if (sendMailRequest.Receiver.Contains(';'))
                {
                    foreach (var address in sendMailRequest.Receiver.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        message.To.Add(new MailboxAddress(string.Empty, address));
                    }
                }
                else
                {
                    message.To.Add(new MailboxAddress(string.Empty, sendMailRequest.Receiver));
                }

                if (!string.IsNullOrEmpty(sendMailRequest.Cc))
                {
                    if (sendMailRequest.Cc.Contains(',')) cc = sendMailRequest.Cc.Split(",");
                    if (sendMailRequest.Cc.Contains(';')) cc = sendMailRequest.Cc.Split(";");
                    if (cc.Length > 0)
                    {
                        for (int i = 0; i < cc.Length; i++)
                        {
                            message.Cc.Add(new MailboxAddress(string.Empty, cc[i]));
                        }
                    }
                }

                if (sendMailRequest.Bcc is not null)
                {
                    bcc = sendMailRequest.Bcc.Split(",");
                    if (bcc.Length > 0)
                    {
                        for (int i = 0; i < bcc.Length; i++)
                        {
                            message.Bcc.Add(new MailboxAddress(string.Empty, bcc[i]));
                        }
                    }
                }

                var bodyBuilder = new BodyBuilder();
                if (sendMailRequest.Attachments != null && sendMailRequest.Attachments.Count > 0)
                    foreach (var attachment in sendMailRequest.Attachments)
                        bodyBuilder.Attachments.Add(attachment.FileName, attachment.Attachment);

                bodyBuilder.TextBody = sendMailRequest.Message;
                bodyBuilder.HtmlBody = sendMailRequest.Message;
                message.Subject = sendMailRequest.Subject;

                message.Body = bodyBuilder.ToMessageBody();
                _logService.LogInfo("MailClientIntegration", "SendAsync", $"URL::{StaticData.SmtpHost}");
                using var client = new SmtpClient();
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                // SecureSocketOptions.StartTls force a secure connection over TLS
                await client.ConnectAsync(StaticData.SmtpHost, StaticData.SmtpPort, SecureSocketOptions.StartTls);

                // Console.WriteLine($"Username:: {StaticData.SmtpUsername}, Password:: {StaticData.SmtpPassword}");
                _logService.LogInfo("MailClientIntegration", "SendAsync", "About to authenticate");
                await client.AuthenticateAsync(
                    userName: StaticData.SmtpUsername,
                    password: StaticData.SmtpPassword
                );
                _logService.LogInfo("MailClientIntegration", "SendAsync", "After authenticated and about to send mail");
                await client.SendAsync(message);
                _logService.LogInfo("MailClientIntegration", "SendAsync", "After mail has been sent");
                await client.DisconnectAsync(true);
                // Console.WriteLine("Mail successfully sent to ::" + sendMailRequest.Receiver);
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError("MailClientIntegration", "SendAsync", ex);
            }
            return false;
        }


        public async Task<bool> Send1Async(SendMailRequest sendMailRequest)
        {
            string[] cc = null;
            string[] bcc = null;

            try
            {

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(StaticData.SmtpDisplayName, StaticData.SmtpSender));
                message.To.Add(new MailboxAddress("Recipient", "recipient@example.com"));
                message.Subject = "Test Email";

                message.Body = new TextPart("plain") { Text = "Hello from Zoho!" };

                using var client = new SmtpClient();
                client.ServerCertificateValidationCallback = (s, c, h, e) => true; // Use with caution
                await client.ConnectAsync("smtp.zoho.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("your@zoho.com", "yourpassword");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);


                //using var message = new MimeMessage();
                //message.From.Add(new MailboxAddress(StaticData.SmtpDisplayName, StaticData.SmtpSender));

                //if (sendMailRequest.Receiver.Contains(';'))
                //{
                //    foreach (var address in sendMailRequest.Receiver.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                //    {
                //        message.To.Add(new MailboxAddress(string.Empty, address));
                //    }
                //}
                //else
                //{
                //    message.To.Add(new MailboxAddress(string.Empty, sendMailRequest.Receiver));
                //}

                //if (!string.IsNullOrEmpty(sendMailRequest.Cc))
                //{
                //    if (sendMailRequest.Cc.Contains(',')) cc = sendMailRequest.Cc.Split(",");
                //    if (sendMailRequest.Cc.Contains(';')) cc = sendMailRequest.Cc.Split(";");
                //    if (cc.Length > 0)
                //    {
                //        for (int i = 0; i < cc.Length; i++)
                //        {
                //            message.Cc.Add(new MailboxAddress(string.Empty, cc[i]));
                //        }
                //    }
                //}

                //if (sendMailRequest.Bcc is not null)
                //{
                //    bcc = sendMailRequest.Bcc.Split(",");
                //    if (bcc.Length > 0)
                //    {
                //        for (int i = 0; i < bcc.Length; i++)
                //        {
                //            message.Bcc.Add(new MailboxAddress(string.Empty, bcc[i]));
                //        }
                //    }
                //}

                //var bodyBuilder = new BodyBuilder();
                //if (sendMailRequest.Attachments != null && sendMailRequest.Attachments.Count > 0)
                //    foreach (var attachment in sendMailRequest.Attachments)
                //        bodyBuilder.Attachments.Add(attachment.FileName, attachment.Attachment);

                //bodyBuilder.TextBody = sendMailRequest.Message;
                //bodyBuilder.HtmlBody = sendMailRequest.Message;
                //message.Subject = sendMailRequest.Subject;

                //message.Body = bodyBuilder.ToMessageBody();
                //_logService.LogInfo("MailClientIntegration", "SendAsync", $"URL::{StaticData.SmtpHost}");
                //using var client = new SmtpClient();
                //// SecureSocketOptions.StartTls force a secure connection over TLS
                //await client.ConnectAsync(StaticData.SmtpHost, StaticData.SmtpPort, SecureSocketOptions.StartTls);

                //// Console.WriteLine($"Username:: {StaticData.SmtpUsername}, Password:: {StaticData.SmtpPassword}");
                //_logService.LogInfo("MailClientIntegration", "SendAsync", "About to authenticate");
                //await client.AuthenticateAsync(
                //    userName: StaticData.SmtpUsername,
                //    password: StaticData.SmtpPassword
                //);
                //_logService.LogInfo("MailClientIntegration", "SendAsync", "After authenticated and about to send mail");
                //await client.SendAsync(message);
                //_logService.LogInfo("MailClientIntegration", "SendAsync", "After mail has been sent");
                //await client.DisconnectAsync(true);
                //// Console.WriteLine("Mail successfully sent to ::" + sendMailRequest.Receiver);
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError("MailClientIntegration", "SendAsync", ex);
            }
            return false;
        }

        //public async Task<bool> SendAsync(SendMailRequest sendMailRequest)
        //{
        //    try
        //    {
        //        //using SmtpClient client = new SmtpClient("smtpout.secureserver.net"); // Or relay-hosting.secureserver.net
        //        //client.Port = 465; // Or 25 if 465 doesn't work and your host allows it
        //        //client.EnableSsl = true;
        //        //client.UseDefaultCredentials = false; // Important!
        //        //client.Credentials = new NetworkCredential(StaticData.SmtpUsername, StaticData.SmtpPassword);

        //        //MailMessage mail = new MailMessage();
        //        //mail.From = new MailAddress(StaticData.SmtpUsername, StaticData.SmtpDisplayName);
        //        //mail.To.Add(sendMailRequest.Receiver);
        //        //mail.Subject = sendMailRequest.Subject;
        //        //mail.Body = sendMailRequest.Message;
        //        //mail.IsBodyHtml = true; // Set to true if your body contains HTML

        //        //client.Send(mail);

        //        var emailMessage = new MimeKit.MimeMessage();
        //        emailMessage.From.Add(new MailboxAddress("Your Name", StaticData.SmtpUsername));
        //        emailMessage.To.Add(new MailboxAddress("Recipient Name", sendMailRequest.Receiver));
        //        emailMessage.Subject = sendMailRequest.Subject;

        //        emailMessage.Body = new TextPart("plain")
        //        {
        //            Text = sendMailRequest.Message
        //        };

        //        using var client = new MailKit.Net.Smtp.SmtpClient();
        //        await client.ConnectAsync("smtpout.secureserver.net", 587, MailKit.Security.SecureSocketOptions.StartTls);
        //        await client.AuthenticateAsync(StaticData.SmtpUsername, StaticData.SmtpPassword);
        //        await client.SendAsync(emailMessage);
        //        await client.DisconnectAsync(true);

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions (e.g., log the error)
        //        Console.WriteLine($"Error sending email: {ex.Message}");
        //    }

        //    return false;
        //}
    }
}
