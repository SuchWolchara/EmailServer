namespace Web.Models.Settings
{
    /// <summary>
    /// Модель конфиг-файла с настройками Pop сервера
    /// </summary>
    public class PopSettingsModel : ISettingsModel
    {
        public string Key { get; } = "PopSettings";

        /// <summary>
        /// Хост Pop сервера
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Порт Pop сервера
        /// </summary>
        public int Port { get; set; }
    }
}
