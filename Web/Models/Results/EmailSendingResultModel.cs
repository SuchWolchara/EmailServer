using Web.Enums;

namespace Web.Models.Results
{
    /// <summary>
    /// Модель ответа сервера о состоянии отправки письма
    /// </summary>
    public class EmailSendingResultModel
    {
        /// <summary>
        /// Получатель письма
        /// </summary>
        public string Recipient { get; set; } = string.Empty;

        /// <summary>
        /// Результат отправки письма
        /// </summary>
        public bool Result { get; set; } = true;

        /// <summary>
        /// Сообщение об ошибке отправки письма (может быть пустым)
        /// </summary>
        public FailedMessages FailedMessage { get; set; } = FailedMessages.None;
    }
}
