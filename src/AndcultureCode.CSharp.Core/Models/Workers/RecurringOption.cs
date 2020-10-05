using System;
using AndcultureCode.CSharp.Core.Enumerations;

namespace AndcultureCode.CSharp.Core.Models.Entities.Worker
{
    /// <summary>
    /// Recurrence configuration for a given worker
    /// </summary>
    public class RecurringOption
    {
        /// <summary>
        /// Integer representation of the day
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Specifies the day of the week
        /// </summary>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// Integer representation of the hour
        /// </summary>
        public int Hour { get; set; }

        /// <summary>
        /// Integer representation of the minute
        /// </summary>
        public int Minute { get; set; }

        /// <summary>
        /// Integer representation of the month
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        public Recurrence Recurrence { get; set; }
    }
}
