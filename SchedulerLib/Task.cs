using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulerLib
{
    public abstract class Task
    {
        protected object lockObj = new object();


        public abstract DateTime LastAttempted { get; }
        public abstract DateTime LastRun { get; }
        public abstract string Name { get; }
        public abstract TaskStatus Status { get; }

        public abstract bool QuickCheck();

        public abstract bool FullCheck();

        public abstract void TaskPending();
        public void RunTask()
        {
            System.Threading.Thread thr = new System.Threading.Thread(new System.Threading.ThreadStart(runTask));
            thr.Priority = System.Threading.ThreadPriority.Lowest;
            thr.Start();
        }

        protected abstract void runTask();

        public delegate void TaskDelegate(Task Task);
    }
}
