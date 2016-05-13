using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.IO;
using System.Timers;
using System.Net;
using System.Threading;

namespace Drop.Service
{
    partial class Service1 : ServiceBase
    {
        public static string client_name;

        private List<TransmissionPartner> partners = new List<TransmissionPartner>();

        private RegistryDrop reg_edit;
        private Mono.Zeroconf.ServiceBrowser browser;
        private Mono.Zeroconf.RegisterService service;

        public Service1() {
            InitializeComponent();
        }

        public void init_zeroconf()
        {
            service = new Mono.Zeroconf.RegisterService();
            browser = new Mono.Zeroconf.ServiceBrowser();

            browser.ServiceAdded += delegate (object o, Mono.Zeroconf.ServiceBrowseEventArgs args)
            {
                var partner = new TransmissionPartner (args);
                partner.partner_ready += delegate (TransmissionPartner ready)
                {
                    write_log("Partner added: " + ready.hostname);
                    this.add_partner(ready);
                };

                partner.resolve();
                partners.Add(partner);
                //test_protocol(partner);
            };

            browser.ServiceRemoved += delegate (object o, Mono.Zeroconf.ServiceBrowseEventArgs args)
            {
                var partner = new TransmissionPartner(args);
   
                bool removed = reg_edit.deletePartnerEntry(partner.name);
                Service1.write_log("Service disconnected: " + partner.name + " : " + removed);
            };

            service.Name = System.Environment.MachineName;
            service.RegType = "_drop._tcp";
            service.ReplyDomain = "local.";
            service.Port = 7431;

            Mono.Zeroconf.TxtRecord txt = new Mono.Zeroconf.TxtRecord();
            txt.Add("display-name", client_name);
            txt.Add("server-enabled", "true");
            txt.Add("protocol-implementation", "windows");
            txt.Add("unencrypted-port", "7432");
            txt.Add("protocol-version", "1");

            service.TxtRecord = txt;

            service.Register();
            
            browser.Browse("_drop._tcp", "local");

            long size = 54; 
            write_log ("Long length: " + BitConverter.GetBytes(size).Length);

            //test_protocol();
        }

        private void test_protocol() {
            var outgoing = new Drop.OutgoingTransmission();
            string[] files = { @"C:\Users\felipe\Downloads\BonjourPSSetup.exe" , @"C:\Users\felipe\Desktop\step.jpg", @"C:\Users\felipe\test.txt" };

            var error = outgoing.start_transmission("magali.local",  files);

            write_log("Transmit util: " + error + " : " + outgoing.file_requests[0].name);


            return;
            /*var protocol_test = new ProtocolImplementation();
            
            if (!protocol_test.connect("magali.local", 7432)) return;

            string raw_data = "1Windows User";
            protocol_test.send_package(System.Text.Encoding.ASCII.GetBytes(raw_data));

            byte[] data_received = protocol_test.receive_package(2);

            string data = "";
            foreach (byte by in data_received) {
                data = data + char.ConvertFromUtf32(by);
            }
            write_log("Inicialization: " + data);

            Thread.Sleep(2000);
            byte[] init_data = {1, 0, 0, 0, 0, 0, 0, 29, };
            
            protocol_test.send_package(init_data);

            Thread.Sleep(2000);
            byte[] received = protocol_test.receive_package(2);
            data = "";
            foreach (byte by in received)
            {
                data = data + by;
            }
            write_log("Files Confirmation: " + data);

            Thread.Sleep(2000);

            FileStream stream = File.OpenRead(@"C:\Users\felipe\test.txt");
            byte[] fileBytes = new byte[stream.Length];

            var value = stream.Read(fileBytes, 0, fileBytes.Length);
            stream.Close();


            byte[] array = { 0, 0 };
            protocol_test.send_package(array);
            Thread.Sleep(500);
            protocol_test.send_package(fileBytes);
            Thread.Sleep(2000);
            protocol_test.end_connection(); */
        }

        private void add_partner(TransmissionPartner partner) {
            if (!partner.added && partner.ready)
            {
                partner.added = true;
                partners.Add(partner);

                bool added = reg_edit.AddPartnerEntry(partner.name, partner.hostname);

                string to_write = "Service found: " + partner.name + " : " + added;
                Service1.write_log(to_write);
            }
        }

        public static void resolve_host(string host) {
            write_log("Trying to resolve " + host);
            try
            {
                IPAddress[] addresses = Dns.GetHostAddresses(host);
                write_log("Addresses: " + addresses.Length);
                foreach (IPAddress ip in addresses)
                {
                    write_log("IP: " + ip.ToString() + " ");
                }

            }
            catch (Exception e){ 
                write_log("No ip found: " + e.Message);
            }
        }

        public static void write_log(string message)
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
            client_name = "Windows User";

            resolve_host("magali");
            reg_edit = new RegistryDrop();

            reg_edit.init();
            reg_edit.deleteAllPartners();
            reg_edit.init();

            init_zeroconf();
            write_log("Initiated");
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
            OnStop();
        }

        protected override void OnStop()
        {
            reg_edit.deleteAllPartners();
            write_log("Stoped");
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
