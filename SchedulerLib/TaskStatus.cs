using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulerLib
{
    /// <summary>
    /// All possible task statuses
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// The task is idle and awaiting valid start conditions
        /// </summary>
        Idle,
        /// <summary>
        /// The start conditiosn have been met an the task is awaiting a start
        /// </summary>
        Pending,
        /// <summary>
        /// The task is currently running
        /// </summary>
        Running,
        /// <summary>
        /// A fault occured while trying to run this task
        /// </summary>
        Fault
    }
}
