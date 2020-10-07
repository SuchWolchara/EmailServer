using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Web.Enums;
using Web.Helpers;
using Web.Models.Json;
using Web.Models.Results;
using Web.Models.Settings;

namespace Web.Services
{
    /// <inheritdoc cref="IEmailService"/>
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly Pop3Client _popClient;

        private readonly EmailSettingsModel _emailSettings;
        private readonly SmtpSettingsModel _smtpSettings;
        private readonly PopSettingsModel _popSettings;

        public Regex EmailRegex { get; }

        public EmailService(IConfiguration configuration)
        {
            EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            _smtpClient = new SmtpClient();
            _popClient = new Pop3Client();

            _emailSettings = new EmailSettingsModel();
            _smtpSettings = new SmtpSettingsModel();
            _popSettings = new PopSettingsModel();

            GetServerSettings(configuration);
        }

        public async Task<IEnumerable<EmailSendingResultModel>> TrySendAsync(IEnumerable<MailboxAddress> recipients, EmailJsonModel emailJsonModel)
        {
            if (!recipients.Any())
                return new List<EmailSendingResultModel>();

            GetMimeMessage(recipients, emailJsonModel, out MimeMessage message);

            await TryConnectPopAsync();
            var countBefore = await _popClient.GetMessageCountAsync();
            await TryDisconnectPopAsync();

            try
            {
                await TryConnectSmtpAsync();
                await _smtpClient.SendAsync(message);
            }
            finally
            {
                await TryDisconnectSmtpAsync();
            }

            var sendingResults = new List<EmailSendingResultModel>();

            foreach (var recipient in recipients)
                sendingResults.Add(new EmailSendingResultModel() { Recipient = recipient.Address });

            await GetSendingResultAsync(sendingResults, countBefore);

            return sendingResults;
        }

        #region Get methods

        private void GetServerSettings(IConfiguration configuration)
        {
            SettingsHelper.GetServerSettings(configuration, _emailSettings);
            SettingsHelper.GetServerSettings(configuration, _smtpSettings);
            SettingsHelper.GetServerSettings(configuration, _popSettings);
        }

        private void GetMimeMessage(IEnumerable<MailboxAddress> recipients, EmailJsonModel emailJsonModel, out MimeMessage message)
        {
            message = new MimeMessage()
            {
                From = { new MailboxAddress(emailJsonModel.MailFrom ?? string.Empty, _emailSettings.UserName) },
                Subject = emailJsonModel.Subject ?? string.Empty,
                Body = new TextPart(TextFormat.Html) { Text = emailJsonModel.Body ?? string.Empty }
            };
            message.To.AddRange(recipients);
        }

        private async Task GetSendingResultAsync(List<EmailSendingResultModel> sendingResults, int countBefore)
        {
            var countAfter = countBefore;

            var timer = new Stopwatch();
            timer.Start();

            while (countAfter == countBefore)
            {
                await TryConnectPopAsync();
                countAfter = await _popClient.GetMessageCountAsync();
                await TryDisconnectPopAsync();

                if (timer.ElapsedMilliseconds > 5000)
                    break;
            }

            timer.Reset();

            if (countAfter > countBefore)
            {
                var indexes = new List<int>();

                for (int i = countBefore; i < countAfter; i++)
                    indexes.Add(i);

                await TryConnectPopAsync();
                var messages = await _popClient.GetMessagesAsync(indexes);
                await TryDisconnectPopAsync();

                var flag = false;

                foreach (var message in messages)
                {
                    if (message.From.Mailboxes.Any(item => item.Name == "Mail Delivery Subsystem"
                        && item.Address == "mailer-daemon@googlemail.com")
                        && message.Subject == "Delivery Status Notification (Failure)")
                    {
                        flag = true;
                        var stringSplit = message.TextBody.Split(' ');
                        var recipient = stringSplit.FirstOrDefault(str => EmailRegex.IsMatch(str));

                        var index = sendingResults.IndexOf(sendingResults.FirstOrDefault(item => item.Recipient == recipient));
                        sendingResults[index].Result = false;

                        if (message.TextBody.Contains("550 5.7.1"))
                            sendingResults[index].FailedMessage = FailedMessages.User;
                        else
                            sendingResults[index].FailedMessage = FailedMessages.Domain;
                    }
                }

                if (flag && sendingResults.Any(item => item.Result))
                    await GetSendingResultAsync(sendingResults, countBefore);
            }
        }

        #endregion

        #region Connection methods

        private async Task TryConnectSmtpAsync()
        {
            if (!_smtpClient.IsConnected)
                await _smtpClient.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, true);
            if (!_smtpClient.IsAuthenticated)
                await _smtpClient.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
        }

        private async Task TryConnectPopAsync()
        {
            if (!_popClient.IsConnected)
                await _popClient.ConnectAsync(_popSettings.Host, _popSettings.Port, true);
            if (!_popClient.IsAuthenticated)
                await _popClient.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
        }

        private async Task TryDisconnectSmtpAsync()
        {
            if (_smtpClient.IsConnected)
                await _smtpClient.DisconnectAsync(true);
        }

        private async Task TryDisconnectPopAsync()
        {
            if (_popClient.IsConnected)
                await _popClient.DisconnectAsync(true);
        }

        #endregion
    }
}
