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

        public void Enact(Task Task)
        {
            switch (type)
            {
                #region Windows power options
                case ActionType.Hibernate:
                    System.Windows.Forms.Application.SetSuspendState(System.Windows.Forms.PowerState.Hibernate, true, true);
                    break;
                case ActionType.ShutDown:
                    {
                        System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("shutdown", "/s /t 0");
                        psi.CreateNoWindow = true;
                        psi.UseShellExecute = false;
                        System.Diagnostics.Process.Start(psi);
                    }
                    break;
                case ActionType.Sleep:
                    System.Windows.Forms.Application.SetSuspendState(System.Windows.Forms.PowerState.Hibernate, true, true);
                    break;
                case ActionType.Reboot:
                    {
                        System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("shutdown", "/r /t 0");
                        psi.CreateNoWindow = true;
                        psi.UseShellExecute = false;
                        System.Diagnostics.Process.Start(psi);
                    }
                    break;
                #endregion
                #region Execution
                case ActionType.ExecuteClass:
                    throw new NotImplementedException();
                case ActionType.RunProgram:
                    {
                        System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(runData[0], runData[1]);
                        psi.CreateNoWindow = true;
                        psi.UseShellExecute = false;
                        System.Diagnostics.Process.Start(psi);
                    }
                    break;
                #endregion
                #region Basics
                case ActionType.PlayVideo:
                case ActionType.PlaySong:
                    throw new NotImplementedException();
                case ActionType.ShowMessage:
                    System.Windows.Forms.MessageBox.Show(runData[0], runData[1], System.Windows.Forms.MessageBoxButtons.OK);
                    break;
                #endregion
                case ActionType.None:
                    break;
                default:
                    throw new Exception();
            }
        }
    }
}
