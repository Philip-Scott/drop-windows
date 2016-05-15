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

namespace Drop.Service
{
    partial class Service1 : ServiceBase
    {
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
            };

            browser.ServiceRemoved += delegate (object o, Mono.Zeroconf.ServiceBrowseEventArgs args)
            {
                var partner = new TransmissionPartner(args);
   
                bool removed = reg_edit.deletePartnerEntry(partner.name);
                Service1.write_log("Service disconnected: " + partner.name + " : " + removed);
            };
            
            service.Name = "Drop windows"; //TODO Change to computer's name
            service.RegType = "_drop._tcp";
            service.ReplyDomain = "local.";
            service.Port = 7431;

            Mono.Zeroconf.TxtRecord txt = new Mono.Zeroconf.TxtRecord();
            txt.Add("display-name", "Felipe (Windows)");
            txt.Add("server-enabled", "true");
            txt.Add("protocol-implementation", "windows");
            txt.Add("unencrypted-port", "7432");
            txt.Add("protocol-version", "1");

            service.TxtRecord = txt;

            service.Register();
            browser.Browse("_drop._tcp", "local");
        }

        private void test_protocol(TransmissionPartner partner) {
            var protocol_test = new ProtocolImplementation();
            IPHostEntry host_info = Dns.Resolve((partner.hostname));
            var addreces = host_info.AddressList;

            write_log("Addreces:" + addreces.Length);
            

            protocol_test.connect(addreces[0].ToString(), 7432);

            string raw_data = "1Windows User";
            
            protocol_test.send_package(System.Text.Encoding.ASCII.GetBytes(raw_data));

            write_log("testing...");
            byte[] data_received = protocol_test.receive_package(14);
            write_log("data received: " + data_received[0].ToString() + data_received[1].ToString() + data_received[2].ToString() + data_received[3].ToString() + data_received[4].ToString());
            
            protocol_test.end_connection();
        }

        private void add_partner(TransmissionPartner partner) {
            if (!partner.added && partner.ready)
            {
                partner.added = true;
                partners.Add(partner);

                bool added = reg_edit.AddPartnerEntry(partner.name);

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
            resolve_host("magali");
            reg_edit = new RegistryDrop();
            //reg_edit.deleteAllPartners();
            init_zeroconf();
            
            write_log("Initiated");
        }

        protected override void OnStop()
        {
            reg_edit.deleteAllPartners();
            write_log("Stoped");
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
