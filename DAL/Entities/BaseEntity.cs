using System;

namespace DAL.Entities
{
    /// <inheritdoc cref="IEntity"/>
    public class BaseEntity : IEntity
    {
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
