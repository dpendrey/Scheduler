using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulerLib
{
    public static class Engine
    {
        private static List<Task> tasks = new List<Task>(),
            runningTasks = new List<Task>();

        public static void LoadTasks()
        {
            #region Remove all tasks from list
            lock (tasks)
            {
                if (TaskRemoved != null)
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        try
                        {
                            TaskRemoved(tasks[i]);
                        }
                        catch (Exception) { }
                    }
                tasks.Clear();
            }
            #endregion

            #region Ensure directory exists
            if (!System.IO.Directory.Exists(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DPend", "Scheduler")))
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DPend", "Scheduler"));
            #endregion


            #region List all task files
            string[] taskFiles = System.IO.Directory.GetFiles(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DPend", "Scheduler"), "*.task");
            for (int i = 0; i < taskFiles.Length; i++)
            {
                SavedTask newTask = new SavedTask(taskFiles[i]);
                newTask.LoadData();
                lock (tasks)
                    tasks.Add(newTask);

                if (TaskAdded != null)
                    try
                    {
                        TaskAdded(newTask);
                    }
                    catch (Exception) { }
            }
            #endregion
        }

        public static void RemoveTask(Task Task)
        {
            lock (tasks)
            {
                tasks.Remove(Task);
            }
            if (TaskRemoved != null)
                try
                {
                    TaskRemoved(Task);
                }
                catch (Exception) { }
        }

        public static void DeleteTask(Task Task)
        {
            throw new NotImplementedException();
        }

        public static void AddTask(Task Task)
        {
            lock (tasks)
                tasks.Add(Task);
            if (TaskAdded != null)
                try
                {
                    TaskAdded(Task);
                }
                catch (Exception) { }
        }

        public delegate void EmptyDelegate();

        public static event Task.TaskDelegate TaskRemoved, TaskAdded;
        public static event EmptyDelegate SchedulingStarted, SchedulingStopped;

        private static System.Threading.Thread thr = null;
        private static bool continueRunning = true;
        private static Task currentTask = null;

        public static void StartScheduling()
        {
            lock (tasks)
            {
                continueRunning = true;
                if (!SchedulingRunning)
                {
                    thr = new System.Threading.Thread(new System.Threading.ThreadStart(runSchedules));
                    thr.Start();
                }
            }
        }

        public static void StopScheduling()
        {
            continueRunning = false;
        }

        public static bool SchedulingRunning
        {
            get
            {
                if (thr == null || !thr.IsAlive)
                    return false;
                else
                    return true;
            }
        }

        private static void runSchedules()
        {
            List<Task> tasksToStart = new List<Task>();

            while (continueRunning)
            {
                tasksToStart.Clear();

                lock (tasks)
                {
                    foreach (Task t in tasks)
                    {
                        if (t.QuickCheck())
                        {
                            if (t.FullCheck())
                            {
                                tasksToStart.Add(t);
                                t.TaskPending();
                            }
                        }
                    }
                }

                foreach (Task t in tasksToStart)
                {
                    runningTasks.Add(t);
                    t.RunTask();
                }

                foreach (Task t in runningTasks)
                {
                    if (t.Status == TaskStatus.Fault || t.Status == TaskStatus.Idle)
                    {
                        runningTasks.Remove(t);
                        break;
                    }
                }

                if (runningTasks.Count > 0)
                    System.Threading.Thread.Sleep(100);
                else
                    System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
