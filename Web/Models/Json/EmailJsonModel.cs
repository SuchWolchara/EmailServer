namespace Web.Models.Json
{
    /// <summary>
    /// Модель JSON POST запроса
    /// </summary>
    public class EmailJsonModel
    {
        /// <summary>
        /// Тема письма
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Текст письма
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Адреса получателей письма
        /// </summary>
        public string[] Recipients { get; set; }

        /// <summary>
        /// Подпись отправителя
        /// </summary>
        public string MailFrom { get; set; }
    }
}
