using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace Drop
{

    /// <summary>
    /// Represents a transmission partner you can connect to.
    /// </summary>
    public class ProtocolImplementation : Object
    {
        private NetworkStream input_stream;
        private NetworkStream output_stream;

        protected const UInt16 MAX_PACKAGE_LENGTH = 16384;

        public ProtocolImplementation() { }

        public ProtocolImplementation(NetworkStream input_stream, NetworkStream output_stream)
        {
            this.input_stream = input_stream;
            this.output_stream = output_stream;
        }

        public void connect(string server_ip, int port = 2004)
        {
            var client = new TcpClient(server_ip, port);
            input_stream = client.GetStream();
            output_stream = client.GetStream();
        }
        public async void send_package(byte[] data)
        {
            try
            {
                var package_length = (UInt16)data.Length;

                if (package_length > MAX_PACKAGE_LENGTH)
                    return;

                var header = new byte[2];

                header[0] = (byte)((package_length >> 8) & 0xff);
                header[1] = (byte)(package_length & 0xff);

                await output_stream.WriteAsync(header, 0, 2);
                await output_stream.WriteAsync(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                log(ex.Message);
            }
        }

        public byte[] receive_package(UInt16 expected_min_length = 1)
        {
            try
            {
                int header_length;
                var header = new byte[2];

                input_stream.Read(header, 0, 2);

                int expected_package_length = (header[0] << 8) + (header[1]);

                if (expected_package_length < expected_min_length)
                {
                    //return null;
                }

                var package = new byte[expected_package_length];

                input_stream.Read(package, 0, expected_package_length);

                if (package.Length != expected_package_length)
                {
                    return null;
                }

                return package;
            }
            catch (Exception ex)
            {
                log("Receiving package failed.");
                return null;
            }
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public void end_connection()
        {
            input_stream.Close();
            output_stream.Close();
        }

        private void log(String message)
        {
        }

    }
}
