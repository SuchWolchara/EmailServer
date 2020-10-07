namespace Web.Models.Settings
{
    /// <summary>
    /// Модель конфиг-файла с настройками Smtp сервера
    /// </summary>
    public class SmtpSettingsModel : ISettingsModel
    {
        public string Key { get; } = "SmtpSettings";

        /// <summary>
        /// Хост SMTP сервера
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Порт SMTP сервера
        /// </summary>
        public int Port { get; set; }
    }
}
