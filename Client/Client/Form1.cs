using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        private string filePath;
        private string ipUrl;
        private string portNumber;
        private int portNum;
        private string fileName;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileDialog fd = new OpenFileDialog();
            if(fd.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("File name: " + fd.FileName);
                label1.Text = "Filepath to upload: " + fd.FileName;
                filePath = fd.FileName;
                fileName = Path.GetFileName(filePath);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            ipUrl = textBox1.Text;
            portNumber = textBox2.Text;
            label1.Text = "Filepath to upload: " + filePath + "IP " + ipUrl + "PORT " + portNumber + "\nFile name to upload: " + fileName;
            
            if (ValidateIP(ipUrl))
            {
                label4.Text = "CORRECT IP";
                if(int.TryParse(portNumber, out portNum))
                {
                    label4.Text = "CORRECT IP AND PORT";
                    await UploadFileToServer(ipUrl, portNum, filePath);
                    await Progress();
                    label4.Text = UploadFile.currentStatus;  
                }
                else
                {
                    label4.Text = "CORRECT IP INCORRECT PORT";
                }
            }
            else
            {
                label4.Text = "INCORRECT IP";
            }

        }

        public bool ValidateIP(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

        async Task Progress()
        {
            var progress = new Progress<int>(value => progressBar1.Value = value);
            await Task.Run(() =>
            {
                for (int i = 0; i <= 100; i++)
                {
                    ((IProgress<int>)progress).Report(i);
                }
            });
        }

        async Task UploadFileToServer(string ipAdress, int portNumber, string filePath)
        {
            await Task.Run(() =>
            {
                UploadFile.SendFile(ipAdress, portNumber, filePath);
            }
            );
        }
    }

}
