using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulerLib
{
    public class Action
    {
        private ActionType type;
        private string[] runData;

        public ActionType ActionType { get { return type; } }
        public string[] RunData { get { return runData; } set { runData = value; } }

        internal void ReadData(string Data)
        {
            ReadData(Data, Data.ToUpper());
        }
        internal void ReadData(string Data, string UpperData)
        {
            if (UpperData.StartsWith("TYPE="))
                type = (ActionType)int.Parse(Data.Substring("TYPE=".Length));
            if (UpperData.StartsWith("RUNDATA="))
                runData = Data.Substring("RUNDATA=".Length).Split(new char[] { ',' });
        }

        internal void WriteData(System.IO.StreamWriter Writer, int Index)
        {
            Writer.WriteLine("Action." + Index.ToString() + ".Type=" + ((int)type).ToString());
            Writer.Write("Action." + Index.ToString() + ".RunData=");
            for (int i = 0; i < runData.Length; i++)
            {
                Writer.Write(runData[i]);
                if (i > 0)
                    Writer.Write(",");
            }
        }
    }
}
