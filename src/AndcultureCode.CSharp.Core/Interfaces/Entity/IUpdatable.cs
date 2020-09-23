using System;

namespace AndcultureCode.CSharp.Core.Interfaces.Entity
{
    public interface IUpdatable
    {
        /// <summary>
        /// Gets or sets the updated by identifier.
        /// </summary>
        long?           UpdatedById { get; set; }
       
        /// <summary>
        /// Gets or sets the updated on.
        /// </summary>
        DateTimeOffset? UpdatedOn   { get; set; }
    }
}
