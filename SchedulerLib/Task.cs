using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulerLib
{
    public class Task
    {
        private object lockObj = new object();

        private DateTime lastAttempted, lastRun;
        private string name, filename;
        private StartCondition[] startConditions = new StartCondition[0];
        private Action[] actions = new Action[0];

        public DateTime LastAttempted { get { return lastAttempted; } }
        public DateTime LastRun { get { return lastRun; } }
        public string Name { get { return name; } set { name = value; } }
        #region Conditions
        public StartCondition[] StartConditions
        {
            get
            {
                StartCondition[] retVal;
                lock (lockObj)
                {
                    retVal = new StartCondition[startConditions.Length];
                    for (int i = 0; i < startConditions.Length; i++)
                        retVal[i] = startConditions[i];
                }
                return retVal;
            }
        }
        public void AddStartCondition(StartCondition Condition)
        {
            lock (lockObj)
            {
                StartCondition[] newValues = new StartCondition[startConditions.Length + 1];
                for (int i = 0; i < startConditions.Length; i++)
                    newValues[i] = startConditions[i];
                newValues[newValues.Length - 1] = Condition;
                startConditions = newValues;
            }
        }
        public void RemoveStartCondition(StartCondition Condition)
        {
            lock (lockObj)
            {
                StartCondition[] newValues = new StartCondition[startConditions.Length - 1];
                int i = 0;
                for (; i < startConditions.Length && newValues[i] != Condition; i++)
                    newValues[i] = startConditions[i];
                for (; i < startConditions.Length; i++)
                    newValues[i] = startConditions[i + 1];

                startConditions = newValues;
            }
        }
        #endregion
        #region Actions
        public Action[] Actions
        {
            get
            {
                Action[] retVal;
                lock (lockObj)
                {
                    retVal = new Action[Actions.Length];
                    for (int i = 0; i < Actions.Length; i++)
                        retVal[i] = Actions[i];
                }
                return retVal;
            }
        }
        public void AddAction(Action Condition)
        {
            lock (lockObj)
            {
                Action[] newValues = new Action[Actions.Length + 1];
                for (int i = 0; i < Actions.Length; i++)
                    newValues[i] = Actions[i];
                newValues[newValues.Length - 1] = Condition;
                actions = newValues;
            }
        }
        public void RemoveAction(Action Condition)
        {
            lock (lockObj)
            {
                Action[] newValues = new Action[Actions.Length - 1];
                int i = 0;
                for (; i < Actions.Length && newValues[i] != Condition; i++)
                    newValues[i] = Actions[i];
                for (; i < Actions.Length; i++)
                    newValues[i] = Actions[i + 1];

                actions = newValues;
            }
        }
        #endregion

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

        internal void LoadData(string Path)
        {
            filename = Path;
            LoadData();
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

        public delegate void TaskDelegate(Task Task);
    }
}
