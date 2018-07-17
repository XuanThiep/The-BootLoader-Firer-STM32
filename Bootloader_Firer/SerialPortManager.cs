using System;
using System.IO.Ports;
using System.Threading;

namespace ThanhNTLibs.DeviceController.Communication.Serial
{
    public sealed class SerialPortManager
    {
        /*****************************************************************
         * 
         * DEFINE VARIABLES, BUFFER....
         * 
         * ***************************************************************/
        //private static readonly Lazy<SerialPortManager> lazy = new Lazy<SerialPortManager>(() => new SerialPortManager());
        //public static SerialPortManager Instance { get { return lazy.Value; } }

        private SerialPort serialPort = new SerialPort();
        private string serialPortName { get; set; }
        private int serialBaudrate { get; set; }

        private Thread sendThread = null;
        private volatile bool keepSending = false;

        private SerialPortBuffer sendMsgQueue = new SerialPortBuffer();
        private ManualResetEvent sendMsgEvent = new ManualResetEvent(false);


        /*****************************************************************
         * 
         * CONFIG UART DRIVER
         * 
         * ***************************************************************/
        /// <summary>
        /// Default config
        /// </summary>
        public SerialPortManager()
        {
            serialPortName = "COM99";
            serialBaudrate = 115200;
        }

        public SerialPortManager(string COMName, int Baudrate)
        {
            serialPortName = COMName;
            serialBaudrate = Baudrate;
        }

        public void SetComPortManual(string portName)
        {
            serialPortName = portName;
        }

        public void SetComSpeedManual(int baudRate)
        {
            serialBaudrate = baudRate;
        }

        /*****************************************************************
         * 
         * EVENT HANDLER
         * 
         * ***************************************************************/
        /// <summary>
        /// Serial port event handler delegation
        /// </summary>
        public delegate void SerialPortNotifyEventHandler(object sender, string msg);
        public delegate void SerialPortDataEventHandler(object sender, byte[] data);
        

        /// <summary>
        /// Update the serial port status to the event subcriber
        /// </summary>
        public event SerialPortNotifyEventHandler OnStatusChanged;

        /// <summary>
        /// Update received data from the serial port to the event subcriber
        /// </summary>
        public event SerialPortDataEventHandler OnDataReceived;

 

        /*****************************************************************
         * 
         * STATUS
         * 
         * ***************************************************************/
        /// <summary>
        /// Return TRUE if the serial port is currently connected
        /// </summary>
        public bool isConnected { get { return serialPort.IsOpen; } }
        /// <summary>
        /// Return Serial Port Name
        /// </summary>
        public string getPortName { get { return this.serialPortName; } }
        /// <summary>
        /// Return Serial Port Baud Rate
        /// </summary>
        public int getPortBaudRate { get { return this.serialBaudrate; } }

        /*****************************************************************
         * 
         * BASIC FUNCTIONS
         * 
         * ***************************************************************/
        #region Port process
        public void Open()
        {
            if (serialPort.IsOpen == false)
            {
                try
                {
                    serialPort.PortName = serialPortName;
                    serialPort.BaudRate = serialBaudrate;
                    serialPort.Parity = Parity.None;
                    serialPort.StopBits = StopBits.One;
                    serialPort.Handshake = Handshake.None;
                    serialPort.DataBits = 8;

                    serialPort.ReadBufferSize = 4096;      // Buffer for read
                    serialPort.WriteBufferSize = 8192;     // Buffer for write

                    // milliseconds before a time-out occurs when a read/write operation does not finish.
                    serialPort.WriteTimeout = 10000; // 10s ~ 12kB with slowest speed at 9600bps ==> Only support speed 9600bps or higher
                    serialPort.ReadTimeout = 500;

                    serialPort.Open();

                    OnStatusChanged?.Invoke(this, string.Format("Open {0} successful", serialPortName));

                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();

                    StartSendingSvc();
                    StartReadingSvc();
                }
                catch (Exception err)
                {

                    OnStatusChanged?.Invoke(this, "[Error] " + err.Message);
                }
            }
            else
            {
                    OnStatusChanged?.Invoke(this, string.Format("{0} already in use.", serialPortName));                
            }
        }
        public void Close()
        {  
            if (serialPort.IsOpen)
            {
                serialPort.BaseStream.Flush();
                while (sendMsgQueue.Length != 0) ;
                serialPort.DiscardOutBuffer();
            }
                
            StopSendingSvc();

            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            OnStatusChanged?.Invoke(this, string.Format( "{0} Connection closed",serialPortName));
        }
        #endregion

        #region Write process
        public void SendMessage(byte[] data)
        {
            if (!serialPort.IsOpen)
            {
                this.Open();
            }
            if (data.Length > serialPort.WriteBufferSize)
            {
                OnStatusChanged?.Invoke(this, "Data lenght to much!");
                return;
            }
            sendMsgQueue.PutBytes(data);
            sendMsgEvent.Set();
            Console.WriteLine("SendQueue size:" + sendMsgQueue.Length);
        }
        private void StartSendingSvc()
        {
            if (!keepSending)
            {
                keepSending = true;
                sendMsgQueue = new SerialPortBuffer();
                sendThread = new Thread(SendThread);
                sendThread.Start();
            }
        }
        private void StopSendingSvc()
        {
            if (keepSending)
            {
                keepSending = false;
                sendThread.Join();
                sendThread = null;
                sendMsgQueue = null;
            }
        }
        private void SendThread()
        {
            while (keepSending)
            {
                if (sendMsgQueue.Length > 0 || sendMsgEvent.WaitOne(100))
                {
                    sendMsgEvent.Reset();

                    byte[] data = sendMsgQueue.RetrieveBytes(sendMsgQueue.Length > 1024 ? 1024 : sendMsgQueue.Length);

                    WriteData(data, 0, data.Length);
                }
                sendMsgEvent.Reset();
            }
        }
        private void WriteData(byte[] buffer, int offset, int count)
        {
            if (serialPort.IsOpen)
                try
                {
                    serialPort.Write(buffer, offset, count);
                    //OnStatusChanged?.Invoke(this, "Data sent");
                }
                catch (Exception)
                {
                    OnStatusChanged?.Invoke(this, "False to send data.");
                }
            else
            {  
                this.Open();
            }
        }
        #endregion

        #region Reading process
        private void StartReadingSvc()
        {
            var buffer = new byte[256];
            Action startListen = null;
            var onRecvData = new AsyncCallback(result => OnRecvData(result, startListen, serialPort, buffer));

            startListen = () =>
            {
                serialPort.BaseStream.BeginRead(buffer, 0, buffer.Length, onRecvData, null);
            };

            startListen();
        }
        private void OnRecvData(IAsyncResult result, Action startListen, SerialPort port, byte[] buffer)
        {
            if (port.IsOpen)
            {
                var actualLeng = port.BaseStream.EndRead(result);
                var received = new byte[actualLeng];
                Buffer.BlockCopy(buffer, 0, received, 0, actualLeng);
                OnDataReceived?.Invoke(this, received);
                startListen();
            }
        }
        #endregion
    }
}
