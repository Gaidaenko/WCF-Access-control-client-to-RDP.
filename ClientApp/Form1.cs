using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using ClientApp.Properties;

namespace ClientApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
            Run();                                                                                       
            textServer.Text = Settings.Default.Address;                                                  
        }

        public string SaveAddressSrv = Settings.Default.Address;                                         
        public string connection;                                                                                                                                       
        public string status;                                                                            
        public string addressSrv;                                                                        
        public int returnOff;                                                                                    
        public int port = 8000;                                                                         

        void SaveAddress()                                                                                       
        {
            if (checkBoxSaveMe.Checked)
            {
                Settings.Default.Address = textServer.Text;                                              
                Settings.Default.SaveMe = checkBoxSaveMe.Checked;                                        
                Settings.Default.Save();                                                                 
            }
            else
            {
                Settings.Default.Address = null;                                                         
                Settings.Default.SaveMe = false;                                                         
            }
        }
        private void button1_Click(object sender, EventArgs e)                                                 
        {
            addressSrv = Convert.ToString(textServer.Text);                                                     
            string connection = "http://" + addressSrv + ":" + port + "/MyService";                             

            try
            {
                Uri tcpUri = new Uri(connection);                                                                         
                EndpointAddress address = new EndpointAddress(tcpUri);                                         
                BasicHttpBinding binding = new BasicHttpBinding();
                ChannelFactory<ServiceApp> factory = new ChannelFactory<ServiceApp>(binding, address);                      
                ServiceApp service = factory.CreateChannel();                                                   

                returnOff = service.serviceOff();                                                                                                      
                status = service.currentStatus("");                                                             
                
                SaveAddress();                                                                                                           

                if (status == "StopPending" | status == "Stopped")                                              
                {
                    Color On = Color.Red;
                    label1.ForeColor = On;

                    label1.Text = "           Сервер отключен";
                    return;
                }
                if (status == "StartPending" | status == "Running")
                {
                    Color Off = Color.Green;
                    label1.ForeColor = Off;
                    label1.Text = "            Сервер включен";
                }                
                return;
            }
            catch
            {
                Color noAccess = Color.Red;
                label1.ForeColor = noAccess;
                label1.Text = "Сервер или служба не доступны.";
            }           
        }
        public async Task Run()                                                                             
        {          
            Uri tcpUri = new Uri("http://" + SaveAddressSrv + ":" + port + "/MyService");                                
            EndpointAddress address = new EndpointAddress(tcpUri);                                          
            BasicHttpBinding binding = new BasicHttpBinding();
            ChannelFactory<ServiceApp> factory = new ChannelFactory<ServiceApp>(binding, address);                        
            ServiceApp service = factory.CreateChannel();                                                   

            status = service.currentStatus("");                                                             

            if (status == "StopPending" | status == "Stopped")                                              
            {
                Color On = Color.Red;
                label1.ForeColor = On;
                
                label1.Text = "           Сервер отключен";
            }
            if (status == "StartPending" | status == "Running")
            {
                Color Off = Color.Green;
                label1.ForeColor = Off;
                label1.Text = "            Сервер включен";
            }            
            await Task.Delay(10000);
            Run();
        }

        [ServiceContract]                                                                                   
        public interface ServiceApp
        {
            [OperationContract]                                                                              
            int serviceOff(int status = 1);

            [OperationContract]
            string currentStatus(string currStatus);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
