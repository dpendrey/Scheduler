using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulerLib
{
    /// <summary>
    /// Enumerates all the different types of start conditions for tasks
    /// </summary>
    public enum StartConditionTypes : int
    {
        #region No start condition
        /// <summary>
        /// Task will never (automatically) start
        /// </summary>
        None = 0x00000000,
        #endregion

        #region Timed start conditions
        /// <summary>
        /// Task will start at a predetermined time
        /// </summary>
        FixedTime = 0x00000101,
        /// <summary>
        /// Task will start after a predetermined time
        /// </summary>
        DelayedTime = 0x00000102,
        /// <summary>
        /// The task runs every minute
        /// </summary>
        MinuteSchedule = 0x00000103,
        /// <summary>
        /// The task runs every hour
        /// </summary>
        HourlySchedule = 0x00000104,
        /// <summary>
        /// The task runs daily
        /// </summary>
        DailySchedule = 0x00000105,
        /// <summary>
        /// The task runs weekly
        /// </summary>
        WeeklySchedule = 0x00000106,
        /// <summary>
        /// The task runs monthly
        /// </summary>
        MonthlySchedule = 0x00000107,
        /// <summary>
        /// The task runs yearly
        /// </summary>
        YearlySchedule = 0x00000108,
        /// <summary>
        /// The task runs on week days
        /// </summary>
        WeekdaySchedule = 0x00000109,
        /// <summary>
        /// The task runs on weekend days
        /// </summary>
        WeekendSchedule = 0x0000010A,
        #endregion
        #region Timed retry conditions
        MinuteRetry = 0x00000123,
        /// <summary>
        /// The task runs every hour
        /// </summary>
        HourlyRetry = 0x00000124,
        /// <summary>
        /// The task runs daily
        /// </summary>
        DailyRetry = 0x00000125,
        /// <summary>
        /// The task runs weekly
        /// </summary>
        WeeklyRetry = 0x00000126,
        /// <summary>
        /// The task runs monthly
        /// </summary>
        MonthlyRetry = 0x00000127,
        /// <summary>
        /// The task runs yearly
        /// </summary>
        YearlyRetry = 0x00000128,
        /// <summary>
        /// The task runs on week days
        /// </summary>
        WeekdayRetry = 0x00000129,
        /// <summary>
        /// The task runs on weekend days
        /// </summary>
        WeekendRetry = 0x0000012A,
        #endregion

        #region System resource conditions
        /// <summary>
        /// The task will run when the computer is idle
        /// </summary>
        SystemIdle = 0x00000201,
        /// <summary>
        /// The task will run when the computer usage is low (disk, memory, and CPU)
        /// </summary>
        SystemLowUse = 0x00000202,
        /// <summary>
        /// The task will run when the computer disk usage is low
        /// </summary>
        SystemLowDiskUse = 0x00000203,
        /// <summary>
        /// The task will run when the computer memory usage is low
        /// </summary>
        SystemLowMemoryUse = 0x00000204,
        /// <summary>
        /// The task will run when the computer CPU usage is low
        /// </summary>
        SystemLowCPUUse = 0x00000205,
        #endregion

        #region Computer events
        /// <summary>
        /// The task will run when the computer has booted up
        /// </summary>
        SystemBoot = 0x00000301,
        /// <summary>
        /// The task will run when the computer wakes from sleep/hybernation
        /// </summary>
        SystemWake = 0x00000302,
        /// <summary>
        /// The task will run when the computer has network access
        /// </summary>
        NetworkAccess = 0x00000303,
        /// <summary>
        /// The task will run when the computer has internet access
        /// </summary>
        InternetAccess = 0x00000304,
        /// <summary>
        /// The task will run when it is on a particular network
        /// </summary>
        OnNetwork = 0x00000305,
        /// <summary>
        /// The task will run when it has access to a particular drive
        /// </summary>
        DriveAccess = 0x00000306,
        /// <summary>
        /// The task will run when it has access to a particular folder
        /// </summary>
        FolderAccess = 0x00000307,
        /// <summary>
        /// The task will run when it has access to a particular FTP server
        /// </summary>
        FTPAccess = 0x00000308
        #endregion
    }
}
