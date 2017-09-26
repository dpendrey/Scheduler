using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulerLib
{
    public static class Engine
    {
        private static List<Task> tasks = new List<Task>();

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

            #region List all task files
            string[] taskFiles = System.IO.Directory.GetFiles(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DPend", "Scheduler"), "*.task");
            for(int i=0;i< taskFiles.Length;i++)
            {
                Task newTask = new Task();
                newTask.LoadData(taskFiles[i]);
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

        public static event Task.TaskDelegate TaskRemoved, TaskAdded;
    }
}
