using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Drop
{
    /// <summary>
    /// This class represents an outgoing transmission used to send files to another computer.
    /// </summary>
    public class OutgoingTransmission : ProtocolImplementation
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

        public OutgoingFileRequest[] file_requests;

        private bool is_secure;
        public ClientState state = ClientState.LOADING_FILES;
        private UInt16 server_version;
        private string server_name;

        public OutgoingTransmission () { }

        public OutgoingTransmission(Socket connection, string client_name, string[] files, bool is_secure)
        {
            // base construction
            this.is_secure = is_secure;
        }

        public int start_transmission(string hostname, string[] files) {
            if (!connect(hostname, 7432)) return 0;

            Thread.Sleep(100);
            //new Thread<int>(null, () => {
            if (!load_files(files))
                {
                    update_state(ClientState.FAILURE);

                    return 1;
                }
            Thread.Sleep(100);
                update_state(ClientState.SENDING_INITIALISATION);

                if (!send_initialisation("Windows User 1")) //TODO: Get client name
                {
                    //protocol_failed(_("Sending initialisation failed."));
                    update_state(ClientState.FAILURE);

                    return 2;
                }
            Thread.Sleep(100);
            update_state(ClientState.AWAITING_INITIALISATION);

                if (!receive_initialisation())
                {
                    //protocol_failed(_("Receiving initialisation failed."));
                    update_state(ClientState.FAILURE);

                    return 3;
                }

            Thread.Sleep(100);
            update_state(ClientState.SENDING_REQUEST);

                if (!send_request(this.file_requests))
                {
                    //protocol_failed(_("Sending request failed."));
                    update_state(ClientState.FAILURE);

                    return 4;
                }

            Thread.Sleep(100);
            update_state(ClientState.AWAITING_CONFIRMATION);

                if (!receive_confirmation())
                {
                    //protocol_failed(_("Receiving confirmation failed."));
                    update_state(ClientState.FAILURE);

                    return 5;
                }

                //if (state == ClientState.SENDING_DATA)
                {
                    if (!send_data())
                    {
                        //protocol_failed(_("Sending files failed."));
                        update_state(ClientState.FAILURE);

                        return 6;
                    }

                    update_state(ClientState.FINISHED);
                }

                return 7;
            //});

        }

        public void cancel()
        {
            Console.WriteLine("Protocol canceled.");

           // update_state(ClientState.CANCELED);
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

        private bool load_files (string[] files)
        {
            this.file_requests = new OutgoingFileRequest[files.Length];
            
            if (files.Length == 0) {
                Console.WriteLine("No files specified.");
                return false;
            }

            try {
                for(UInt16 i = 0; i < files.Length; i++) {
                    FileStream file = new FileStream(files[i], FileMode.Open);
                    file_requests[i] = new OutgoingFileRequest();
                    

                    file_requests[i].id = i;
                    file_requests[i].size = file.Length;
                    file_requests[i].name = Path.GetFileName(file.Name);
                    file_requests[i].filename = files[i];
                    file_requests[i].accepted = false;

                    file.Close();
                }
                return true; 
            } catch (Exception e)
            {
                Console.WriteLine("Can't load files: " + e.Message);

                return false;
            }
        }
        
        private bool send_initialisation(string client_name)
        {
            byte[] package = new byte[1 + client_name.Length];

            package[0] = 1; //Client Version

            byte[] client_bytes = System.Text.Encoding.ASCII.GetBytes(client_name);

            for (int i = 0; i < client_bytes.Length; i++) {
                package[i + 1] = client_bytes[i];
            }

            return send_package(package); 
        }

        private bool receive_initialisation()
        {
            byte[] package = receive_package(2);

            if (package == null)
            {
                return false;
            }

            server_version = package[0];
            package[0] = 0;

            server_name = System.Text.Encoding.ASCII.GetString(package);
        
            return true;// send_package(package);
        }

        private bool send_request(OutgoingFileRequest[] files)
        {
            for (UInt16 i = 0; i < files.Length; i++)
            {
                var file = files[i];
                byte[] file_name = System.Text.Encoding.ASCII.GetBytes(file.name);

                byte[] package = new byte[1 + 2 + 5 + file_name.Length];
                if (i + 1 == files.Length) package[0] = 1; else package[0] = 0;

                //ID
                byte[] id = BitConverter.GetBytes(i);
                package[1] = id[1];
                package[2] = id[0];

                //FileSize
                byte[] size = BitConverter.GetBytes(file.size);
                package[3] = size[4];
                package[4] = size[3];
                package[5] = size[2];
                package[6] = size[1];
                package[7] = size[0];

                //File Name
                //byte[] data = { 116,101,115,116,46,116,120,116};
                for (int k = 0; k < file_name.Length; k++) {
                    package[8 + k] = file_name[k];
                }

                if (send_package(package) == false) return false;
            }
            return true;
        }

        private bool receive_confirmation ()
        {
            byte[] received = receive_package(1);
           
            return received[0] == 1;
        }

        public bool send_data() {

            foreach (var file in file_requests) {
                byte[] id = new byte[2];
                byte[] request_id = BitConverter.GetBytes(file.id);
                id[0] = request_id[1];
                id[1] = request_id[0];
                
                send_package(id);
                Thread.Sleep(500);
                sendFile(file.filename);
            }

            return true;
        }

        public bool sendFile(string filename)
        {
            FileStream stream = File.OpenRead(filename);
            long file_size = stream.Length;

            while (file_size > 0)
            {
                int package_size = 16384;
                if (file_size < package_size) package_size = (int) file_size;

                byte[] fileBytes = new byte[package_size];
                stream.Read(fileBytes, 0, package_size);
                send_package(fileBytes);
                file_size = file_size-package_size;
            }

            return true;
        }

        private void update_state(ClientState state)
        {
            this.state = state;
            //state_changed(state);
        }
    }
}
