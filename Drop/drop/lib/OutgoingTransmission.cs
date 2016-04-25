using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drop.lib
{
    /// <summary>
    /// This class represents an outgoing transmission used to send files to another computer.
    /// </summary>
    class OutgoingTransmission
    {
        /// <summary>
        /// The dbus-path of the current transmission.
        /// </summary>
        public string interface_path;

        /// <summary>
        /// Creates a new interface for the dbus-interface of an outgoing transmission.
        /// </summary>
        /// <param name="interface_path"></param>
        public OutgoingTransmission (string interface_path)
        {
            return ;
        }

        /// <summary>
        /// Lists the requested files for the transmission.
        /// </summary>
        /// <returns></returns>
        public List<OutgoingFileRequest> get_file_requests () 
        {
            return;
        }

        /// <summary>
        /// Cancels the transmission.
        /// </summary>
        public void cancel()
        {

        }

        /// <summary>
        /// Returns if the current connection is encrypted.
        /// </summary>
        /// <returns></returns>
        public bool get_is_secure()
        {
            return true;
        }

        /// <summary>
        /// Returns the current protocol state.
        /// </summary>
        /// <returns></returns>
        public void /*ClientState*/ get_state()
        {

        }

        /// <summary>
        /// Returns the used protocol version of the connected server.
        /// </summary>
        /// <returns></returns>
        public UInt16 get_server_version ()
        {
            return 1;
        }

        /// <summary>
        /// Returns the display name of the connected server.
        /// </summary>
        /// <returns></returns>
        public string get_server_name()
        {
            return "";
        }

        // Signals 
    }
}
