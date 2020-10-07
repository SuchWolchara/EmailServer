using Microsoft.Extensions.Configuration;
using Web.Models.Settings;

namespace Web.Helpers
{
    /// <summary>
    /// Помощник в работе с настройками сервера
    /// </summary>
    public static class SettingsHelper
    {
        /// <summary>
        /// Метод, позволяющий получить настройки сервера из конфиг-файла
        /// </summary>
        public static void GetServerSettings(IConfiguration configuration, ISettingsModel settingsModel)
        {
            configuration.GetSection(settingsModel.Key).Bind(settingsModel);
        }
    }
}
