using DAL.Entities;
using Web.Enums;
using Web.Models.Json;

namespace Web.Helpers
{
    /// <summary>
    /// Помощник в работе с информацией о письмах
    /// </summary>
    public static class EmailHelper
    {
        private static readonly string[] _failedMessages = {
            string.Empty,
            "Неверный формат электронного адреса",
            "Несуществующий домен",
            "Несуществующий пользователь"
        };

        /// <summary>
        /// Метод, собирающий всю информацию о письме в объект EmailEntity и возвращающий его
        /// </summary>
        public static EmailEntity GetEmailEntity(EmailJsonModel emailJsonModel, string recipient, bool result, FailedMessages failedMessage)
        {
            return new EmailEntity()
            {
                Subject = emailJsonModel.Subject,
                Body = emailJsonModel.Body,
                Recipient = recipient,
                MailFrom = emailJsonModel.MailFrom,
                Result = result,
                FailedMessage = _failedMessages[(int)failedMessage]
            };
        }
    }
}
