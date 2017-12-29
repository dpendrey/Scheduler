using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulerLib
{
    /// <summary>
    /// Represents a condition which must be met before a task will start
    /// </summary>
    public class StartCondition
    {
        private StartConditionTypes type;
        private object data1, data2, data3, data4, data5;

        public StartCondition()
        {
        }

        public StartCondition(StartConditionTypes ConditionType)
        {
            type = ConditionType;
        }

        public StartCondition(StartConditionTypes ConditionType, object ConditionData1)
        {
            type = ConditionType;
            data1 = ConditionData1;
        }

        public StartCondition(StartConditionTypes ConditionType, object ConditionData1, object ConditionData2)
        {
            type = ConditionType;
            data1 = ConditionData1;
            data2 = ConditionData2;
        }

        public StartCondition(StartConditionTypes ConditionType, object ConditionData1, object ConditionData2, object ConditionData3)
        {
            type = ConditionType;
            data1 = ConditionData1;
            data2 = ConditionData2;
            data3 = ConditionData3;
        }

        public StartCondition(StartConditionTypes ConditionType, object ConditionData1, object ConditionData2, object ConditionData3, object ConditionData4)
        {
            type = ConditionType;
            data1 = ConditionData1;
            data2 = ConditionData2;
            data3 = ConditionData3;
            data4 = ConditionData4;
        }

        public StartCondition(StartConditionTypes ConditionType, object ConditionData1, object ConditionData2, object ConditionData3, object ConditionData4, object ConditionData5)
        {
            type = ConditionType;
            data1 = ConditionData1;
            data2 = ConditionData2;
            data3 = ConditionData3;
            data4 = ConditionData4;
            data5 = ConditionData5;
        }

        /// <summary>
        /// Gets/sets the type of this condition
        /// </summary>
        public StartConditionTypes ConditionType { get { return type; } set { type = value; } }
        /// <summary>
        /// Gets/sets the first piece of data for this condition
        /// </summary>
        public object ConditionData1 { get { return data1; } set { data1 = value; } }
        /// <summary>
        /// Gets/sets the second piece of data for this condition
        /// </summary>
        public object ConditionData2 { get { return data2; } set { data2 = value; } }
        /// <summary>
        /// Gets/sets the third piece of data for this condition
        /// </summary>
        public object ConditionData3 { get { return data3; } set { data3 = value; } }
        /// <summary>
        /// Gets/sets the fourth piece of data for this condition
        /// </summary>
        public object ConditionData4 { get { return data4; } set { data4 = value; } }
        /// <summary>
        /// Gets/sets the fifth piece of data for this condition
        /// </summary>
        public object ConditionData5 { get { return data5; } set { data5 = value; } }

        /// <summary>
        /// Runs a quick test to determine if this condition has been passed
        /// </summary>
        /// <returns>True if the condition is passed (or if this is a lengthy test to run)</returns>
        public bool QuickCheck(Task Task)
        {
            switch (type)
            {
                #region No start condition
                case StartConditionTypes.None:
                    return false;
                #endregion

                #region Timed start conditions
                case StartConditionTypes.FixedTime:
                    return (DateTime.Now >= (DateTime)data1);
                case StartConditionTypes.DelayedTime:
                    throw new NotImplementedException();
                case StartConditionTypes.MinuteSchedule:
                    return (DateTime.Now >= Task.LastRun.AddMinutes((int)data1));
                case StartConditionTypes.HourlySchedule:
                    return (DateTime.Now >= Task.LastRun.AddHours((int)data1));
                case StartConditionTypes.DailySchedule:
                    return (DateTime.Now >= Task.LastRun.AddDays((int)data1));
                case StartConditionTypes.WeeklySchedule:
                    return (DateTime.Now >= Task.LastRun.AddDays(7 * (int)data1));
                case StartConditionTypes.MonthlySchedule:
                    return (DateTime.Now >= Task.LastRun.AddMonths((int)data1));
                case StartConditionTypes.YearlySchedule:
                    return (DateTime.Now >= Task.LastRun.AddYears((int)data1));
                case StartConditionTypes.WeekdaySchedule:
                    throw new NotImplementedException();
                case StartConditionTypes.WeekendSchedule:
                    throw new NotImplementedException();
                case StartConditionTypes.HourlyAt:
                    return (Task.LastRun.AddHours(1).)
                case StartConditionTypes.DailyAt:
                case StartConditionTypes.WeeklyAt:
                #endregion

                #region System resource conditions
                case StartConditionTypes.SystemIdle:
                case StartConditionTypes.SystemLowUse:
                case StartConditionTypes.SystemLowDiskUse:
                case StartConditionTypes.SystemLowMemoryUse:
                case StartConditionTypes.SystemLowCPUUse:
                #endregion

                #region Computer events
                case StartConditionTypes.SystemBoot:
                case StartConditionTypes.SystemWake:
                case StartConditionTypes.NetworkAccess:
                case StartConditionTypes.InternetAccess:
                case StartConditionTypes.OnNetwork:
                case StartConditionTypes.DriveAccess:
                    throw new NotImplementedException();
                case StartConditionTypes.FolderAccess:
                    return true;
                case StartConditionTypes.FTPAccess:
                #endregion

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Runs a complete test to determine if this condition has been passed
        /// </summary>
        /// <returns>True if this condition is passed</returns>
        public bool FullCheck(Task Task)
        {
            switch (type)
            {

                #region No start condition
                case StartConditionTypes.None:
                    return false;
                #endregion

                #region Timed start conditions
                case StartConditionTypes.FixedTime:
                case StartConditionTypes.DelayedTime:
                case StartConditionTypes.MinuteSchedule:
                case StartConditionTypes.HourlySchedule:
                case StartConditionTypes.DailySchedule:
                case StartConditionTypes.WeeklySchedule:
                case StartConditionTypes.MonthlySchedule:
                case StartConditionTypes.YearlySchedule:
                case StartConditionTypes.WeekdaySchedule:
                case StartConditionTypes.WeekendSchedule:
                    return true;    // Assume we've already passed since we made it through the quick test)
                #endregion

                #region System resource conditions
                case StartConditionTypes.SystemIdle:
                case StartConditionTypes.SystemLowUse:
                case StartConditionTypes.SystemLowDiskUse:
                case StartConditionTypes.SystemLowMemoryUse:
                case StartConditionTypes.SystemLowCPUUse:
                #endregion

                #region Computer events
                case StartConditionTypes.SystemBoot:
                case StartConditionTypes.SystemWake:
                case StartConditionTypes.NetworkAccess:
                case StartConditionTypes.InternetAccess:
                case StartConditionTypes.OnNetwork:
                case StartConditionTypes.DriveAccess:
                    throw new NotImplementedException();
                case StartConditionTypes.FolderAccess:
                    return System.IO.Directory.Exists((string)data1);
                case StartConditionTypes.FTPAccess:
                #endregion

                default:
                    throw new NotImplementedException();
            }
        }

        internal void ReadData(string Data)
        {
            ReadData(Data, Data.ToUpper());
        }
        internal void ReadData(string Data, string UpperData)
        {
            if (UpperData.StartsWith("TYPE="))
                type = (StartConditionTypes)int.Parse(Data.Substring("TYPE=".Length));
            if (UpperData.StartsWith("DATA1="))
            {
                switch (type)
                {
                    #region No start condition
                    case StartConditionTypes.None:
                        break;
                    #endregion

                    #region Timed start conditions
                    case StartConditionTypes.FixedTime:
                        data1 = new DateTime(long.Parse(Data.Substring("DATA1=".Length)));
                        break;
                    case StartConditionTypes.DelayedTime:
                        throw new NotImplementedException();
                    case StartConditionTypes.MinuteSchedule:
                    case StartConditionTypes.HourlySchedule:
                    case StartConditionTypes.DailySchedule:
                    case StartConditionTypes.WeeklySchedule:
                    case StartConditionTypes.MonthlySchedule:
                    case StartConditionTypes.YearlySchedule:
                        data1 = int.Parse(Data.Substring("DATA1=".Length));
                        break;
                    case StartConditionTypes.WeekdaySchedule:
                        throw new NotImplementedException();
                    case StartConditionTypes.WeekendSchedule:
                        throw new NotImplementedException();
                    #endregion

                    #region System resource conditions
                    case StartConditionTypes.SystemIdle:
                    case StartConditionTypes.SystemLowUse:
                    case StartConditionTypes.SystemLowDiskUse:
                    case StartConditionTypes.SystemLowMemoryUse:
                    case StartConditionTypes.SystemLowCPUUse:
                    #endregion

                    #region Computer events
                    case StartConditionTypes.SystemBoot:
                    case StartConditionTypes.SystemWake:
                    case StartConditionTypes.NetworkAccess:
                    case StartConditionTypes.InternetAccess:
                    case StartConditionTypes.OnNetwork:
                    case StartConditionTypes.DriveAccess:
                    case StartConditionTypes.FolderAccess:
                    case StartConditionTypes.FTPAccess:
                    #endregion

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        internal void WriteData(System.IO.StreamWriter Writer, int Index)
        {
            Writer.WriteLine("StartCondition." + Index.ToString() + ".Type=" + ((int)type).ToString());

            switch (type)
            {
                #region No start condition
                case StartConditionTypes.None:
                    break;
                #endregion

                #region Timed start conditions
                case StartConditionTypes.FixedTime:
                    Writer.WriteLine("StartCondition." + Index.ToString() + ".Data1=" + ((DateTime)data1).Ticks);
                    break;
                case StartConditionTypes.DelayedTime:
                    throw new NotImplementedException();
                case StartConditionTypes.MinuteSchedule:
                case StartConditionTypes.HourlySchedule:
                case StartConditionTypes.DailySchedule:
                case StartConditionTypes.WeeklySchedule:
                case StartConditionTypes.MonthlySchedule:
                case StartConditionTypes.YearlySchedule:
                    Writer.WriteLine("StartCondition." + Index.ToString() + ".Data1=" + ((int)data1).ToString());
                    break;
                case StartConditionTypes.WeekdaySchedule:
                    throw new NotImplementedException();
                case StartConditionTypes.WeekendSchedule:
                    throw new NotImplementedException();
                #endregion

                #region System resource conditions
                case StartConditionTypes.SystemIdle:
                case StartConditionTypes.SystemLowUse:
                case StartConditionTypes.SystemLowDiskUse:
                case StartConditionTypes.SystemLowMemoryUse:
                case StartConditionTypes.SystemLowCPUUse:
                #endregion

                #region Computer events
                case StartConditionTypes.SystemBoot:
                case StartConditionTypes.SystemWake:
                case StartConditionTypes.NetworkAccess:
                case StartConditionTypes.InternetAccess:
                case StartConditionTypes.OnNetwork:
                case StartConditionTypes.DriveAccess:
                case StartConditionTypes.FolderAccess:
                case StartConditionTypes.FTPAccess:
                #endregion

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
