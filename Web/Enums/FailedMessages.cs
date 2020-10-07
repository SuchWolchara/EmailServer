namespace Web.Enums
{
    /// <summary>
    /// Словарь ошибок при отправке письма
    /// </summary>
    public enum FailedMessages
    {
        /// <summary>
        /// Нет ошибки
        /// </summary>
        None = 0,
        /// <summary>
        /// Ошибка формата адреса
        /// </summary>
        Format = 1,
        /// <summary>
        /// Несуществующий домен
        /// </summary>
        Domain = 2,
        /// <summary>
        /// Несуществующий пользователь
        /// </summary>
        User = 3
    }
}
