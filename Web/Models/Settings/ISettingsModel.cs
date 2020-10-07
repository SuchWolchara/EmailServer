namespace Web.Models.Settings
{
    /// <summary>
    /// Модель конфиг-файла с настройками
    /// </summary>
    public interface ISettingsModel
    {
        /// <summary>
        /// Ключ для получения настроек из конфиг-файла
        /// </summary>
        string Key { get; }
    }
}
