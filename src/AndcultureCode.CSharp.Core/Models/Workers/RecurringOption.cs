using System;
using AndcultureCode.CSharp.Core.Enumerations;

namespace AndcultureCode.CSharp.Core.Models.Entities.Worker
{
    /// <summary>
    /// Recurrance configuration for a given worker
    /// </summary>
    public class RecurringOption
    {
        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        public int Day { get; set; }
        
        /// <summary>
        /// Gets or sets the day of week.
        /// </summary>
        public DayOfWeek DayOfWeek { get; set; }
        
        /// <summary>
        /// Gets or sets the hour.
        /// </summary>
        public int Hour { get; set; }
        
        /// <summary>
        /// Gets or sets the minute.
        /// </summary>
        public int Minute { get; set; }
        
        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        public int Month { get; set; }
        
        /// <summary>
        /// Gets or sets the recurrence.
        /// </summary>
        public Recurrence Recurrence { get; set; }
    }
}
