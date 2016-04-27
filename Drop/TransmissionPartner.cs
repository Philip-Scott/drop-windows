using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drop { 

    /// <summary>
    /// Represents a transmission partner you can connect to.
    /// </summary>
    public class TransmissionPartner {
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
