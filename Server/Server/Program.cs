using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var tcpListener = new TcpListener(IPAddress.Loopback, 13000);
            tcpListener.Start();
            while (true)
            {
                Console.WriteLine("Server is running..");
                using (var client = tcpListener.AcceptTcpClient())
                using (var stream = client.GetStream())
                using (MemoryStream memStream = new MemoryStream())
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    Console.WriteLine("Connected. Receiving data..");

                    var fileName = reader.ReadString();
                    Console.WriteLine("FileName from client:" + fileName);

                    int i = 0;
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                    string fileExtension = Path.GetExtension(fileName);
                    while (File.Exists("F:/result/" + fileName))
                    {
                        fileName = fileNameWithoutExt + i + fileExtension;
                        i++;
                    }

                    // read the file in chunks of 1KB
                    var buffer = new byte[1024];
                    int bytesRead;
                    using (var output = File.Create("F:/result/" + fileName))
                    {
                        while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            output.Write(buffer, 0, bytesRead);
                        }
                        Console.WriteLine("File saved as:" + fileName);
                    }
                }

                Console.WriteLine("\nWaiting for next connection...");
            }
        }

        
    }
}
