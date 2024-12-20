using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UART_SeaBattle;
using WindowsInput;
using WindowsInput.Native;

namespace StmReader;

public class SerialIO
{

	static SerialPort ComPort = new SerialPort("COM4", 115200, Parity.None, 8, StopBits.One);

	static readonly string PRESSED_BNT_ID_PATTERN = @"BTN_ID=(\d+)";

	static readonly InputSimulator simulator = new InputSimulator();

	private enum ButtonId
	{
		Shoot = 0,
		RotateLeft = 1,
		RotateRight = 2
	}

	private static VirtualKeyCode[] codes = 
	{
		VirtualKeyCode.SPACE,              
		VirtualKeyCode.LEFT,               
        VirtualKeyCode.RIGHT,

	};

	static readonly Dictionary<string, Action<string>> patternsToActions = new Dictionary<string, Action<string>>
	{
		{ PRESSED_BNT_ID_PATTERN, data =>
			{
				Match match = Regex.Match(data, PRESSED_BNT_ID_PATTERN);
				if (match.Success)
				{
					 int ID = int.Parse(match.Groups[1].Value);
					 if (Enum.IsDefined(typeof(ButtonId), ID)) {
						VirtualKeyCode code = codes[ID];

                         simulator.Keyboard.KeyPress(code);

					}
					
				}
			}
		},
	};

	private static void processRecievedData(string receivedData)
	{
		foreach (var pattern in patternsToActions)
		{
			Match match = Regex.Match(receivedData, pattern.Key);
			if (match.Success)
			{
				pattern.Value(receivedData);
				return;
			}
		}

		Console.WriteLine("No valid pattern received found. " + "\"" + receivedData + "\"");
	}


	[STAThread]
	public static void Start()
	{
		SerialPortProgram();
	}

	private static void SerialPortProgram()
	{
		ComPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
		try
		{
			ComPort.Open();
		}
		catch (Exception)
		{
			Console.WriteLine("Unable to open com port");
			return;
		}
	}

	private static void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
	{

		string receiveData;
		try
		{
			receiveData = ComPort.ReadLine();
		}
		catch (InvalidOperationException)
		{
			Console.WriteLine("Com port is not opened");
			return;
		}
		catch (TimeoutException)
		{
			Console.WriteLine("No bytes were readed");
			return;
		}
		catch (Exception)
		{
			Console.WriteLine("Undefined exception occured While reading from com port");
			return;
		}
		finally
		{
			Console.WriteLine("This code display anyway");
		}
		processRecievedData(receiveData);

	}

	public static void SendData(string data)
	{
		ComPort.WriteLine(data);
	}
}
