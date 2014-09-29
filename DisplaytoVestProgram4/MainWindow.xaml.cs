namespace Microsoft.Samples.Kinect.DepthBasics
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;
    using System.IO.MemoryMappedFiles;
    using System.Threading;

    public partial class MainWindow : Window
    {
        private KinectSensor sensor;

        private WriteableBitmap colorBitmap;

        private short[] depthPixels;

        private byte[] colorPixels;

        bool PortState1 = false;

        int[] motorvalue = new int[48];
        int[] motorvalue2 = new int[48];

        int[,] Depthstream = new int[80,60];

        int xlength = 80;
        int ylength = 60;

        int sizeofboxx = 6;
        int sizeofboxy = 15;
        int boxesinpicturex = 12;
        int boxesinpicturey = 4;

        DateTime firstDate = DateTime.Now;

        public MainWindow()
        {
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                // Turn on the depth stream to receive depth frames
                this.sensor.DepthStream.Enable(DepthImageFormat.Resolution80x60Fps30);
                
                // Allocate space to put the depth pixels we'll receive
                this.depthPixels = new short[this.sensor.DepthStream.FramePixelDataLength];

                // Allocate space to put the color pixels we'll create
                this.colorPixels = new byte[this.sensor.DepthStream.FramePixelDataLength * sizeof(int)];

                // This is the bitmap we'll display on-screen
                this.colorBitmap = new WriteableBitmap(this.sensor.DepthStream.FrameWidth, this.sensor.DepthStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                // Set the image we display to point to the bitmap where we'll put the image data
                this.Image.Source = this.colorBitmap;

                // Add an event handler to be called whenever there is new depth frame data
                this.sensor.DepthFrameReady += this.SensorDepthFrameReady;

                // Start the sensor!
                try
                {
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }

            if (null == this.sensor)
            {
            }

        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
            }
        }

        private void SensorDepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (DepthImageFrame depthFrame = e.OpenDepthImageFrame())
            {
                int a = 0;
                int b = 0;
                if (depthFrame != null)
                {
                    // Copy the pixel data from the image to a temporary array
                    depthFrame.CopyPixelDataTo(this.depthPixels);
                    //snapshot
                    
                    TimeSpan dateDiff;
                    DateTime secondDate = DateTime.Now;
                    dateDiff = secondDate.Subtract(firstDate);
                    if (dateDiff.TotalSeconds >= 60)
                    {
                        firstDate = DateTime.Now;
                        if (null == this.sensor)
                        {
                            return;
                        }

                        // create a png bitmap encoder which knows how to save a .png file
                        BitmapEncoder encoder = new PngBitmapEncoder();

                        // create frame from the writable bitmap and add to encoder
                        encoder.Frames.Add(BitmapFrame.Create(this.colorBitmap));

                        string time = System.DateTime.Now.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);

                        string myPhotos = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                        string path = Path.Combine(myPhotos, "KinectSnapshot-" + time + ".png");

                        // write the new file to disk
                        try
                        {
                            using (FileStream fs = new FileStream(path, FileMode.Create))
                            {
                                encoder.Save(fs);
                            }
                        }
                        catch (IOException)
                        {
                        }
                    }
                    // Convert the depth to RGB
                    int colorPixelIndex = 0;
                    for (int i = 0; i < this.depthPixels.Length; ++i)
                    {
                        // discard the portion of the depth that contains only the player index
                        short depth = (short)(this.depthPixels[i] >> DepthImageFrame.PlayerIndexBitmaskWidth);

                        // to convert to a byte we're looking at only the lower 8 bits
                        // by discarding the most significant rather than least significant data
                        // we're preserving detail, although the intensity will "wrap"
                        // add 1 so that too far/unknown is mapped to black
                        byte intensity = (byte)((depth + 1) & byte.MaxValue);
                        
                        Depthstream[a,b] = depth;

                        if (depth <= 480)
                        {

                            // Write out blue byte
                            this.colorPixels[colorPixelIndex++] = 0;

                            // Write out green byte
                            this.colorPixels[colorPixelIndex++] = 0;

                            // Write out red byte                        
                            this.colorPixels[colorPixelIndex++] = 0;

                        }
                        if (depth > 480 && depth <= 880)
                        {

                            // Write out blue byte
                            this.colorPixels[colorPixelIndex++] = 35;

                            // Write out green byte
                            this.colorPixels[colorPixelIndex++] = 35;

                            // Write out red byte                        
                            this.colorPixels[colorPixelIndex++] = 35;

                        }
                        if (depth > 880 && depth <= 1180)
                        {

                            // Write out blue byte
                            this.colorPixels[colorPixelIndex++] = 60;

                            // Write out green byte
                            this.colorPixels[colorPixelIndex++] = 60;

                            // Write out red byte                        
                            this.colorPixels[colorPixelIndex++] = 60;

                        }
                        if (depth > 1180 && depth <= 1480)
                        {

                            // Write out blue byte
                            this.colorPixels[colorPixelIndex++] = 85;

                            // Write out green byte
                            this.colorPixels[colorPixelIndex++] = 85;

                            // Write out red byte                        
                            this.colorPixels[colorPixelIndex++] = 85;

                        }
                        if (depth > 1480 && depth <= 1780)
                        {

                            // Write out blue byte
                            this.colorPixels[colorPixelIndex++] = 130;

                            // Write out green byte
                            this.colorPixels[colorPixelIndex++] = 130;

                            // Write out red byte                        
                            this.colorPixels[colorPixelIndex++] = 130;

                        }
                        if (depth > 1780 && depth <= 2080)
                        {

                            // Write out blue byte
                            this.colorPixels[colorPixelIndex++] = 170;

                            // Write out green byte
                            this.colorPixels[colorPixelIndex++] = 170;

                            // Write out red byte                        
                            this.colorPixels[colorPixelIndex++] = 170;

                        }
                        if (depth > 2080 && depth <= 2380)
                        {

                            // Write out blue byte
                            this.colorPixels[colorPixelIndex++] = 210;

                            // Write out green byte
                            this.colorPixels[colorPixelIndex++] = 210;

                            // Write out red byte                        
                            this.colorPixels[colorPixelIndex++] = 210;

                        }
                        if (depth > 2380)
                        {

                            // Write out blue byte
                            this.colorPixels[colorPixelIndex++] = 255;

                            // Write out green byte
                            this.colorPixels[colorPixelIndex++] = 255;

                            // Write out red byte                        
                            this.colorPixels[colorPixelIndex++] = 255;

                        }
                        
                      
                                                
                        // We're outputting BGR, the last byte in the 32 bits is unused so skip it
                        // If we were outputting BGRA, we would write alpha here.
                        ++colorPixelIndex;
                        a = a + 1;
                        if (a == xlength)
                        {
                            a = 0;
                            b = b + 1;
                        }
                    }
                    int totalinsquare = sizeofboxx * sizeofboxy;

                    int[,] adveragepicture = new int[boxesinpicturex, boxesinpicturey];
                    // -1 and 4095 are boundries
                    for (int j = 0; j < boxesinpicturex; ++j)
                    {
                        for (int k = 0; k < boxesinpicturey; ++k)
                        {
                            totalinsquare = sizeofboxx * sizeofboxy;
                            for (int l = 0; l < sizeofboxx; ++l)
                            {
                                for (int m = 0; m < sizeofboxy; ++m)
                                {
                                    if (Depthstream[j * sizeofboxx + l, k * sizeofboxy + m] == -1)
                                    {
                                        totalinsquare -= 1;
                                    }
                                    else
                                    {
                                        adveragepicture[j, k] = adveragepicture[j, k] + Depthstream[j * sizeofboxx + l, k * sizeofboxy + m];
                                    }
                                }
                            }
                            if (adveragepicture[j, k] != 0 && totalinsquare != 0)
                            {
                                adveragepicture[j, k] = adveragepicture[j, k] / totalinsquare;
                            }
                        }

                    }
                    int motornumber = 0;
                    for (int n = 0; n < boxesinpicturex; ++n)
                    {
                        for (int o = 0; o < boxesinpicturey; ++o)
                        {
                            if (adveragepicture[n, o] <= 480)
                            {
                                motorvalue[motornumber] = 1;
                            }
                            else if (adveragepicture[n, o] > 480 && adveragepicture[n, o] <= 880)
                            {
                                motorvalue[motornumber] = 2;
                            }
                            else if (adveragepicture[n, o] > 880 && adveragepicture[n, o] <= 1180)
                            {
                                motorvalue[motornumber] = 3;
                            }
                            else if (adveragepicture[n, o] > 1180 && adveragepicture[n, o] <= 1480)
                            {
                                motorvalue[motornumber] = 4;
                            }
                            else if (adveragepicture[n, o] > 1480 && adveragepicture[n, o] <= 1780)
                            {
                                motorvalue[motornumber] = 5;
                            }
                            else if (adveragepicture[n, o] > 1780 && adveragepicture[n, o] <= 2080)
                            {
                                motorvalue[motornumber] = 6;
                            }
                            else if (adveragepicture[n, o] > 2080 && adveragepicture[n, o] <= 2380)
                            {
                                motorvalue[motornumber] = 7;
                            }
                            else if (adveragepicture[n, o] > 2380 && adveragepicture[n, o] <= 2680)
                            {
                                motorvalue[motornumber] = 8;
                            }
                            else if (adveragepicture[n, o] > 2680)
                            {
                                motorvalue[motornumber] = 0;
                            }
                            motornumber += 1;
                        }
                    }
                    updatebuttons();
                    if (PortState1 == true)
                    {
                        StreamtoOtherProgram();
                    }


                    // Write the pixel data into our bitmap
                    this.colorBitmap.WritePixels(
                        new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                        this.colorPixels,
                        this.colorBitmap.PixelWidth * sizeof(int),
                        0);

                }
            }
        }
      
        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        

        

        }

        public void updatebuttons()
        {
            button0.Content = motorvalue[0];
            button1.Content = motorvalue[1];
            button2.Content = motorvalue[2];
            button3.Content = motorvalue[3];
            button4.Content = motorvalue[4];
            button5.Content = motorvalue[5];
            button6.Content = motorvalue[6];
            button7.Content = motorvalue[7];
            button8.Content = motorvalue[8];
            button9.Content = motorvalue[9];
            button10.Content = motorvalue[10];
            button11.Content = motorvalue[11];
            button12.Content = motorvalue[12];
            button13.Content = motorvalue[13];
            button14.Content = motorvalue[14];
            button15.Content = motorvalue[15];
            button16.Content = motorvalue[16];
            button17.Content = motorvalue[17];
            button18.Content = motorvalue[18];
            button19.Content = motorvalue[19];
            button20.Content = motorvalue[20];
            button21.Content = motorvalue[21];
            button22.Content = motorvalue[22];
            button23.Content = motorvalue[23];
            button24.Content = motorvalue[24];
            button25.Content = motorvalue[25];
            button26.Content = motorvalue[26];
            button27.Content = motorvalue[27];
            button28.Content = motorvalue[28];
            button29.Content = motorvalue[29];
            button30.Content = motorvalue[30];
            button31.Content = motorvalue[31];
            button32.Content = motorvalue[32];
            button33.Content = motorvalue[33];
            button34.Content = motorvalue[34];
            button35.Content = motorvalue[35];
            button36.Content = motorvalue[36];
            button37.Content = motorvalue[37];
            button38.Content = motorvalue[38];
            button39.Content = motorvalue[39];
            button40.Content = motorvalue[40];
            button41.Content = motorvalue[41];
            button42.Content = motorvalue[42];
            button43.Content = motorvalue[43];
            button44.Content = motorvalue[44];
            button45.Content = motorvalue[45];
            button46.Content = motorvalue[46];
            button47.Content = motorvalue[47];
          
        }

        public void pwmmotor(ref string motor)
        {
            try
            {
                if (motorvalue[int.Parse(motor)] == 0)
                {
                    motorvalue[int.Parse(motor)] = 1;
                }
                else if (motorvalue[int.Parse(motor)] == 1)
                {
                    motorvalue[int.Parse(motor)] = 2;
                }
                else if (motorvalue[int.Parse(motor)] == 2)
                {
                    motorvalue[int.Parse(motor)] = 3;
                }
                else if (motorvalue[int.Parse(motor)] == 3)
                {
                    motorvalue[int.Parse(motor)] = 4;
                }
                else if (motorvalue[int.Parse(motor)] == 4)
                {
                    motorvalue[int.Parse(motor)] = 5;
                }
                else if (motorvalue[int.Parse(motor)] == 5)
                {
                    motorvalue[int.Parse(motor)] = 6;
                }
                else if (motorvalue[int.Parse(motor)] == 6)
                {
                    motorvalue[int.Parse(motor)] = 7;
                }
                else if (motorvalue[int.Parse(motor)] == 7)
                {
                    motorvalue[int.Parse(motor)] = 8;
                }
                else if (motorvalue[int.Parse(motor)] == 8)
                {
                    motorvalue[int.Parse(motor)] = 9;
                }
                else if (motorvalue[int.Parse(motor)] == 9)
                {
                    motorvalue[int.Parse(motor)] = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


      

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (PortState1 == false) 
            {
                PortState1 = true;
            }
            else if (PortState1 == true)
            {
                PortState1 = false;
            }
        }

        public void StreamtoOtherProgram()
        {
            try
            {
                using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("testmap"))
                {
                    //right to left
                    motorvalue2[0] = motorvalue[44];
                    motorvalue2[1] = motorvalue[45];
                    motorvalue2[2] = motorvalue[46];
                    motorvalue2[3] = motorvalue[47];

                    motorvalue2[4] = motorvalue[40];
                    motorvalue2[5] = motorvalue[41];
                    motorvalue2[6] = motorvalue[42];
                    motorvalue2[7] = motorvalue[43];

                    motorvalue2[8] = motorvalue[36];
                    motorvalue2[9] = motorvalue[37];
                    motorvalue2[10] = motorvalue[38];
                    motorvalue2[11] = motorvalue[39];

                    motorvalue2[12] = motorvalue[32];
                    motorvalue2[13] = motorvalue[33];
                    motorvalue2[14] = motorvalue[34];
                    motorvalue2[15] = motorvalue[35];

                    motorvalue2[16] = motorvalue[28];
                    motorvalue2[17] = motorvalue[29];
                    motorvalue2[18] = motorvalue[30];
                    motorvalue2[19] = motorvalue[31];

                    motorvalue2[20] = motorvalue[24];
                    motorvalue2[21] = motorvalue[25];
                    motorvalue2[22] = motorvalue[26];
                    motorvalue2[23] = motorvalue[27];

                    motorvalue2[24] = motorvalue[20];
                    motorvalue2[25] = motorvalue[21];
                    motorvalue2[26] = motorvalue[22];
                    motorvalue2[27] = motorvalue[23];

                    motorvalue2[28] = motorvalue[16];
                    motorvalue2[29] = motorvalue[17];
                    motorvalue2[30] = motorvalue[18];
                    motorvalue2[31] = motorvalue[19];

                    motorvalue2[32] = motorvalue[12];
                    motorvalue2[33] = motorvalue[13];
                    motorvalue2[34] = motorvalue[14];
                    motorvalue2[35] = motorvalue[15];

                    motorvalue2[36] = motorvalue[8];
                    motorvalue2[37] = motorvalue[9];
                    motorvalue2[38] = motorvalue[10];
                    motorvalue2[39] = motorvalue[11];

                    motorvalue2[40] = motorvalue[4];
                    motorvalue2[41] = motorvalue[5];
                    motorvalue2[42] = motorvalue[6];
                    motorvalue2[43] = motorvalue[7];

                    motorvalue2[44] = motorvalue[0];
                    motorvalue2[45] = motorvalue[1];
                    motorvalue2[46] = motorvalue[2];
                    motorvalue2[47] = motorvalue[3];


                    Mutex mutex = Mutex.OpenExisting("testmapmutex");
                    int b = 0;
                    int c = 0;
                    int d = 0;
                    int f = 0;
                    int g = 0;
                    for (int a = 0; a < 48; a += 1)
                    {
                        if (motorvalue2[a] == 0)
                        {
                            b = 0;
                            c = 0;
                            d = 0;
                            f = 0;
                            g = 0;
                        }
                        else if (motorvalue2[a] == 1)
                        {
                            b = 1;
                            c = 0;
                            d = 0;
                            f = 0;
                            g = 0;
                        }
                        else if (motorvalue2[a] == 2)
                        {
                            b = 0;
                            c = 1;
                            d = 0;
                            f = 0;
                            g = 0;
                        }
                        else if (motorvalue2[a] == 3)
                        {
                            b = 1;
                            c = 1;
                            d = 0;
                            f = 0;
                            g = 0;
                        }
                        else if (motorvalue2[a] == 4)
                        {
                            b = 0;
                            c = 0;
                            d = 1;
                            f = 0;
                            g = 0;
                        }
                        else if (motorvalue2[a] == 5)
                        {
                            b = 1;
                            c = 0;
                            d = 1;
                            f = 0;
                            g = 0;
                        }
                        else if (motorvalue2[a] == 6)
                        {
                            b = 0;
                            c = 1;
                            d = 1;
                            f = 0;
                            g = 0;
                        }
                        else if (motorvalue2[a] == 7)
                        {
                            b = 1;
                            c = 1;
                            d = 1;
                            f = 0;
                            g = 0;
                        }
                        else if (motorvalue2[a] == 8)
                        {
                            b = 0;
                            c = 0;
                            d = 0;
                            f = 1;
                            g = 0;
                        }
                        else if (motorvalue2[a] == 9)
                        {
                            b = 1;
                            c = 0;
                            d = 0;
                            f = 1;
                            g = 0;
                        }
                        else if (motorvalue2[a] == 10)
                        {
                            b = 0;
                            c = 1;
                            d = 0;
                            f = 1;
                            g = 0;
                        }
                        else if (motorvalue2[a] == 11)
                        {
                            b = 1;
                            c = 1;
                            d = 0;
                            f = 1;
                            g = 0;
                        }
                        else if (motorvalue2[a] == 12)
                        {
                            b = 0;
                            c = 0;
                            d = 1;
                            f = 1;
                            g = 0;
                        }
                        else if (motorvalue2[a] == 13)
                        {
                            b = 1;
                            c = 0;
                            d = 1;
                            f = 1;
                            g = 0;
                        }
                        else if (motorvalue2[a] == 14)
                        {
                            b = 0;
                            c = 1;
                            d = 1;
                            f = 1;
                            g = 0;
                        }
                        else if (motorvalue2[a] == 15)
                        {
                            b = 1;
                            c = 1;
                            d = 1;
                            f = 1;
                            g = 0;
                        }
                        else if (motorvalue2[a] == 16)
                        {
                            b = 0;
                            c = 0;
                            d = 0;
                            f = 0;
                            g = 1;
                        }
                        
                        using (MemoryMappedViewStream stream = mmf.CreateViewStream(a * 5, 0))
                        {
                            BinaryWriter writer = new BinaryWriter(stream);

                            writer.Write(b);
                        }

                        using (MemoryMappedViewStream stream = mmf.CreateViewStream(a * 5 + 1, 0))
                        {
                            BinaryWriter writer = new BinaryWriter(stream);

                            writer.Write(c);
                        }

                        using (MemoryMappedViewStream stream = mmf.CreateViewStream(a * 5 + 2, 0))
                        {
                            BinaryWriter writer = new BinaryWriter(stream);

                            writer.Write(d);
                        }

                        using (MemoryMappedViewStream stream = mmf.CreateViewStream(a * 5 + 3, 0))
                        {
                            BinaryWriter writer = new BinaryWriter(stream);

                            writer.Write(f);
                        }
                        using (MemoryMappedViewStream stream = mmf.CreateViewStream(a * 5 + 4, 0))
                        {
                            BinaryWriter writer = new BinaryWriter(stream);

                            writer.Write(g);
                        }
                    }
                    //  mutex.ReleaseMutex();
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Memory-mapped file does not exist. Run Process A first.");
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string motor = "0";
            pwmmotor(ref motor);
            button1.Content = motorvalue[int.Parse(motor)];
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            string motor = "1";
            pwmmotor(ref motor);
            button3.Content = motorvalue[int.Parse(motor)];
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            string motor = "2";
            pwmmotor(ref motor);
            button4.Content = motorvalue[int.Parse(motor)];
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            string motor = "3";
            pwmmotor(ref motor);
            button5.Content = motorvalue[int.Parse(motor)];
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            string motor = "4";
            pwmmotor(ref motor);
            button6.Content = motorvalue[int.Parse(motor)];
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            string motor = "5";
            pwmmotor(ref motor);
            button7.Content = motorvalue[int.Parse(motor)];
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            string motor = "6";
            pwmmotor(ref motor);
            button8.Content = motorvalue[int.Parse(motor)];
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            string motor = "7";
            pwmmotor(ref motor);
            button9.Content = motorvalue[int.Parse(motor)];
        }

        private void button10_Click(object sender, RoutedEventArgs e)
        {
            string motor = "8";
            pwmmotor(ref motor);
            button10.Content = motorvalue[int.Parse(motor)];
        }

        private void button11_Click(object sender, RoutedEventArgs e)
        {
            string motor = "9";
            pwmmotor(ref motor);
            button11.Content = motorvalue[int.Parse(motor)];
        }

        private void button12_Click(object sender, RoutedEventArgs e)
        {
            string motor = "10";
            pwmmotor(ref motor);
            button12.Content = motorvalue[int.Parse(motor)];
        }

        private void button13_Click(object sender, RoutedEventArgs e)
        {
            string motor = "11";
            pwmmotor(ref motor);
            button13.Content = motorvalue[int.Parse(motor)];
        }

        private void button14_Click(object sender, RoutedEventArgs e)
        {
            string motor = "12";
            pwmmotor(ref motor);
            button14.Content = motorvalue[int.Parse(motor)];
        }

        private void button15_Click(object sender, RoutedEventArgs e)
        {
            string motor = "13";
            pwmmotor(ref motor);
            button15.Content = motorvalue[int.Parse(motor)];
        }

        private void button16_Click(object sender, RoutedEventArgs e)
        {
            string motor = "14";
            pwmmotor(ref motor);
            button16.Content = motorvalue[int.Parse(motor)];
        }

        private void button17_Click(object sender, RoutedEventArgs e)
        {
            string motor = "15";
            pwmmotor(ref motor);
            button17.Content = motorvalue[int.Parse(motor)];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (null == this.sensor)
            {
                return;
            }

            // create a png bitmap encoder which knows how to save a .png file
            BitmapEncoder encoder = new PngBitmapEncoder();

            // create frame from the writable bitmap and add to encoder
            encoder.Frames.Add(BitmapFrame.Create(this.colorBitmap));

            string time = System.DateTime.Now.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);

            string myPhotos = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            string path = Path.Combine(myPhotos, "KinectSnapshot-" + time + ".png");

            // write the new file to disk
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    encoder.Save(fs);
                }
            }
            catch (IOException)
            {
            }
        }





    }
}