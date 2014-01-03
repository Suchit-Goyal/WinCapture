/*

 * Copyright (c) 2013-2014 by ThinkSys Software Pvt. Ltd. .  ALL RIGHTS RESERVED.

 * Consult your license regarding permissions and restrictions.

 */



/*

 * This file is part of software bearing the following

 * restrictions:

 *

 * Copyright (c) 2013

 * ThinkSys Software Pvt. Ltd.

 *

 * Permission to use, copy, modify, distribute and sell this

 * software and its documentation for any purpose is hereby

 * granted with fee, provided that the above copyright notice

 * appear in all copies and that both that copyright notice and

 * this permission notice appear in supporting documentation.

 *  ThinkSys Software Pvt. Ltd. Company makes no representations about the

 * suitability of this software for any purpose. It is provided

 * "as is" without express or implied warranty.

 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using WMEncoderLib;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Windows.Controls.Primitives;
using System.Threading;
using System.Windows;


namespace Camshot
{
    public class RecordVideo
    {

        public static Popup popup2 = new Popup();
        public static string format = "MMM_ddd_yyyy_HH-mm-ff";
        private static IntPtr _previousHandle = IntPtr.Zero;
        private static IntPtr _previousToLastHandle = IntPtr.Zero;
        
        public static string strshowUI;
        public static DateTime recordStarttime;

        public static int x, y, temp, temp1;

        public static WMEncoderApp DesktopEncoderAppln = new WMEncoderApp();
        public static IWMEncoder DesktopEncoder = DesktopEncoderAppln.Encoder;
        public static IWMEncStatistics broadCastStats;
     
        [DllImport("user32.dll")]
            static extern IntPtr GetForegroundWindow();

        //Set the previous window active.
        private static void setLastActive()
        {
            //          System.Windows.Forms.MessageBox.Show("Reach Here");
            IntPtr currentHandle = GetForegroundWindow();
            if (currentHandle != _previousHandle)
            {

                _previousToLastHandle = _previousHandle;
                _previousHandle = currentHandle;
            }
        }
  

        //Start the custom and top window recording.
        public static void VideoRecord()
        {
            DesktopEncoder.Reset();
            IWMEncProfile SelProfile;
            IWMEncSource AudioSrc;
          
        //    DesktopEncoderAppln = new WMEncoderApp();
            try
            {
                if (DesktopEncoder != null)
                {
                    if (DesktopEncoder.RunState == WMENC_ENCODER_STATE.WMENC_ENCODER_PAUSED)
                    {
                        DesktopEncoder.Start();
                        return;
                    }
                }
               
                IWMEncSourceGroupCollection SrcGroupCollection = DesktopEncoder.SourceGroupCollection;
                IWMEncSourceGroup SrcGroup = SrcGroupCollection.Add("SG_2");
                IWMEncVideoSource VideoSrc = (IWMEncVideoSource)SrcGroup.AddSource(WMENC_SOURCE_TYPE.WMENC_VIDEO);
                VideoSrc.SetInput("ScreenCapture1", "ScreenCap", "");
               
                double x_bottom = Screen.PrimaryScreen.Bounds.Width - temp;
                double y_bottom = Screen.PrimaryScreen.Bounds.Height - temp1;
                if (x_bottom < 0)
                {
                    x_bottom = -x_bottom;
                }
                if (y_bottom < 0)
                {
                    y_bottom = -y_bottom;
                }
                if (x < 0)
                {
                    x = -x;
                }
                if (y < 0)
                {
                    y = -y;
                }
                float g = (float)Screen.PrimaryScreen.Bounds.Width / 320;
                float k = (float)Screen.PrimaryScreen.Bounds.Height / 240;
                VideoSrc.CroppingMode = WMENC_CROPPING_MODE.WMENC_CROPPING_ABSOLUTE;
                VideoSrc.CroppingBottomMargin = Convert.ToInt32(y_bottom / k);
                VideoSrc.CroppingLeftMargin = Convert.ToInt32(x / g);
                VideoSrc.CroppingRightMargin = Convert.ToInt32(x_bottom / g);
                VideoSrc.CroppingTopMargin = Convert.ToInt32(y / k);

                IWMEncProfileCollection ProfileCollection = DesktopEncoder.ProfileCollection;
                ProfileCollection = DesktopEncoder.ProfileCollection;
                SelProfile = ProfileCollection.Item(28);
                SrcGroup.set_Profile((IWMEncProfile)SelProfile);
                IWMEncFile inputFile = DesktopEncoder.File;
                DateTime saveNow = DateTime.Now;
                string path;

                MainWindow.LoadBool();
            
                if (MainWindow.check_video == "False")
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\VideoCapture\" + saveNow.ToString(format) + MainWindow.outputformat;
                    MainWindow.globalVideoPath = path;
                }

                else
                {
                    MainWindow.LoadSettings();
                    path = MainWindow.temp_path_video + @"\" + saveNow.ToString(format) + MainWindow.outputformat;
                    MainWindow.globalVideoPath = path;
                }
                inputFile.LocalFileName = path;
                DesktopEncoder.PrepareToEncode(true);

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
      //Start the full screen recording.
        public static void FullVideoRecord()
        {
            DesktopEncoder.Reset();
            IWMEncProfile SelProfile;
            IWMEncSource AudioSrc;
            //DesktopEncoderAppln = new WMEncoderApp();
            try
            {
                if (DesktopEncoder != null)
                {
                    if (DesktopEncoder.RunState == WMENC_ENCODER_STATE.WMENC_ENCODER_PAUSED)
                    {
                        DesktopEncoder.Start();
                        return;
                    }
                }
                //     setLastActive();
                //         VideoActiveWindow.Capture(VideoActiveWindow.CaptureMode.Window);

             
         //       DesktopEncoder = DesktopEncoderAppln.Encoder;
                IWMEncSourceGroupCollection SrcGroupCollection = DesktopEncoder.SourceGroupCollection;
                IWMEncSourceGroup SrcGroup = SrcGroupCollection.Add("SG_2");
                IWMEncVideoSource VideoSrc = (IWMEncVideoSource)SrcGroup.AddSource(WMENC_SOURCE_TYPE.WMENC_VIDEO);
                VideoSrc.SetInput("ScreenCapture1", "ScreenCap", "");
                
                IWMEncProfileCollection ProfileCollection = DesktopEncoder.ProfileCollection;
                ProfileCollection = DesktopEncoder.ProfileCollection;
              
                //for (int i = 0; i <= lLength - 1; i++)
                //{
                //    SelProfile = ProfileCollection.Item(i);
                //    if (SelProfile.Name == "Screen Video/Audio High (CBR)")
                //    {
                //        SrcGroup.set_Profile((IWMEncProfile)SelProfile);
                //        break;
                //    }
                //}
                SelProfile = ProfileCollection.Item(28);

                SrcGroup.set_Profile((IWMEncProfile)SelProfile);
                        
                IWMEncFile inputFile = DesktopEncoder.File;
                DateTime saveNow = DateTime.Now;
                string path;

                MainWindow.LoadBool();
                if (MainWindow.check_video == "False")
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\VideoCapture\" + saveNow.ToString(format) + MainWindow.outputformat;
                    MainWindow.globalVideoPath = path;
                }

                else
                {
                    MainWindow.LoadSettings();
                    path = MainWindow.temp_path_video + @"\" + saveNow.ToString(format) + MainWindow.outputformat;
                    MainWindow.globalVideoPath = path;
                }
                //      Thread.Sleep(2000);
                inputFile.LocalFileName = path;
                DesktopEncoder.PrepareToEncode(true);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        //Stop the recording
        public static void Stop()
        {
                DesktopEncoder.Stop();
                Thread.Sleep(2000);
      
               
                
        }
        //Stop the recording
        public static void StopCancelRecording()
        {
             DesktopEncoder.Stop();
                Thread.Sleep(2000);
              
        }
        //Pause the recording
        public static void PauseVideo()
        {
            if (DesktopEncoder != null)
            {
                DesktopEncoder.Pause();
            }
        }
        //Stop the recording
        public static void Pause()
        {
            DesktopEncoder.Pause();
        }
        //Resume paused  recording
        public static void Resume()
        {
          
            DesktopEncoder.Start();
        }
        //Start the recording
        public static void Start()
        {
           
            DesktopEncoder.Start();
        }
        public static void ReleaseResources()
        {
            try
            {
                #region Release com objects
                //if (inputFile != null)
                //    Marshal.FinalReleaseComObject(inputFile);
                //if (SelProfile != null)
                //    Marshal.FinalReleaseComObject(SelProfile);

                //if (SrcGroup != null)
                //    Marshal.FinalReleaseComObject(SrcGroup);
                //if (SrcGroupCollection != null)
                //    Marshal.FinalReleaseComObject(SrcGroupCollection);
                //if (ProfileCollection != null)
                //    Marshal.FinalReleaseComObject(ProfileCollection);
                if (DesktopEncoder != null)
                    Marshal.FinalReleaseComObject(DesktopEncoder);
                // GC collect is explicitly called because of a memory leak issue of WMEncoder.
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                #endregion
            }
            catch { }
        }
    }
}

 