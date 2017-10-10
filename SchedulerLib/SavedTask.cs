using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulerLib
{
    /// <summary>
    /// A task which is saved to disk to be run later
    /// </summary>
    public class SavedTask : SimpleTask
    {
        public SavedTask(string Filename)
        {
            filename = Filename;
        }

        private DateTime lastAttempted, lastRun;
        private string name;
        private string filename;

        public override DateTime LastAttempted { get { return lastAttempted; } }
        public override DateTime LastRun { get { return lastRun; } }
        public override string Name { get { return name; } }


        #region Load data
        public void LoadData()
        {
            lock (lockObj)
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(filename);
                string curLine;

                while ((curLine = reader.ReadLine()) != null)
                {
                    curLine = curLine.Trim();
                    string upper = curLine.ToUpper();

                    if (upper.StartsWith("NAME="))
                        name = curLine.Substring("NAME=".Length);
                    if (upper.StartsWith("LASTRUN="))
                        lastRun = new DateTime((long.Parse(curLine.Substring("LASTRUN=".Length))));
                    if (upper.StartsWith("LASTATTEMPTED="))
                        lastAttempted = new DateTime((long.Parse(curLine.Substring("LASTATTEMPTED=".Length))));
                    if (upper.StartsWith("STARTCONDITION."))
                    {
                        int condition = int.Parse(name.Substring("STARTCONDITION.".Length, curLine.IndexOf('.', "STARTCONDITION.".Length) - "STARTCONDITION.".Length));
                        upper = upper.Substring(curLine.IndexOf('.', "STARTCONDITION.".Length));
                        curLine = curLine.Substring(curLine.IndexOf('.', "STARTCONDITION.".Length));

                        lock (lockObj)
                            while (condition >= startConditions.Length)
                                AddStartCondition(new StartCondition());

                        startConditions[condition].ReadData(curLine, upper);
                    }
                    if (upper.StartsWith("ACTION."))
                    {
                        int action = int.Parse(name.Substring("ACTION.".Length, curLine.IndexOf('.', "ACTION.".Length) - "ACTION.".Length));
                        upper = upper.Substring(curLine.IndexOf('.', "ACTION.".Length));
                        curLine = curLine.Substring(curLine.IndexOf('.', "ACTION.".Length));

                        lock (lockObj)
                            while (action >= actions.Length)
                                AddAction(new Action());

                        actions[action].ReadData(curLine, upper);
                    }
                }

                reader.Close();
            }
        }
        #endregion

        #region Save data
        public void SaveData()
        {
            lock (lockObj)
            {
                System.IO.StreamWriter writer = new System.IO.StreamWriter(filename);

                writer.WriteLine("Name=" + name);
                writer.WriteLine("LastRun=" + lastRun.Ticks.ToString());
                writer.WriteLine("LastAttempted=" + lastAttempted.Ticks.ToString());
                for (int i = 0; i < startConditions.Length; i++)
                    startConditions[i].WriteData(writer, i);
                for (int i = 0; i < actions.Length; i++)
                    actions[i].WriteData(writer, i);

                writer.Flush();
                writer.Close();
            }
        }
        #endregion

        public override void TaskPending()
        {
            lock (lockObj)
            {
                lastAttempted = DateTime.Now;
                SaveData();
            }
            base.TaskPending();
        }

        protected override void runTask()
        {
            base.RunTask();

            if (Status == TaskStatus.Idle)
            {
                lock (lockObj)
                {
                    lastRun = DateTime.Now;
                    SaveData();
                }
            }
        }
    }
}
