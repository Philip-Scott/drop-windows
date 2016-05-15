using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Security;
using System.ServiceProcess;

namespace Drop {
    public delegate void partner_added(Drop.TransmissionPartner partner);

    /// <summary>
    /// Creates a new Session.
    /// </summary>
    public class Session {
        public event partner_added new_partner_signal;

        //public event void new_incomming_transmission (IncomingTransmission incoming_transmission);
        //public event new_outgoing_transmission (IncomingTransmission incoming_transmission);
        //public event void transmission_partner_added (TransmissionPartner new_partner);
        //public event void transmission_partner_removed (string name);

        public Session() {
        }

        /// <summary>
        /// Returns a list of the available transmission partners.
        /// </summary>
        /// <returns></returns>
        public List<Drop.TransmissionPartner> get_transmission_partners () {
            var partner_list = new List<Drop.TransmissionPartner>();

            // TODO: Temp code. Needs to read from discovery service
            string[] persons = {"Felipe Escoto", "Arturo Hernandes", "Carlos Flores"};
            foreach (string person in persons) {
                var partner = new TransmissionPartner();
                partner.name = person;
                partner_list.Add(partner);
            }
            
            return partner_list;    
        }

        /*
        /// <summary>
        /// Returns a list of the running incoming transmissions.
        /// </summary>
        public List<Drop.IncomingTransmission> get_incomming_transmissions() {

        }
        */

        /*/// <summary>
        /// Returns a list of the running outgoing transmissions.
        /// </summary>
        /// <returns></returns>
        public List<Drop.OutgoingTransmission> get_outgoing_transmissions() {

        }*/

        /// <summary>
        /// Starts a new outgoing transmisson.
        /// </summary>
        /// <param name="hostname">The hostname or ip-address to connect to.</param>
        /// <param name="port">The port of the service.</param>
        /// <param name="files">A list of filenames that will be requested for transmission.</param>
        /// <param name="encrypt">Sets if the choosen port requires an encrypted connection.</param>
        public void start_transmission (string hostname, UInt16 port, string[] files, bool encrypt = false) {

        }

        /// <summary>
        /// Starts the partner discovery service
        /// </summary>
        public void start_discovery () {

        }
    }
}
