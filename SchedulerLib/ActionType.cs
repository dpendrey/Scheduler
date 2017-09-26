using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulerLib
{
    public enum ActionType : int
    {
        #region No action
        None = 0x00000000,
        #endregion

        #region Windows power options
        ShutDown = 0x00000101,
        Reboot = 0x00000102,
        Sleep = 0x00000103,
        Hibernate = 0x00000104,
        #endregion

        #region Execution
        RunProgram = 0x00000201,
        ExecuteClass = 0x00000202
        #endregion
    }
}
