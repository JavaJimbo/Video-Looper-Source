/*******************************************************************************************
 *  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
 *  Adapted from Emgu CameraCapture example
 *  and Device - CDC - Basic Demo / PC Software Example for C#
 *  from Microchip Solutions V2013-06-05

 *  4-19-16: CDC serial port working very well with 4096 byte transfers & webcam very smooth
 *  5-20-16: Works beautifully with PIC32 LED COntroller CDC
 *  Strange offset error seems to be occuring. Otherwise works great.
 *  Fixed offset bug. No compression in this version
 *  For four panels.
 *  6-2-16: Incoming camera image is 423 x 270. Crop it to 64 x 64.
 *  Copy image to 128 x 128 matrix
 *  6-3-16: Added serial port # to Panel class. Added 32x32 matrix capability.
 *  Works great with sixteen 16x32 panels and one 32x32 panel.
 *  6-4-16: Rotation works. Attempted to center cam image with cropping.
 *  6-7-16: Basic video works great with three serial ports.
 *  6-12-16: Added USE_SERIAL and USE_C25 compiler options
 *           Added cropping, enlarging modified image.
 *  6-27-16: Got all panels working with six serial ports. Using both 16x32 and 32x32 panels.
 *           Transmitting one byte grayscale.
 *  6-28-16: One byte color works great!
 *  10-10-16: Replaying works very nicely.
 *  10-11-16: All features working.
 *  10-12-16: Use Ghosthrax.avi for video VIDEO_FILENAME.
 *  10-21-16: Fixed Ghostrax bug that caused ghost to get cut off on bottom of screen.
 *              Also did general cleanup on MSI PC.
 ********************************************************************************************/
#define PLAY_VIDEO_FILE
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using System.IO.Ports;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.VideoSurveillance;

namespace VideoLooper
{
    public partial class VideoLooperForm : Form
    {        
        Timer My_Timer = new Timer();
        int FPS = 30;
        int formWidth = 0;
        int formHeight = 0;
        public const string VIDEO_FILENAME = "C:\\VideoLooper\\Ghosthrax.avi";
        public const string COM_PORT_NAME = "COM7";

        Capture _playVideo = null;
        bool startupFlag = true;
        // int frameCounter = 30;

        public VideoLooperForm()
        {            
            InitializeComponent();
            Cursor.Hide();
            try
            {
                // Frame Rate                
                My_Timer.Interval = 1000 / FPS;
                My_Timer.Tick += new EventHandler(My_Timer_Tick);
                My_Timer.Start();
                _playVideo = new Capture(VIDEO_FILENAME);
               GoFullscreen(true);                
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }

        private void My_Timer_Tick(object sender, EventArgs e)
        {
            Mat frame = new Mat();            
            bool serialPortError = false;


            frame = _playVideo.QueryFrame();    // Get next frame of video

            if (frame == null) _playVideo = new Capture(VIDEO_FILENAME);   

            if (startupFlag == true || frame == null)
            {
                try
                {
                    serialPort1.PortName = COM_PORT_NAME;
                    serialPort1.BaudRate = 9600;
                    serialPort1.Parity = 0;
                    serialPort1.DataBits = 8;
                    serialPort1.StopBits = System.IO.Ports.StopBits.One;
                    serialPort1.ReadTimeout = 100;
                    serialPort1.Open();                  
                    
                }
                catch
                {                    
                    serialPortError = true;
                    My_Timer.Stop();
                    GoFullscreen(false);
                    if (startupFlag == false) Process.Start("shutdown", "/s /t 0");
                    System.Windows.Forms.Application.Exit();
                    MessageBox.Show("Port error: can't open COM PORT");
                }

                if (serialPortError == false)
                {
                    try
                    {
                        byte[] outPacket = new byte[1];
                        outPacket[0] = (byte)'X';
                        serialPort1.Write(outPacket, 0, 1);
                    }
                    catch
                    {                        
                        serialPortError = true;
                        MessageBox.Show("Write error");
                        serialPort1.DiscardInBuffer();
                        serialPort1.Close();
                        My_Timer.Stop();
                        GoFullscreen(false);
                        if (startupFlag == false) Process.Start("shutdown", "/s /t 0");
                        System.Windows.Forms.Application.Exit();                        
                    }
                }

                if (serialPortError == false)
                {
                    try
                    {
                        byte[] inPacket = new byte[8];
                        inPacket[0] = (byte)'\0';
                        serialPort1.Read(inPacket, 0, 1);
                        serialPort1.DiscardInBuffer();
                        serialPort1.Close();
                    }
                    catch
                    {
                        serialPortError = true;
                        // MessageBox.Show("Read Error");
                        serialPort1.DiscardInBuffer();
                        serialPort1.Close();
                        My_Timer.Stop();
                        GoFullscreen(false);
                        if (startupFlag == false) Process.Start("shutdown", "/s /t 0");
                        System.Windows.Forms.Application.Exit();
                    }
                }
            }

            
            if (frame != null && serialPortError == false)
            {
                Image<Bgr, Byte> imgOrg = frame.ToImage<Bgr, Byte>();
                if (VideoLooperForm.ActiveForm != null)
                {                    
                    formWidth = VideoLooperForm.ActiveForm.Width;
                    formHeight = VideoLooperForm.ActiveForm.Height;
                    Image<Bgr, byte> cpimg = imgOrg.Resize(formWidth, formHeight, Emgu.CV.CvEnum.Inter.Linear);//this is image with resize  Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR
                    captureImageBox.Image = cpimg;
                }
                else captureImageBox.Image = imgOrg;
            }           
            startupFlag = false;
        }

        

        private void GoFullscreen(bool fullscreen)
        {
            if (fullscreen)
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Bounds = Screen.PrimaryScreen.Bounds;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            }
        }

        private void captureImageBox_Click(object sender, EventArgs e)
        {

        }
    }
}
