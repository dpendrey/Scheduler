using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulerLib
{
    /// <summary>
    /// Provides a method of interfacing with the Scheduler engine to provide custom programmed actions
    /// </summary>
    public abstract class ClassStarter
    {
        /// <summary>
        /// Runs the action requested
        /// </summary>
        /// <param name="Task">Task being performed</param>
        /// <param name="Action">Action to be performed</param>
        /// <returns>True if the action was a success</returns>
        public abstract bool RunAction(Task Task, Action Action);
    }
}
