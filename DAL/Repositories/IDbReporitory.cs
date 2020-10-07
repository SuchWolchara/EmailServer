using DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    /// <summary>
    /// Репозиторий для взаимодействия с БД
    /// </summary>
    public interface IDbReporitory
    {
        /// <summary>
        /// Метод, позволяющий получить все записи из указанной сущности в БД
        /// </summary>
        IQueryable<T> GetAll<T>() where T : class, IEntity;
        /// <summary>
        /// Метод, асинхронно выполняющий отправку новых записей в БД
        /// </summary>
        Task AddRangeAsync<T>(IEnumerable<T> newEntities) where T : class, IEntity;
        /// <summary>
        /// Метод, асинхронно выполняющий сохранение внесенных в БД изменений
        /// </summary>
        Task<int> SaveChangesAsync();
    }
}
