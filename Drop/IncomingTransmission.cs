using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drop.lib
{
    class IncomingTransmission
    {
        public enum ServerState
        {
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

        //public event protocol_failed(string error_message);
        //public ev state_changed(ServerState state);
        //public void progress_changed(System.UInt64 bytes_received, System.UInt64 total_size);
        //public void file_received(uint id, string filename);

        private bool is_secure = false;
        private ServerState state = ServerState.AWAITING_INITIALISATION;

        private System.SByte client_version;
        private string client_name;
        private ArrayList file_requests = new ArrayList();
        public IncomingTransmission(Stream connection, string server_name, bool is_secure)
        {
           
        }

        public void reject_transmission()
        {
            
        }

        public void accept_transmission(System.UInt16[] ids)
        {
            
        }

        public void cancel()
        {
        }

        public bool get_is_secure()
        {
            return is_secure;
        }

        public ServerState get_state()
        {
            return state;
        }

        public System.SByte get_client_version()
        {
            return client_version;
        }

        public string get_client_name()
        {
            return client_name;
        }

        public FileRequest[] get_file_requests()
        {
            FileRequest[] fr;
            return fr;
        }

        private bool receive_initialisation()
        {
            return false;
        }

        private bool send_initialisation(string server_name)
        {
            return false;
        }

        private bool receive_request()
        {
            return false;
        }

        private void remember_accepted_ids(System.UInt16[] accepted_ids)
        {
            
        }

        private bool receive_data()
        {
            return false;
        }

        private void update_state(ServerState state)
        {

        }
    }
}
