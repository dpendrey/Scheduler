using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scheduler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            SchedulerLib.Engine.LoadTasks();
            SchedulerLib.Engine.StartScheduling();

            SchedulerLib.StartCondition[] conditions = new SchedulerLib.StartCondition[2];
            conditions[0] = new SchedulerLib.StartCondition(SchedulerLib.StartConditionTypes.MinuteSchedule, 1);
            conditions[1] = new SchedulerLib.StartCondition(SchedulerLib.StartConditionTypes.FolderAccess, @"\\mediaserver\DataStorage\Backups");

            SchedulerLib.Action[] actions = new SchedulerLib.Action[1];
            actions[0] = new SchedulerLib.Action(SchedulerLib.ActionType.ShowMessage, new string[] { "Yay!","This message thing is working" });

            SchedulerLib.Engine.AddTask(new SchedulerLib.SimpleTask("Test Task", conditions,actions));
        }
    }
}
