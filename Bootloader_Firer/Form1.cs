using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using ThanhNTLibs.DeviceController.Communication.Serial;
namespace Bootloader_Firer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /* Declaring Global Variables */
        SerialPortManager serialPortManager = new SerialPortManager();
        DateTime time = DateTime.Now;
        BootLoaderStatus bootLoaderStatus;

        Thread SendPacketLengthThread;
        Thread SendDataThread;


        ulong lengthOfFile;
        ulong countSendData;

        byte[] lengthOfFileArr = new byte[3];
        byte[] NlengthOfFileArr = new byte[3];
        byte[] CRC32Arr = new byte[4];
        byte[] NCRC32Arr = new byte[4];
        byte[] PackitLengthOfFile = new byte[15];
        UInt32 CRC32Value;

        byte[] Data;

        #region Define Constan Value
        enum BootLoaderStatus 
        {
            SendPing = 0,
            SendLength ,
            SendData,
            Ending 
        }
        const byte ACK_VALUE = 0xff;
        const byte NACK_VALUE = 0x00;
        static bool ACK_Flag = false;

        #endregion

        #region CRC32 Calculate
        public static UInt32 calculateCRCWord(UInt32 crc, byte data)
        {
            int i;            
            crc = crc ^ data;

            for (i = 0; i < 32; i++)
            {
                if ((crc & 0x80000000) != 0)  
                {
                    crc = (crc << 1) ^ 0x04C11DB7; // Polynomial used in STM32
                }
                else
                {
                    crc = (crc << 1);
                }
            }

            return (crc);
        }

        public static UInt32 calculateCRCBuffer(UInt32 initial_value, byte[] buffer, int offset, int lengthCalculateCRC)
        {
            for (int i = 0; i < lengthCalculateCRC; i++)
            {
                initial_value = calculateCRCWord(initial_value, buffer[offset + i]);
            }

            return initial_value;
        }

        #endregion

        #region Brown File Button
        private void btnbrown_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {            
                time = DateTime.Now;
                txtstatus.Text += String.Format("\r\n{0}:- Open file [{1}]", time.ToLongTimeString(),openFileDialog1.SafeFileName);
            }
        }

        #endregion

        #region Fire Button 

        /***************************** Communication Protocol ***************************************************
            *
            * 				|-------------------- Packet Length of File (for erase flash )----------------------|
            *	Position: 	|   0	|     1    	|     2		|     3		|     4     |     5     |     6		 	|
            * 				|-----------------------------------------------------------------------------------|
            * 	Value:		| '*'	| NData[2] 	| NData[1]	| NData[0]	| !NData[2] | !NData[1]	| !NData[0]	 	|
            * 				|-----------------------------------------------------------------------------------|
            *	Position: 	|   7		|     8    	|     9  	|    10		|    11     |    12     |    13	 	|
            * 				|-----------------------------------------------------------------------------------|
            * 	Value:		| CRC32[3]	| CRC32[2] 	| CRC32[1]	| CRC32[0]	| !CRC32[3] | !CRC32[2]	| !CRC32[1]	|
            * 				|-----------------------------------------------------------------------------------|
            *	Position: 	|   14		|
            * 				|-----------|
            * 	Value:		| !CRC32[0]	|
            * 				|-----------|
            *
            *
            * 				|------------------ Packet Data (max data length = 2048 bytes) ---------------------|
            *	Position: 	|   0	|     1    	|     2		|     3		|     4     |     5     |      ...	 	|
            * 				|-----------------------------------------------------------------------------------|
            * 	Value:		| '!'	| NData[1] 	| NData[0]	| !NData[1]	| !NData[0] |  Data[0]	| 	   ...    	|
            * 				|-----------------------------------------------------------------------------------|
            *	Position: 	|   n+5		|   n+6    	|   n+7  	|   n+8		|   n+9     |   n+10    |   n+11 	|
            * 				|-----------------------------------------------------------------------------------|
            * 	Value:		| Data[n]	| CRC32[3] 	| CRC32[2]	| CRC32[1]	|  CRC32[0] | !CRC32[3]	| !CRC32[2]	|
            * 				|-----------------------------------------------------------------------------------|
            *	Position: 	|  n+12		|	n+13	|
            * 				|-----------------------|
            * 	Value:		| !CRC32[1]	| !CRC32[0]	|
            * 				|-----------|-----------|
            */

        private void btnFire_Click(object sender, EventArgs e)
        {
            if (serialPortManager.isConnected)
            {
                time = DateTime.Now;
                txtstatus.Text += String.Format("\r\n{0}:- Start program, waiting ACK from Boot Loader", time.ToLongTimeString());

                btnFire.Enabled = false;
                btnbrown.Enabled = false;
                countSendData = 0;
                progressBar1.Value = 0;

                SendPacketLengthThread = new Thread(SendPacketLength);
                SendDataThread = new Thread(SendData);

                /* Start Send Ping */
                bootLoaderStatus = BootLoaderStatus.SendPing;
                timerSendPing.Enabled = true;
            }
        }

        #endregion

        #region TxtFileName Double Click
        private void txtfileName_DoubleClick(object sender, EventArgs e)
        {
            txtfileName.SelectAll();
        }
        #endregion

        #region Form Load
        private void Form1_Load(object sender, EventArgs e)
        {

            serialPortManager.OnStatusChanged += SerialPortNotifyEventHandler_CallBack;
            serialPortManager.OnDataReceived += SerialPortDataEventHandler_CallBack;
  
            cbbbaud.SelectedIndex = 0;
            cbbdatalength.SelectedIndex = 0;
            cbbparity.SelectedIndex = 0;
            time = DateTime.Now;
            txtstatus.Text = string.Format("{0}:- Welcome to The Boot Loader Firer", time.ToLongTimeString()) ;
        }

        #endregion

        #region Timer Scan Serial Port
        private void timerScanSerialPort_Click(object sender, EventArgs e)
        {
            string[] port = SerialPort.GetPortNames();
            if(port.Length != cbbserialport.Items.Count)
            {
                cbbserialport.Items.Clear();
                cbbserialport.SelectedText = "";
                foreach(string _port in port)
                {
                    cbbserialport.Items.Add(_port);
                }
            }
        }
        #endregion

        #region Combobox SerialPort Selected Index Changed
        private void cbbserialport_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                serialPortManager.SetComPortManual(cbbserialport.SelectedItem.ToString());
                serialPortManager.SetComSpeedManual(1000000);
                serialPortManager.Open();
                btnFire.Enabled = true;
            }
            catch 
            {
                btnFire.Enabled = false;
                MessageBox.Show("Can't open the COM port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Form Closing
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPortManager.Close();
            
        }
        #endregion

        #region Serial Port Manager 

        public void SerialPortNotifyEventHandler_CallBack(object sender, string msg)
        {
            time = DateTime.Now;
            txtstatus.Invoke(new MethodInvoker(delegate ()
            {
                txtstatus.Text += string.Format("\r\n{0}:- " + msg, time.ToLongTimeString());
            }));           
        }

        public void SerialPortDataEventHandler_CallBack(object sender, byte[] data)
        {
            /* Receive ACK-PING from MCU */
            if((data[0] == ACK_VALUE) && (bootLoaderStatus == BootLoaderStatus.SendPing))
            {
                timerSendPing.Enabled = false;
                bootLoaderStatus = BootLoaderStatus.SendLength;

                time = DateTime.Now;
                txtstatus.Invoke(new MethodInvoker(delegate ()
                {              
                    txtstatus.Text += String.Format("\r\n{0}:- Receive ACK-PING From MCU", time.ToLongTimeString());
                }));
               
                Thread.Sleep(400);//Critical code

                SendPacketLengthThread.Start();               
            }
            /* Receive ACK-DATA-LENGTH from MCU */
            else if ((data[0] == ACK_VALUE) && (bootLoaderStatus == BootLoaderStatus.SendLength))
            {
                bootLoaderStatus = BootLoaderStatus.SendData;
                            
                txtstatus.Invoke(new MethodInvoker(delegate ()
                {
                    time = DateTime.Now;
                    txtstatus.Text += String.Format("\r\n{0}:- Receive ACK-DATA-LENGTH From MCU", time.ToLongTimeString());
                }));
              
                SendDataThread.Start();
            }
            /* Receive NACK-DATA-LENGTH from MCU */
            else if ((data[0] == NACK_VALUE) && (bootLoaderStatus == BootLoaderStatus.SendLength))
            {
                txtstatus.Invoke(new MethodInvoker(delegate ()
                {
                    time = DateTime.Now;
                    txtstatus.Text += String.Format("\r\n{0}:- Receive NOT-ACK-DATA-LENGTH From MCU", time.ToLongTimeString());
                }));

                if(!SendPacketLengthThread.IsAlive)
                {
                    SendPacketLengthThread = new Thread(SendPacketLength);
                    SendPacketLengthThread.Start();
                }
            }
            /* Receive ACK-DATA from MCU */
            else if ((data[0] == ACK_VALUE) && (bootLoaderStatus == BootLoaderStatus.SendData))
            {
                time = DateTime.Now;
                progressBar1.Invoke(new MethodInvoker(delegate ()
                {
                    progressBar1.Value = Convert.ToInt32(((float)countSendData / (float)lengthOfFile)*100);
                }));

                if (countSendData == lengthOfFile)
                {
                    bootLoaderStatus = BootLoaderStatus.Ending;

                    txtstatus.Invoke(new MethodInvoker(delegate ()
                    {
                        txtstatus.Text += String.Format("\r\n{0}:- Program Success", time.ToLongTimeString());
                    }));

                    btnFire.Invoke(new MethodInvoker(delegate ()
                    {
                        btnFire.Enabled = true;
                    }));

                    btnbrown.Invoke(new MethodInvoker(delegate ()
                    {
                        btnbrown.Enabled = true;
                    }));                    
                }
                else
                {
                    txtstatus.Invoke(new MethodInvoker(delegate ()
                    {
                        txtstatus.Text += String.Format("\r\n{0}:- Receive ACK-DATA From MCU", time.ToLongTimeString());
                    }));
              
                    if (!SendDataThread.IsAlive)
                    {
                        SendDataThread = new Thread(SendData);
                        SendDataThread.Start();
                    }
                }
            }
        }

        #endregion

        #region Send PacketLength Thread
        public void SendPacketLength()
        {
            if(bootLoaderStatus == BootLoaderStatus.SendLength)
            {
                lengthOfFileArr = BitConverter.GetBytes(lengthOfFile);
                NlengthOfFileArr = BitConverter.GetBytes(lengthOfFile ^ 0xffffff); //3 Bytes

                PackitLengthOfFile[0] = Convert.ToByte('*');
                PackitLengthOfFile[1] = lengthOfFileArr[2]; //MSB of lengthOfFile
                PackitLengthOfFile[2] = lengthOfFileArr[1];
                PackitLengthOfFile[3] = lengthOfFileArr[0];

                PackitLengthOfFile[4] = NlengthOfFileArr[2]; //MSB of NlengthOfFileArr
                PackitLengthOfFile[5] = NlengthOfFileArr[1];
                PackitLengthOfFile[6] = NlengthOfFileArr[0];

                CRC32Value = calculateCRCBuffer(0xFFFFFFFF, PackitLengthOfFile,1,6);

                CRC32Arr = BitConverter.GetBytes(CRC32Value);
                NCRC32Arr = BitConverter.GetBytes(CRC32Value ^ 0xffffffff); //4Bytes

                PackitLengthOfFile[7] = CRC32Arr[3]; //MSB of CRC32Arr
                PackitLengthOfFile[8] = CRC32Arr[2];
                PackitLengthOfFile[9] = CRC32Arr[1];
                PackitLengthOfFile[10] = CRC32Arr[0];

                PackitLengthOfFile[11] = NCRC32Arr[3];//MSB of NCRC32Arr
                PackitLengthOfFile[12] = NCRC32Arr[2];
                PackitLengthOfFile[13] = NCRC32Arr[1];
                PackitLengthOfFile[14] = NCRC32Arr[0];

                time = DateTime.Now;
                txtstatus.Invoke(new MethodInvoker(delegate ()
                {
                    txtstatus.Text += String.Format("\r\n{0}:- Send Packet Length {1} bytes", time.ToLongTimeString(), lengthOfFile);
                }));

                serialPortManager.SendMessage(PackitLengthOfFile);
            }            
        }
        #endregion

        #region Send Data Thread
        public void SendData()
        {
            if(bootLoaderStatus == BootLoaderStatus.SendData)
            {
                byte[] packetData = new byte[2048 + 13]; //Max data length = 2048, header length = 13
                ulong lengthData = 0;
                byte[] lengthDataArr = new byte[2];
                byte[] NlengthDataArr = new byte[2];

                if (lengthOfFile >= 2048)
                {
                    if ((countSendData + 2048) <= lengthOfFile)
                    {
                        lengthData = 2048;                        
                    }
                    else
                    {
                        lengthData = (ulong)(lengthOfFile - countSendData);
                    }                    
                }
                else
                {
                    MessageBox.Show("Length of file too short !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SendDataThread.Abort();
                }

                lengthDataArr = BitConverter.GetBytes(lengthData);
                NlengthDataArr = BitConverter.GetBytes(lengthData ^ 0xffff); //2 bytes

   
                packetData[0] = Convert.ToByte('!');
                packetData[1] = lengthDataArr[1];   //MSB length Data
                packetData[2] = lengthDataArr[0];

                packetData[3] = NlengthDataArr[1];   //MSB Nlength Data
                packetData[4] = NlengthDataArr[0];

                Buffer.BlockCopy(Data,(int) countSendData, packetData, 5,(int) lengthData);

                CRC32Value = calculateCRCBuffer(0xFFFFFFFF, packetData, 1,(int)lengthData + 4);

                CRC32Arr = BitConverter.GetBytes(CRC32Value);
                NCRC32Arr = BitConverter.GetBytes(CRC32Value ^ 0xffffffff); //4Bytes

                packetData[5 + lengthData + 0] = CRC32Arr[3];
                packetData[5 + lengthData + 1] = CRC32Arr[2];
                packetData[5 + lengthData + 2] = CRC32Arr[1];
                packetData[5 + lengthData + 3] = CRC32Arr[0];

                packetData[5 + lengthData + 4] = NCRC32Arr[3];
                packetData[5 + lengthData + 5] = NCRC32Arr[2];
                packetData[5 + lengthData + 6] = NCRC32Arr[1];
                packetData[5 + lengthData + 7] = NCRC32Arr[0];

                countSendData = countSendData + lengthData;

                time = DateTime.Now;
                txtstatus.Invoke(new MethodInvoker(delegate ()
                {
                    txtstatus.Text += String.Format("\r\n{0}:- Send Data {1} bytes", time.ToLongTimeString(), lengthData);
                }));

                serialPortManager.SendMessage(packetData);

            }
        }
        #endregion

        #region Timer Send Ping
        private void timerSendPing_Tick(object sender, EventArgs e)
        {
            /* Send Ping Value Every 50ms Ultil receive ACK Value */
            if(bootLoaderStatus == BootLoaderStatus.SendPing)
            {
                byte[] SendData = new byte[] { ACK_VALUE };
                serialPortManager.SendMessage(SendData);
            }
        }
        #endregion

        #region Open File Dialog OK
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                txtfileName.Text = openFileDialog1.FileName;
                FileStream fs = new FileStream(txtfileName.Text, FileMode.Open, FileAccess.Read);
                lengthOfFile = (ulong)(fs.Length);
                Data = new byte[fs.Length];
                fs.Read(Data, 0, Convert.ToInt32(fs.Length));
                fs.Close();
            }
            catch(Exception ee)
            {
                MessageBox.Show(String.Format("Can't open the *.bin file\r\nDetails: {0}",ee), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion


    }
}
