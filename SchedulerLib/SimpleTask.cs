using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulerLib
{
    /// <summary>
    /// A task class which makes use of Actions and StartConditions to execute
    /// </summary>
    public class SimpleTask : Task
    {
        public SimpleTask()
        { }
        public SimpleTask(string Name)
        {
            name = Name;
        }
        public SimpleTask(string Name, StartCondition[] StartConditions,Action[] Actions)
        {
            name = Name;
            startConditions = StartConditions;
            actions = Actions;
        }

        protected StartCondition[] startConditions = new StartCondition[0];
        protected Action[] actions = new Action[0];
        private DateTime lastAttempted = DateTime.MinValue,
            lastRun = DateTime.MinValue;
        private string name;
        private TaskStatus status = TaskStatus.Idle;

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

        public override string Name
        {
            get { return name; }
        }

        public override DateTime LastAttempted
        {
            get { return lastAttempted; }
        }
        public override DateTime LastRun
        {
            get { return lastRun; }
        }

        public override bool QuickCheck()
        {
            foreach (StartCondition condition in startConditions)
                if (!condition.QuickCheck(this))
                    return false;
            return true;
        }

        public override bool FullCheck()
        {
            foreach (StartCondition condition in startConditions)
                if (!condition.FullCheck(this))
                    return false;
            return true;
        }

        public override void TaskPending()
        {
            lock (lockObj)
                if (status == TaskStatus.Idle)
                    status = TaskStatus.Pending;
        }

        protected override void runTask()
        {
            lock (lockObj)
            {
                if (status == TaskStatus.Pending)
                    status = TaskStatus.Running;
                lastAttempted = DateTime.Now;
            }

            foreach (Action action in actions)
                action.Enact(this);



            lock (lockObj)
            {
                if (status == TaskStatus.Running)
                    status = TaskStatus.Idle;
                lastRun = DateTime.Now;
            }
        }

        public override TaskStatus Status
        {
            get { return status; }
        }
    }
}
