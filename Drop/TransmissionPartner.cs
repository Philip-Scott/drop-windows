using Mono.Zeroconf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drop {

    public delegate void PartnerReady(TransmissionPartner partner);

    /// <summary>
    /// Represents a transmission partner you can connect to.
    /// </summary>
    public class TransmissionPartner : Object{
        private Mono.Zeroconf.ServiceBrowseEventArgs partner;

        public event PartnerReady partner_ready;
        public bool ready = false;
        public bool added = false;

        public TransmissionPartner() { }

        /// <summary>
        /// Forms a Transmission Partner from a TxtRecord
        /// </summary>
        /// <param name="record"></param>
        public TransmissionPartner(Mono.Zeroconf.ServiceBrowseEventArgs partner_) {
            this.partner = partner_;
            
            this.partner.Service.Resolved += delegate (object o, ServiceResolvedEventArgs handeler) {
                this.populate_partner(handeler.Service.TxtRecord);
            };
        }

        public void resolve ()
        {
            this.partner.Service.Resolve();
        }

        private void populate_partner(ITxtRecord record) {
            
              for (int n = 0; n < record.Count; n++) {
                 var item = record.GetItemAt(n);
                 switch (item.Key) {
                     case "display-name": this.name = item.ValueString; break;
                     case "server-enabled": this.server_enabled = item.ValueString == "true"; break;
                     case "protocol-implementation": this.procotol_implementation = item.ValueString; break;
                     case "unencrypted-port": this.unencrypeted_port = UInt16.Parse(item.ValueString); break;
                     case "protocol-version": this.protocol_implementation = int.Parse(item.ValueString); break;
                 }

                //hostname = this.partner.Service.;
                //hostname = this.partner.Service.
            }

            ready = true;

            if (partner_ready != null) {
                partner_ready(this);
            }
        }

        /// <summary>
        /// The name of the transmission partner, acting as an ID.
        /// </summary>
        public string name;

        /// <summary>
        /// The hostname of the computer.
        /// </summary>
        public string hostname;

        /// <summary>
        /// The port of the service. This port requires a TLS handshake.
        /// </summary>
        public UInt16 port;

        /// <summary>
        /// The unencrypted port of the service. This port doesn't allow TLS, make sure that the user does not send critical files using this port.
        /// </summary>
        public UInt16 unencrypeted_port;

        /// <summary>
        /// The version of the protocol the server uses.
        /// </summary>
        public int protocol_implementation;

        /// <summary>
        /// The name of the server's protocol implementation.
        /// </summary>
        public string procotol_implementation;

        /// <summary>
        /// The server-name that should be displayed to the user.
        /// </summary>
        public string display_name;

        /// <summary>
        /// If true, the server is running and accepts transmissions.
        /// </summary>
        public bool server_enabled;
    }
}
