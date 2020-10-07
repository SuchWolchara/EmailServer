using System;

namespace DAL.Entities
{
    /// <summary>
    /// Сущность таблицы в БД
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Id записи
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// Дата создания записи
        /// </summary>
        DateTime DateCreated { get; set; }
    }
}
