using AndcultureCode.CSharp.Core.Interfaces.Entity;

namespace AndcultureCode.CSharp.Core.Models.Entities
{
    public abstract class Entity : IEntity
    {
        /// <summary>
        /// Get or sets the id.
        /// </summary>
        public long Id { get; set; }
    }
}
