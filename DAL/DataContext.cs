using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    /// <summary>
    /// Класс, сопоставляющий сущности и связи с таблицами в БД
    /// </summary>
    public class DataContext : DbContext
    {
        /// <summary>
        /// Сущность таблицы Emails
        /// </summary>
        public DbSet<EmailEntity> Emails { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
