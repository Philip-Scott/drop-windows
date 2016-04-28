using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace Drop
{
    /// <summary>
    /// This class represents an outgoing transmission used to send files to another computer.
    /// </summary>
    class OutgoingTransmission /*: ProtocolImplementation*/
    {
        public enum ClientState : byte {
            LOADING_FILES,
            SENDING_INITIALISATION,
            AWAITING_INITIALISATION,
            SENDING_REQUEST,
            AWAITING_CONFIRMATION,
            REJECTED,
            SENDING_DATA,
            FINISHED,
            CANCELED,
            FAILURE
        };

        public class FileRequest
        {
            UInt16 id;
            UInt64 size;
            string name;
            string filename;
            bool accepted;
        }

        public Dictionary<UInt16, FileRequest> file_requests;

        private bool is_secure;
        public ClientState state = ClientState.LOADING_FILES;
        private UInt16 server_version;
        private string server_name;

        public OutgoingTransmission(Socket connection, string client_name, string[] files, bool is_secure)
        {
            // base construction
            this.is_secure = is_secure;


        }

        public void cancel()
        {
            Console.WriteLine("Protocol canceled.");

            // cancellable

            //update_state(ClientState.CANCELED);
        }

        public bool get_is_secure ()
        {
            return is_secure;
        }

        public ClientState get_state()
        {
            return state;
        }

        public UInt16 get_server_version()
        {
            return server_version;
        }

        public string get_server_name ()
        {
            
            return server_name;
        }

        public FileRequest[] get_file_requests ()
        {
            FileRequest[] requests = new FileRequest[file_requests.Count];
            int i = 0;
            foreach(var item in file_requests)
            {
                requests[i++] = item.Value ;
            }

            return requests;
        }

        private bool load_files (string[] files)
        {
            file_requests = new Dictionary<UInt16, FileRequest>();

            if (files.Length == 0)
            {
                Console.WriteLine("No files specified.");
                return false;
            }

            try
            {
                for(int i = 0; i < files.Length; i++)
                {
                    FileStream file = new FileStream(files[i], FileMode.Open);

                    if (file == null)
                    {
                        Console.WriteLine("File " + files[i] + " doesn't exist.");

                        return false;
                    }

                    // Read the file
                    Console.WriteLine("File loaded");
                    return true;
                }
                return true; // NOT SURE
            } catch (Exception e)
            {
                Console.WriteLine("Can't load files: " + e.Message);

                return false;
            }
        }

        private bool recieve_initialisation ()
        {
            UInt16[] package = receive_package(2);

            if (package == null)
            {
                return false;
            }

            return send_package(package);
        }
    }

}
