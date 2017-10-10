using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulerLib
{
    public abstract class ScheulerInterface
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract void LoadInterface();
        public abstract void ShowMainForm();
        public abstract void ShowSettingsForm();
    }
}
