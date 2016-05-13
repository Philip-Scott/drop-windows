using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drop.lib
{
    class IncomingTransmission
    {
        public enum ServerState {
            AWAITING_INITIALISATION,
            SENDING_INITIALISATION,
            AWAITING_REQUEST,
            NEEDS_CONFIRMATION,
            SENDING_CONFIRMATION,
            REJECTED,
            RECEIVING_DATA,
            FINISHED,
            CANCELED,
            FAILURE
        }

        public struct FileRequest
        {
            System.UInt16 id;
            System.UInt16 size;
            string name;
            bool accepted;
        }

        public void protocol_failed(string error_message);
        public void state_changed(ServerState state);
        public void progress_changed(System.UInt64 bytes_received, System.UInt64 total_size);
        public void file_received(uint id, string filename);

        private bool is_secure = false;
        private ServerState state = ServerState.AWAITING_INITIALISATION;

        private System.SByte client_version;
        private string client_name;
        private Gee.HashMap< System.UInt16, FileRequest?> file_requests;



    }
}
