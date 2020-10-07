using MimeKit;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Web.Models.Json;
using Web.Models.Results;

namespace Web.Services
{
    /// <summary>
    /// Cервис для отправки писем через SMTP сервер
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Регулярное выражение для проверки правильности формата электронного адреса
        /// </summary>
        Regex EmailRegex { get; }
        /// <summary>
        /// Метод, выполняющий асинхронную отправку письма через SMTP сервер и возвращающий ответ о её состоянии
        /// </summary>
        Task<IEnumerable<EmailSendingResultModel>> TrySendAsync(IEnumerable<MailboxAddress> recipients, EmailJsonModel emailJsonModel);
    }
}
