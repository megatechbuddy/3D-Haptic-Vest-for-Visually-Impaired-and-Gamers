using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.IO.Ports;
using System.Windows;
using System.Net.Mail;
using System.Net;
using System.Diagnostics;
using System.ComponentModel;

namespace talking
{
    class function
    {

    }
    class Program
    {
        public class SomeGlobalVariables
        {
            public static bool[] list = new bool[240];
            public static int[] listformotors = new int[48];
            public static int[] listformotors2 = new int[48];
        } 



        static void Main(string[] args)
        {
            
            //  hook up the event for receiving the data 


            using (MemoryMappedFile mmf = MemoryMappedFile.CreateNew("testmap", 10000))
            {
                bool mutexCreated;
                Mutex mutex = new Mutex(true, "testmapmutex", out mutexCreated);
                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                {
                    BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(1);
                }
                mutex.ReleaseMutex();

                Console.WriteLine("Start Process B and press ENTER to continue.");
                Console.ReadLine();
                int helpbuttoncount = 0;
                DateTime firstDate = DateTime.Now;
                while (true)
                {

                    mutex.WaitOne();

                    using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                    {
                        BinaryReader reader = new BinaryReader(stream);
                        for (int ii = 0; ii < 48; ii++)
                        {
                            SomeGlobalVariables.list[ii * 5] = reader.ReadBoolean();
                            SomeGlobalVariables.list[ii * 5 + 1] = reader.ReadBoolean();
                            SomeGlobalVariables.list[ii * 5 + 2] = reader.ReadBoolean();
                            SomeGlobalVariables.list[ii * 5 + 3] = reader.ReadBoolean();
                            SomeGlobalVariables.list[ii * 5 + 4] = reader.ReadBoolean();
                            //Console.WriteLine(SomeGlobalVariables.list[ii * 5]);
                            //Console.WriteLine(SomeGlobalVariables.list[ii * 5 + 1]);
                            //Console.WriteLine(SomeGlobalVariables.list[ii * 5 + 2]);
                            //Console.WriteLine(SomeGlobalVariables.list[ii * 5 + 3]);
                            //Console.WriteLine(SomeGlobalVariables.list[ii * 5 + 4]);
                        }
                    }
                    newconvert(); // Converts binary to decimal
                    spaghetti();
                    startvest(ref helpbuttoncount);  // Coverts the decimal to output
                    Console.WriteLine(helpbuttoncount);
                    if (helpbuttoncount == 1)
                    {
                        firstDate = DateTime.Now;
                    }
                    TimeSpan dateDiff;
                    DateTime secondDate = DateTime.Now;
                    dateDiff = secondDate.Subtract(firstDate);
                    if (dateDiff.TotalSeconds >= 10)
                    {
                        if (helpbuttoncount >= 6)
                        {
                            Process myProcess = new Process();
                            try
                            {
                                myProcess.StartInfo.UseShellExecute = false;
                                // You can start any process, HelloWorld is a do-nothing example.
                                myProcess.StartInfo.FileName = "C:\\Users\\Tim\\Documents\\ColorBasics-WPF\\bin\\Debug\\ColorBasics-WPF.exe";
                                myProcess.StartInfo.CreateNoWindow = true;
                                myProcess.Start();
                            }
                            catch { }
                            helpbuttoncount = 0;
                            
                        }
                    }
                   
                    Console.WriteLine(dateDiff);
                    // Console.ReadLine();
                    
                    // mutex.ReleaseMutex(); 
                }
            }
        }

        static void newconvert()
        {
            for (int i = 0; i < 48; i++)
            {
                SomeGlobalVariables.listformotors[i] = 0; 
            }
            for (int i = 0; i < 48; i++)
            {
                if (SomeGlobalVariables.list[i * 5] == false && SomeGlobalVariables.list[i * 5 + 1] == false && SomeGlobalVariables.list[i * 5 + 2] == false && SomeGlobalVariables.list[i * 5 + 3] == false && SomeGlobalVariables.list[i * 5 + 4] == false)
                {
                    SomeGlobalVariables.listformotors[i] = 0;
                }
                else if (SomeGlobalVariables.list[i * 5] == true && SomeGlobalVariables.list[i * 5 + 1] == false && SomeGlobalVariables.list[i * 5 + 2] == false && SomeGlobalVariables.list[i * 5 + 3] == false && SomeGlobalVariables.list[i * 5 + 4] == false)
                {
                    SomeGlobalVariables.listformotors[i] = 1;
                }
                else if (SomeGlobalVariables.list[i * 5] == false && SomeGlobalVariables.list[i * 5 + 1] == true && SomeGlobalVariables.list[i * 5 + 2] == false && SomeGlobalVariables.list[i * 5 + 3] == false && SomeGlobalVariables.list[i * 5 + 4] == false)
                {
                    SomeGlobalVariables.listformotors[i] = 2;
                }
                else if (SomeGlobalVariables.list[i * 5] == true && SomeGlobalVariables.list[i * 5 + 1] == true && SomeGlobalVariables.list[i * 5 + 2] == false && SomeGlobalVariables.list[i * 5 + 3] == false && SomeGlobalVariables.list[i * 5 + 4] == false)
                {
                    SomeGlobalVariables.listformotors[i] = 3;
                }
                else if (SomeGlobalVariables.list[i * 5] == false && SomeGlobalVariables.list[i * 5 + 1] == false && SomeGlobalVariables.list[i * 5 + 2] == true && SomeGlobalVariables.list[i * 5 + 3] == false && SomeGlobalVariables.list[i * 5 + 4] == false)
                {
                    SomeGlobalVariables.listformotors[i] = 4;
                }
                else if (SomeGlobalVariables.list[i * 5] == true && SomeGlobalVariables.list[i * 5 + 1] == false && SomeGlobalVariables.list[i * 5 + 2] == true && SomeGlobalVariables.list[i * 5 + 3] == false && SomeGlobalVariables.list[i * 5 + 4] == false)
                {
                    SomeGlobalVariables.listformotors[i] = 5;
                }
                else if (SomeGlobalVariables.list[i * 5] == false && SomeGlobalVariables.list[i * 5 + 1] == true && SomeGlobalVariables.list[i * 5 + 2] == true && SomeGlobalVariables.list[i * 5 + 3] == false && SomeGlobalVariables.list[i * 5 + 4] == false)
                {
                    SomeGlobalVariables.listformotors[i] = 6;
                }
                else if (SomeGlobalVariables.list[i * 5] == true && SomeGlobalVariables.list[i * 5 + 1] == true && SomeGlobalVariables.list[i * 5 + 2] == true && SomeGlobalVariables.list[i * 5 + 3] == false && SomeGlobalVariables.list[i * 5 + 4] == false)
                {
                    SomeGlobalVariables.listformotors[i] = 7;
                }
                else if (SomeGlobalVariables.list[i * 5] == false && SomeGlobalVariables.list[i * 5 + 1] == false && SomeGlobalVariables.list[i * 5 + 2] == false && SomeGlobalVariables.list[i * 5 + 3] == true && SomeGlobalVariables.list[i * 5 + 4] == false)
                {
                    SomeGlobalVariables.listformotors[i] = 8;
                }
                else if (SomeGlobalVariables.list[i * 5] == true && SomeGlobalVariables.list[i * 5 + 1] == false && SomeGlobalVariables.list[i * 5 + 2] == false && SomeGlobalVariables.list[i * 5 + 3] == true && SomeGlobalVariables.list[i * 5 + 4] == false)
                {
                    SomeGlobalVariables.listformotors[i] = 9;
                } 
                else if (SomeGlobalVariables.list[i * 5] == false && SomeGlobalVariables.list[i * 5 + 1] == true && SomeGlobalVariables.list[i * 5 + 2] == false && SomeGlobalVariables.list[i * 5 + 3] == true && SomeGlobalVariables.list[i * 5 + 4] == false)
                {
                    SomeGlobalVariables.listformotors[i] = 10;
                }
                else if (SomeGlobalVariables.list[i * 5] == true && SomeGlobalVariables.list[i * 5 + 1] == true && SomeGlobalVariables.list[i * 5 + 2] == false && SomeGlobalVariables.list[i * 5 + 3] == true && SomeGlobalVariables.list[i * 5 + 4] == false)
                {
                    SomeGlobalVariables.listformotors[i] = 11;
                }
                else if (SomeGlobalVariables.list[i * 5] == false && SomeGlobalVariables.list[i * 5 + 1] == false && SomeGlobalVariables.list[i * 5 + 2] == true && SomeGlobalVariables.list[i * 5 + 3] == true && SomeGlobalVariables.list[i * 5 + 4] == false)
                {
                    SomeGlobalVariables.listformotors[i] = 12;
                }
                else if (SomeGlobalVariables.list[i * 5] == true && SomeGlobalVariables.list[i * 5 + 1] == false && SomeGlobalVariables.list[i * 5 + 2] == true && SomeGlobalVariables.list[i * 5 + 3] == true && SomeGlobalVariables.list[i * 5 + 4] == false)
                {
                    SomeGlobalVariables.listformotors[i] = 13;
                }
                else if (SomeGlobalVariables.list[i * 5] == false && SomeGlobalVariables.list[i * 5 + 1] == true && SomeGlobalVariables.list[i * 5 + 2] == true && SomeGlobalVariables.list[i * 5 + 3] == true && SomeGlobalVariables.list[i * 5 + 4] == false)
                {
                    SomeGlobalVariables.listformotors[i] = 14;
                }
                else if (SomeGlobalVariables.list[i * 5] == true && SomeGlobalVariables.list[i * 5 + 1] == true && SomeGlobalVariables.list[i * 5 + 2] == true && SomeGlobalVariables.list[i * 5 + 3] == true && SomeGlobalVariables.list[i * 5 + 4] == false)
                {
                    SomeGlobalVariables.listformotors[i] = 15;
                }
                else if (SomeGlobalVariables.list[i * 5] == false && SomeGlobalVariables.list[i * 5 + 1] == true && SomeGlobalVariables.list[i * 5 + 2] == true && SomeGlobalVariables.list[i * 5 + 3] == false && SomeGlobalVariables.list[i * 5 + 4] == true)
                {
                    SomeGlobalVariables.listformotors[i] = 16;
                }
                
                
            }
        //    SomeGlobalVariables.listformotorsb[0] = SomeGlobalVariables.listformotorsb[0];
        }
       
        static void spaghetti()
        {
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine(SomeGlobalVariables.listformotors[i] + " " + SomeGlobalVariables.listformotors[4+i] + " " + SomeGlobalVariables.listformotors[8+i] + " " + SomeGlobalVariables.listformotors[12+i] + " " + SomeGlobalVariables.listformotors[16+i] + " " + SomeGlobalVariables.listformotors[20+i] + " " + SomeGlobalVariables.listformotors[24+i] + " " + SomeGlobalVariables.listformotors[28+i] + " " + SomeGlobalVariables.listformotors[32+i] + " " + SomeGlobalVariables.listformotors[36+i] + " " + SomeGlobalVariables.listformotors[40+i] + " " + SomeGlobalVariables.listformotors[44+i]);
            }
            Console.WriteLine("");
            Console.WriteLine("");

            //SomeGlobalVariables.listformotors[0] = 0;
            //SomeGlobalVariables.listformotors[1] = 0;
            //SomeGlobalVariables.listformotors[2] = 0;
            //SomeGlobalVariables.listformotors[3] = 0;
            //
            //SomeGlobalVariables.listformotors[4] = 0;
            //SomeGlobalVariables.listformotors[5] = 0;
            //SomeGlobalVariables.listformotors[6] = 0;
            //SomeGlobalVariables.listformotors[7] = 0;
            //
            //SomeGlobalVariables.listformotors[8] = 0;
            //SomeGlobalVariables.listformotors[9] = 0;
            //SomeGlobalVariables.listformotors[10] = 0;
            //SomeGlobalVariables.listformotors[11] = 0;
            //
            //SomeGlobalVariables.listformotors[12] = 0;
            //SomeGlobalVariables.listformotors[13] = 0;
            //SomeGlobalVariables.listformotors[14] = 0;
            //SomeGlobalVariables.listformotors[15] = 0;
            //
            //SomeGlobalVariables.listformotors[16] = 0;
            //SomeGlobalVariables.listformotors[17] = 0;
            //SomeGlobalVariables.listformotors[18] = 0;
            //SomeGlobalVariables.listformotors[19] = 0;
            //
            //SomeGlobalVariables.listformotors[20] = 0;
            //SomeGlobalVariables.listformotors[21] = 0;
            //SomeGlobalVariables.listformotors[22] = 0;
            //SomeGlobalVariables.listformotors[23] = 0;

            //SomeGlobalVariables.listformotors[0] = 1;



            //middle
            SomeGlobalVariables.listformotors2[14] = SomeGlobalVariables.listformotors[36];
            SomeGlobalVariables.listformotors2[13] = SomeGlobalVariables.listformotors[37];
            SomeGlobalVariables.listformotors2[12] = SomeGlobalVariables.listformotors[38];
            SomeGlobalVariables.listformotors2[16] = SomeGlobalVariables.listformotors[39];

            SomeGlobalVariables.listformotors2[15] = SomeGlobalVariables.listformotors[40];
            SomeGlobalVariables.listformotors2[11] = SomeGlobalVariables.listformotors[41];
            SomeGlobalVariables.listformotors2[10] = SomeGlobalVariables.listformotors[42];
            SomeGlobalVariables.listformotors2[17] = SomeGlobalVariables.listformotors[43];

            SomeGlobalVariables.listformotors2[9] = SomeGlobalVariables.listformotors[44];
            SomeGlobalVariables.listformotors2[8] = SomeGlobalVariables.listformotors[45];
            SomeGlobalVariables.listformotors2[7] = SomeGlobalVariables.listformotors[46];
            SomeGlobalVariables.listformotors2[6] = SomeGlobalVariables.listformotors[47];


            SomeGlobalVariables.listformotors2[5] = SomeGlobalVariables.listformotors[0];
            SomeGlobalVariables.listformotors2[2] = SomeGlobalVariables.listformotors[1];
            SomeGlobalVariables.listformotors2[4] = SomeGlobalVariables.listformotors[2];
            SomeGlobalVariables.listformotors2[3] = SomeGlobalVariables.listformotors[3];

            SomeGlobalVariables.listformotors2[18] = SomeGlobalVariables.listformotors[4];
            SomeGlobalVariables.listformotors2[21] = SomeGlobalVariables.listformotors[5];
            SomeGlobalVariables.listformotors2[0] = SomeGlobalVariables.listformotors[6];
            SomeGlobalVariables.listformotors2[1] = SomeGlobalVariables.listformotors[7];

            SomeGlobalVariables.listformotors2[20] = SomeGlobalVariables.listformotors[8];
            SomeGlobalVariables.listformotors2[19] = SomeGlobalVariables.listformotors[9];
            SomeGlobalVariables.listformotors2[23] = SomeGlobalVariables.listformotors[10];
            SomeGlobalVariables.listformotors2[22] = SomeGlobalVariables.listformotors[11];




            //sides
            SomeGlobalVariables.listformotors2[32] = SomeGlobalVariables.listformotors[24];
            SomeGlobalVariables.listformotors2[35] = SomeGlobalVariables.listformotors[25];
            SomeGlobalVariables.listformotors2[31] = SomeGlobalVariables.listformotors[26];
            SomeGlobalVariables.listformotors2[30] = SomeGlobalVariables.listformotors[27];
            SomeGlobalVariables.listformotors2[41] = SomeGlobalVariables.listformotors[28];
            SomeGlobalVariables.listformotors2[40] = SomeGlobalVariables.listformotors[29];
            SomeGlobalVariables.listformotors2[33] = SomeGlobalVariables.listformotors[30];
            SomeGlobalVariables.listformotors2[34] = SomeGlobalVariables.listformotors[31];
            SomeGlobalVariables.listformotors2[38] = SomeGlobalVariables.listformotors[32];
            SomeGlobalVariables.listformotors2[39] = SomeGlobalVariables.listformotors[33];
            SomeGlobalVariables.listformotors2[37] = SomeGlobalVariables.listformotors[34];
            SomeGlobalVariables.listformotors2[36] = SomeGlobalVariables.listformotors[35];
                                             
            SomeGlobalVariables.listformotors2[42] = SomeGlobalVariables.listformotors[12];
            SomeGlobalVariables.listformotors2[44] = SomeGlobalVariables.listformotors[13];
            SomeGlobalVariables.listformotors2[43] = SomeGlobalVariables.listformotors[14];
            SomeGlobalVariables.listformotors2[27] = SomeGlobalVariables.listformotors[15];
            SomeGlobalVariables.listformotors2[47] = SomeGlobalVariables.listformotors[16];
            SomeGlobalVariables.listformotors2[45] = SomeGlobalVariables.listformotors[17];
            SomeGlobalVariables.listformotors2[29] = SomeGlobalVariables.listformotors[18];
            SomeGlobalVariables.listformotors2[46] = SomeGlobalVariables.listformotors[19];
            SomeGlobalVariables.listformotors2[26] = SomeGlobalVariables.listformotors[20];
            SomeGlobalVariables.listformotors2[24] = SomeGlobalVariables.listformotors[21];
            SomeGlobalVariables.listformotors2[28] = SomeGlobalVariables.listformotors[22];
            SomeGlobalVariables.listformotors2[25] = SomeGlobalVariables.listformotors[23];
                                             

        }

        static void startvest(ref int helpbuttoncount)
        {
            SerialPort _port = new SerialPort("COM5") { BaudRate = 9600};

            string _lastData = "";
            _port.Open();
            string data = "";
            _port.Write("9901");//frame keyvf
            _port.Write("\n\r");
            for (int a = 0; a < 48; a += 1)
            {
                if (SomeGlobalVariables.listformotors2[a] == 0)
                {
                    _port.Write("0");
                }
                else if (SomeGlobalVariables.listformotors2[a] == 8)
                {
                    _port.Write("500");
                }
                else if (SomeGlobalVariables.listformotors2[a] == 7)
                {
                    _port.Write("1000");
                }
                else if (SomeGlobalVariables.listformotors2[a] == 6)
                {
                    _port.Write("1500");
                }
                else if (SomeGlobalVariables.listformotors2[a] == 5)
                {
                    _port.Write("2000");
                }
                else if (SomeGlobalVariables.listformotors2[a] == 4)
                {
                    _port.Write("2500");
                }
                else if (SomeGlobalVariables.listformotors2[a] == 3)
                {
                    _port.Write("3000");
                }
                else if (SomeGlobalVariables.listformotors2[a] == 2)
                {
                    _port.Write("3500");
                }
                else if (SomeGlobalVariables.listformotors2[a] == 1)
                {
                    _port.Write("4095");
                }
                else
                {
                    _port.Write("0");
                }
                _port.Write("\n\r");
                data = data + _port.ReadExisting();
                if (data == "hi") 
                {
                    Console.WriteLine(data);
                    helpbuttoncount += 1;
                    data = "";
                }
            }
           
            _port.Close();
            

        }
    }
    
}
