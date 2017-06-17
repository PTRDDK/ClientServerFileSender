using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class UploadFile
    {
        public static string currentStatus = "";

        public static void SendFile(string ipAdress, int portNumber, string filePath)
        {
            Console.WriteLine(ipAdress + portNumber + filePath);
            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress[] ipAddress = Dns.GetHostAddresses(ipAdress);
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress[0], portNumber);

                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                currentStatus = "Connecting to server...";
                client.Connect(ipEndPoint);

                //string fileName = Path.GetFileName(filePath);

                currentStatus = "Buffering file...";
                byte[] preBufName;
                using (var memoryStream = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(memoryStream))
                    {
                        string fileName = Path.GetFileName(filePath);
                        writer.Write(fileName);
                    }
                    preBufName = memoryStream.ToArray();
                }

                

                currentStatus = "Sending file...";
                client.SendFile(filePath, preBufName, null, TransmitFileOptions.UseDefaultWorkerThread);

                currentStatus = "Disconnecting...";
                client.Shutdown(SocketShutdown.Both);
                client.Close();

                currentStatus = "File transfered";
            }
            catch (System.Net.Sockets.SocketException e)
            {
                currentStatus = "No server on this port!";
            }
            catch (FileNotFoundException e)
            {
                currentStatus = "No file choosen!";
            }
            catch (ArgumentNullException e)
            {
                currentStatus = "No file choosen!";
            }
            catch (Exception e)
            {
                currentStatus = "ERROR!";
            }
        }
    }
}
