using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using drop.Desktop.Models;

namespace drop.Application
{
    public class RegistryDrop
    {

        public RegistryDrop()
        {

        }

        public bool addContextMenuHandler(List<User> users)
        {
            var baseReg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            var reg = baseReg.CreateSubKey("Software\\Test");
            RegistryKey key;
            RegistryKey command;
            string formattedUsers;
            List<string> usersTemp = new List<string>();

            try
            {
                key = Registry.ClassesRoot.CreateSubKey(@"*\shell\Drop to the");
                key.SetValue("MUIVerb", "Drop to");
                key.SetValue("Position", "Bottom");

                users.ForEach(delegate (User user)
                {
                    usersTemp.Add(user.name);
                });

                formattedUsers = string.Join(";|;",usersTemp.ToArray());
                key.SetValue("SubCommands", formattedUsers);

                users.ForEach(delegate (User user)
                {
                    command = baseReg.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\"+user.name);
                    command.SetValue("", user.name);
                    command = baseReg.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\"+user.name+"\\command");
                    command.SetValue("", @"Notepad.exe");

                });
                return true;
            }
            catch(Exception ex)
            {
                Utils.log.Error("An error has ocurred: " + ex.Message.ToString());
                return false;
            }
          

        }



    }

   



}
