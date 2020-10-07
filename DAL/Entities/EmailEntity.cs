namespace DAL.Entities
{
    /// <summary>
    /// Сущность таблицы Emails в БД
    /// </summary>
    public class EmailEntity : BaseEntity
    {
        /// <summary>
        /// Тема письма
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Текст письма
        /// </summary>
        /// 
        public string Body { get; set; }

        /// <summary>
        /// Адрес получателя письма
        /// </summary>
        /// 
        public string Recipient { get; set; }

        /// <summary>
        /// Подпись отправителя
        /// </summary>
        /// 
        public string MailFrom { get; set; }

        /// <summary>
        /// Результат отправки письма
        /// </summary>
        /// 
        public bool Result { get; set; }

        /// <summary>
        /// Сообщение об ошибке отправки письма (может быть пустым)
        /// </summary>
        /// 
        public string FailedMessage { get; set; }
    }
}
