using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;

namespace PhoneLog
{
  class PhoneLog
  {
    static int Main(string[] args)
    {
      string porttoopen = args[0];
      if (args.Length != 2)
      {
        Console.WriteLine("Usage: PhoneLog {comX} {path to phone log}");
        return 1;
      }
      string path = args[1];

      
      StreamWriter sw;
      if (!File.Exists(path))
      {
        sw = File.CreateText(path);
        sw.Close();
      }
      
      SerialPort port = new SerialPort(porttoopen, 19200, Parity.None, 8);
      port.Open();
      port.DtrEnable = true;
      port.RtsEnable = true;
      String line;
      port.ReadTimeout = 500;
      while (true)
      {
        try
        {
          line = port.ReadLine();
          if (!line.Contains("ATQ0E0V1S0"))
          {
            sw = File.AppendText(path);
            sw.WriteLine(line);
            sw.Close();
          }
          Console.WriteLine(line);
          // get rid of silly unreachable code warning
          if (line.Equals("Make it stop!")) break;
        }
        catch (TimeoutException)
        {
          // Once we make this into a service, check for service stop command here.
        }
      }
      return 1;
    }
  }
}
