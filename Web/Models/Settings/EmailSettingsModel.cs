namespace Web.Models.Settings
{
    /// <summary>
    /// Модель конфиг-файла с настройками Email сервера
    /// </summary>
    public class EmailSettingsModel : ISettingsModel
    {
        public string Key { get; } = "EmailSettings";

        /// <summary>
        /// Логин электронной почты
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Пароль электронной почты
        /// </summary>
        public string Password { get; set; }
    }
}
