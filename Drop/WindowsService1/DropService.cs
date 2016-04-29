using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.IO;
using System.Timers;

namespace Drop.Service
{
    partial class Service1 : ServiceBase
    {
        private Timer timer = null;

        private RegistryDrop reg_edit;
        private Mono.Zeroconf.ServiceBrowser browser;
        private Mono.Zeroconf.RegisterService service;

        public Service1()
        {
            InitializeComponent();
        }

        public void init_zeroconf()
        {
            browser = new Mono.Zeroconf.ServiceBrowser();

            browser.ServiceAdded += delegate (object o, Mono.Zeroconf.ServiceBrowseEventArgs args)
            {
                bool added = reg_edit.AddPartnerEntry(args.Service.Name);
                string to_write = "Service found: " + args.Service.Name + " : " + added;
                Service1.WirteLog(to_write);
            };

            browser.ServiceRemoved += delegate (object o, Mono.Zeroconf.ServiceBrowseEventArgs args)
            {
                bool removed = reg_edit.deletePartnerEntry(args.Service.Name);
                Service1.WirteLog("Service disconnected: " + args.Service.Name + " : " + removed);
            };
            browser.Browse("_drop._tcp", "local");

            service = new Mono.Zeroconf.RegisterService();

            service.Name = "Drop windows";
            service.RegType = "_drop._tcp";
            service.ReplyDomain = "local.";
            service.Port = 7431;

            Mono.Zeroconf.TxtRecord txt = new Mono.Zeroconf.TxtRecord();
            txt.Add("display-name", "Windows");
            txt.Add("server-enabled", "true");
            txt.Add("protocol-implementation", "windows");
            txt.Add("unencrypted-port", "7432");
            txt.Add("protocol-version", "1");

            service.TxtRecord = txt;

            service.Register();

            var protocol_test = new ProtocolImplementation("192.168.100.120");
        }

        public static void WirteLog(string message)
        {
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + " " + message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
                
            }
        }

        protected override void OnStart(string[] args)
        {
            reg_edit = new RegistryDrop();
            reg_edit.deleteAllPartners();
            init_zeroconf();
            WirteLog("Initiated");
        }

        protected override void OnStop()
        {
            reg_edit.deleteAllPartners();
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
