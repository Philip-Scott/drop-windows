using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;


namespace Drop
{
    public class RegistryDrop {

        private RegistryKey _baseReg { get; set; }
        public RegistryKey baseReg
        {
            get
            {
                return this._baseReg;
            }

            set
            {
                this._baseReg = value;
            }
        }

        private RegistryKey _mainKey { get; set; }
        public RegistryKey mainKey
        {
            get
            {
                return this._mainKey;
            }

            set
            {
                this._mainKey = value;
            }
        }


        public RegistryDrop()
        {
            this._baseReg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            this.baseReg.CreateSubKey("Software\\Test");
            this._mainKey = Registry.ClassesRoot.CreateSubKey(@"*\shell\Drop to the");
            this._mainKey.SetValue("MUIVerb", "Drop to");
            this._mainKey.SetValue("Position", "Bottom");
        }

        public void deleteAllPartners()
        {
            this._baseReg.DeleteSubKey("Software\\Test");
            Registry.ClassesRoot.DeleteSubKey(@"*\shell\Drop to the");
        }

        public bool deletePartnerEntry(string hostName)
        {
            object stringKeySubcommands = this._mainKey.GetValue("SubCommands");
            try
            {
                if (stringKeySubcommands != null)
                {
                    this._mainKey.SetValue("SubCommands", stringKeySubcommands.ToString().Replace((";" + hostName), String.Empty));
                    this._baseReg.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\" + hostName);
                    this._baseReg.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\" + hostName + "\\command");
                }
         
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddPartnerEntry(string hostName)
        {
            object stringKeySubcommands=this._mainKey.GetValue("SubCommands");

            try
            {
                if (stringKeySubcommands != null)
                    this._mainKey.SetValue("SubCommands", stringKeySubcommands.ToString() + ";" + hostName);               
                else
                    this._mainKey.SetValue("SubCommands", hostName);
                this._baseReg.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\" + hostName).SetValue("", hostName);
                this._baseReg.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\" + hostName + "\\command").SetValue("", @"Notepad.exe");

                return true;
            }catch(Exception ex)
            {
                return false;
            }
           
        }

        public bool addAllParterns(List<TransmissionPartner> users)
        {
            try
            {
                users.ForEach(user => AddPartnerEntry(user.name));
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }

   



}
