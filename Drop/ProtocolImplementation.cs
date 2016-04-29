using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace Drop {

    /// <summary>
    /// Represents a transmission partner you can connect to.
    /// </summary>
    public class ProtocolImplementation : Object
    {
        public ProtocolImplementation(string partner_ip) {
            string local_host = Dns.GetHostName();
            print_host_info(local_host);
            //ping_test("Testing", "10.0.2.15");
        }


        public static void print_host_info(string host) {
            IPHostEntry host_info = Dns.Resolve (host);

            Console.WriteLine("Host Name: " + host_info.HostName);

            foreach (IPAddress ip in host_info.AddressList) {
                Console.WriteLine("IP: " + ip.ToString() + " ");
            }
        }

        public static void ping_test(string text, string server_ip, int port = 2004) {
            //Convert input string into bytes:
            byte[] bytebuffer = Encoding.ASCII.GetBytes(text);

            TcpClient client = null;
            NetworkStream net_stream = null;

            try
            {
                client = new TcpClient(server_ip, port);
                Console.WriteLine("Connected to server... Sending echo");

                net_stream = client.GetStream();

                //Send the encoded string to the server
                net_stream.Write(bytebuffer, 0, bytebuffer.Length);

                Console.WriteLine("Sent {0} bytes to server...", bytebuffer.Length);
                
                int total_bytes_received = 0;   //Total received so far
                int bytes_gotten = 0;           //Bytes received on the last read


                bytes_gotten = net_stream.Read(bytebuffer, total_bytes_received, bytebuffer.Length - total_bytes_received);
                //while (total_bytes_received < bytebuffer.Length) {
                  
                Console.WriteLine("Gotten {0} bytes...", bytes_gotten);
                
                

                Console.WriteLine("Received {0} bytes to server...", bytebuffer.Length);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally {
                net_stream.Close();
                client.Close();
            }


        }
    }
}
