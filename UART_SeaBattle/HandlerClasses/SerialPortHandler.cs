using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UART_SeaBattle.HandlerClasses
{
    public class SerialPortHandler
    {
        private SerialPort _serialPort;
        public Action<byte[]>? OnCommandReceived;

        public SerialPortHandler(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate)
            {
                Parity = Parity.None,
                StopBits = StopBits.One,
                DataBits = 8,
                Handshake = Handshake.None
            };

            _serialPort.DataReceived += SerialPort_DataReceived;
            _serialPort.Open();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int bytesToRead = _serialPort.BytesToRead;
            byte[] buffer = new byte[bytesToRead];
            _serialPort.Read(buffer, 0, bytesToRead);

            // Передаем полученные байты в обработчик
            OnCommandReceived?.Invoke(buffer);
        }

        public void Dispose()
        {
            if (_serialPort.IsOpen)
                _serialPort.Close();
            _serialPort.Dispose();
        }
    }
}
