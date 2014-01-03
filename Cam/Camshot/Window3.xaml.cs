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

using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.IO;

namespace Camshot
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        static bool isCanceled = false;
        int check = 0;
        public static Popup popup1 = new Popup {AllowsTransparency = true };
        public static Popup popup2 = new Popup { AllowsTransparency = true };
        public static Popup popup3 = new Popup { AllowsTransparency = true };
        menu m = new menu();
        AddComment ad = new AddComment();
        WaitWindow ww = new WaitWindow();
        public static bool isStop = false , isPause = false;
        public static System.Windows.Forms.NotifyIcon ni;
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            uint uFlags);

        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;

        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        double a1 = SystemParameters.VirtualScreenWidth;
        double b1 = SystemParameters.VirtualScreenHeight;
        double a = SystemParameters.WorkArea.Width;
        double b = SystemParameters.WorkArea.Height;
         bool menuOnTop =false;
       /// <summary>
       ///Window 3 initialization.
       /// </summary>
        public Window3()
        {
            InitializeComponent();
          //  hotkey();
          
            var k = App.Current as App;
            k.isPlaying = false;
            ShowUserControl();
            temp_record();
            ni  = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon("Icon.ico");
            ni.Visible = false;
            ni.Click += new EventHandler(ShowWindow);
            
        }
        static Timer t;
        static System.Windows.Media.Color backGroundColor2 = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFEFBC1C");
        SolidColorBrush yellow_brush = new SolidColorBrush(backGroundColor2);
        SolidColorBrush Red_brush = new SolidColorBrush(Colors.Red);
        int counter;
        bool isTimerStarted = false;
        public void InitializeTimer()
        {
            t = new Timer();
            counter = 0;
            t.Interval = 750;
            t.Enabled = true;
            timer1_Tick(null, null);

            t.Tick += new EventHandler(timer1_Tick);
            start_timer();
        }

        /// <summary>
        ///Start the timer tick.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void timer1_Tick(object sender, EventArgs e)
        {
           if((counter %2)==0 )
           {
               rectangle1.Stroke = yellow_brush;

            }
               else
               {
                   rectangle1.Stroke = Red_brush;
               }
           counter++;
        }
        /// <summary>
        ///Start the timer
        /// </summary>
        public void start_timer()
        {

            t.Start();
            isTimerStarted = true;
        }
        /// <summary>
        ///Stop the timer.
        /// </summary>
        public void stop_timer()
        {
            if (isTimerStarted == true)
            {
                rectangle1.Stroke = yellow_brush;
                t.Stop();
            }
        }
        /// <summary>
        ///Dispose the timer.
        /// </summary>
        public void dispose_timer()
        {
            if (isTimerStarted == true)
            {
                t.Dispose();
            }
        }
        public void ShowWindow(object sender, EventArgs e)
        {
            var k =App.Current as App;
            if (k.isPlaying == true)
            {
                isPause = true;
                changetoresumebutton();
                    RecordVideo.Pause();
                    stop_timer();
                ni.Visible = false;
                popup1.IsOpen = true;
                //CancelRecordingEvents();    
            }
        }
        public void temp_record()
        {
            this.WindowState = System.Windows.WindowState.Maximized; 
            this.ShowInTaskbar = false;
            DrawRectangle();
        }
        /// <summary>
        ///Use to Set the menu.xaml and WaitWindow.xaml
        /// </summary>
        public void ShowUserControl()
        {
    
            popup1.Child = m;
            popup1.IsOpen = true;
            popup2.Child = ww; 
            if (MainWindow.whichChecked == "fullscreen")
            {
                menuOnTop = true;
               popup2.VerticalOffset = (b1 / 2) -150;
                popup2.HorizontalOffset = (a1 / 2)-150;
                double tbHeight = SystemParameters.VirtualScreenHeight - SystemParameters.WorkArea.Height;
                popup1.HorizontalOffset = a1-5;
                popup1.VerticalOffset = SystemParameters.WorkArea.Height;
                RecordVideo.FullVideoRecord();
            }
        
      if (MainWindow.whichChecked == "topwindow")
            {

                RecordVideo.x = Coordinates.x1;
                RecordVideo.y = Coordinates.y1;
                RecordVideo.temp = Coordinates.x2;
                RecordVideo.temp1 = Coordinates.y2;
                int width = Coordinates.x2 - Coordinates.x1;
                int height = Coordinates.y2 - Coordinates.y1;
                popup2.HorizontalOffset = width / 2 + Coordinates.x1 - 150;
                popup2.VerticalOffset = height / 2 + Coordinates.y1 - 50;
                int b = Coordinates.x2;
                int h = Coordinates.y2;
                if (134 >
                   (Screen.PrimaryScreen.WorkingArea.Width - b)
                   && m.Height > (Screen.PrimaryScreen.WorkingArea.Height - h))
                {
                    menuOnTop = true;
                    popup1.HorizontalOffset = b-134;
                    popup1.VerticalOffset = h - m.Height;
                    //     RecordVideo.VideoRecord();
                }
                else
                    if ((Screen.PrimaryScreen.WorkingArea.Width - b) > 134 && (Screen.PrimaryScreen.WorkingArea.Height - h) > m.Height)
                    {
                        popup1.HorizontalOffset = b - 150;
                        popup1.VerticalOffset = h + 1;
                        //  RecordVideo.VideoRecord();
                    }
                    else
                        if (134 <
                    (Screen.PrimaryScreen.WorkingArea.Width - b)
                    && m.Height > (Screen.PrimaryScreen.WorkingArea.Height - h))
                        {
                            popup1.HorizontalOffset = b;
                            popup1.VerticalOffset = Screen.PrimaryScreen.WorkingArea.Height;
                            //     RecordVideo.VideoRecord();
                        }
                        else
                            if (134 >
                 (Screen.PrimaryScreen.WorkingArea.Width - b)
                 && m.Height < (Screen.PrimaryScreen.WorkingArea.Height - h))
                            {
                                popup1.HorizontalOffset = b - 150;
                                popup1.VerticalOffset = h;
                                // RecordVideo.VideoRecord();
                            }
                RecordVideo.VideoRecord();
            }
            if (MainWindow.whichChecked == "customscreen" )
            {
                RecordVideo.x = Coordinates.x1;
                RecordVideo.y = Coordinates.y1;
                RecordVideo.temp = Coordinates.x2;
                RecordVideo.temp1 = Coordinates.y2;
                int width = Coordinates.x2 - Coordinates.x1;
                int height = Coordinates.y2 - Coordinates.y1;
                popup2.HorizontalOffset = width / 2 + Coordinates.x1 - 150;
                popup2.VerticalOffset = height / 2 + Coordinates.y1 - 50;
                int b = Coordinates.x2;
                int h = Coordinates.y2;
                if (134 > 
                    (Screen.PrimaryScreen.WorkingArea.Width -b) 
                    && m.Height > (Screen.PrimaryScreen.WorkingArea.Height-h))
                {
                    menuOnTop = true;
                    popup1.HorizontalOffset = b - 134;
                    popup1.VerticalOffset =h -m.Height;
              //      System.Windows.MessageBox.Show(Screen.PrimaryScreen.WorkingArea.Height.ToString());
               //     RecordVideo.VideoRecord();
                }
                else
                    if ((Screen.PrimaryScreen.WorkingArea.Width - b) > 134 && (Screen.PrimaryScreen.WorkingArea.Height - h) > m.Height)
                    {
                        popup1.HorizontalOffset = b - 150;
                        popup1.VerticalOffset = h +1;
                      //  RecordVideo.VideoRecord();
                    }
                    else
                        if (134 <
                    (Screen.PrimaryScreen.WorkingArea.Width - b)
                    && m.Height > (Screen.PrimaryScreen.WorkingArea.Height - h))
                        {
                            popup1.HorizontalOffset = b ;
                            popup1.VerticalOffset = Screen.PrimaryScreen.WorkingArea.Height ;
                      
                        }
                else
                            if (134 >
                 (Screen.PrimaryScreen.WorkingArea.Width - b)
                 && m.Height < (Screen.PrimaryScreen.WorkingArea.Height - h))
                            {
                                popup1.HorizontalOffset = b-150;
                                popup1.VerticalOffset = h;
                        
                            }
                RecordVideo.VideoRecord();
            }
            StartRecording();
          
        }
        /// <summary>
        ///Change the @image1 to pause.
        /// </summary>
        public void changetopausebutton()
        {
            m.image1.Source = new BitmapImage(new Uri("/Camshot;component/Images/pause-btn.png", UriKind.Relative));
            m.image1.ToolTip = "'F5' to Pause Recording";
        }
        /// <summary>
        ///Change the @image2 to resume.
        /// </summary>
        public void changetoresumebutton()
        {
            m.image1.Source = new BitmapImage(new Uri("/Camshot;component/Images/play-btn.png", UriKind.Relative));
            m.image1.ToolTip = "'F5' to Resume Recording";
            m.image1.Visibility = System.Windows.Visibility.Visible;
        }
        /// <summary>
        ///Disable the stop button  till it is started..
        /// </summary>
        public void disableStop()
        {
            m.image2.IsEnabled = false;
        }
        /// <summary>
        ///Enable the stop button.
        /// </summary>
        public void enableStop()
        {

            m.image2.IsEnabled = true;
        }
        public void ShowTrayIcon()
        {
            if (menuOnTop == true)
            {
                ni.Visible = true;
                popup1.IsOpen = false;
                
            }

        }
    /// <summary>
    ///Change close button hover image
    /// </summary>
        public void changeCloseImage()
        {
            m.image3.Source = new BitmapImage(new Uri("/Camshot;component/Images/close-btn_hover.png", UriKind.Relative));
      
        }
       /// <summary>
        ///This function is called to start/stop/pause/resume the video recording.
        ///@image1 = start/pause/resume
        ///@image2 = Stop
        ///@image3 = Cancelling/Resuming the recording.It will open a WaitWindow modal. 
       /// </summary>
        public void StartRecording()
        {
           var k = App.Current as App;
            MainWindow main = System.Windows.Application.Current.MainWindow as MainWindow;
       
            m.image1.MouseDown += (s, args) =>
            {
                if (k.isPlaying == false)
                {
                    
                    enableStop();
                    changetopausebutton();
                    ShowTrayIcon();     
                    k.isPlaying = true;
                    RecordVideo.Start();
                    InitializeTimer();
                }
                else
                {
                    if (isPause == false)
                    {
                        popup1.IsOpen = true;
                        ni.Visible = false;
                        isPause = true;
                        changetoresumebutton();
                        RecordVideo.Pause();
                        stop_timer();
                    }
                    else
                    {
                        ShowTrayIcon();
                        changetopausebutton();
                        isPause = false;
                        RecordVideo.Resume();
                        start_timer();
                    }
                }
                };
        
            m.image2.MouseDown += (s, args) =>
            {
                RecordVideo.Stop();
                stop_timer();
                dispose_timer();
                this.Close();
                ni.Visible = false;
                popup1.IsOpen = false;
                isStop = true;
               
                if (main.OpenMediaPlayer.IsChecked && main.SaveAs.IsChecked)
                {
                    MainWindow.openSaveAs();
                }
                else
                {
                    if (main.OpenMediaPlayer.IsChecked)
                    {
                        MainWindow.open_media();
                    }
                    else
                        if (main.SaveAs.IsChecked)
                        {
                            MainWindow.openSaveAs();
                        }
                }
             
                main.SetWindow();
                MainWindow.isReturn = false;
                MainWindow.isCaptureStarted = false;
            };
            m.image3.MouseLeftButtonDown += (s, args) =>
            {
                changeCloseImage();
                if (k.isPlaying == true)
                {
                    if (isPause == false)
                    {
                        RecordVideo.Pause();
                        stop_timer();
                    }
                 
                    ni.Visible = false;
                    
                    popup2.IsOpen = true;
                    popup1.IsOpen = false;
                    CancelRecordingEvents();    
                }
                else
                {
                    
                    k.isPlaying = false;
             
                    stop_timer(); 
                    dispose_timer();
                    this.Close();
                    popup1.IsOpen = false;
                    RecordVideo.DesktopEncoder.Reset();
                    MainWindow m1 = System.Windows.Application.Current.MainWindow as MainWindow;
                    m1.Show();
                    m1.SetWindow();
                    MainWindow.isReturn = false;
                    MainWindow.isCaptureStarted = false;
                    
                }

            };
           

        }
     /// <summary>
     ///Gets the input from modal for cancelling or resuming the current recording.
     /// </summary>
        public void CancelRecordingEvents()
        {
            var k = App.Current as App;
            MainWindow main = System.Windows.Application.Current.MainWindow as MainWindow;
            ww.button1.Click += (s, args) =>
                {
                    ShowTrayIcon();
                    popup2.IsOpen = false;
                   
                    changetopausebutton();
                    isPause = false;
                    RecordVideo.Resume();
                    start_timer();
                    //main.Show();
                };
            ww.button2.Click +=(s,args) =>
            {
               
                k.isPlaying = false;
                isCanceled = true;
                RecordVideo.StopCancelRecording();
                RecordVideo.DesktopEncoder.Reset();
                stop_timer();
                dispose_timer();
                this.Close();
                File.Delete(MainWindow.globalVideoPath);
                popup2.IsOpen = false;
                main.Show();
                main.SetWindow();
                MainWindow.isReturn = false;
                MainWindow.isCaptureStarted = false;
                ni.Visible = false;
            };
        }
        System.Windows.Shapes.Rectangle rectangle1;

        private void Drawtextbox()
        {
            popup3.Child = ad;
            popup3.IsOpen = true;
            System.Windows.Controls.TextBox t1 = new System.Windows.Controls.TextBox();
            popup3.HorizontalOffset = Coordinates.x1 + 500;
            popup3.VerticalOffset = Coordinates.y1 + 300;
                
        }

      /// <summary>
      ///Draws the canvas rectangle on top of the current window.
      /// </summary>
        public void DrawRectangle()
        {
            System.Windows.Media.Color backGroundColor1 = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFEFBC1C");
            rectangle1 = new System.Windows.Shapes.Rectangle();
            SolidColorBrush brush = new SolidColorBrush(backGroundColor1);
            rectangle1.Stroke = brush;
            rectangle1.Fill = new SolidColorBrush(Colors.Transparent);
            rectangle1.StrokeThickness = 3;
            if (MainWindow.whichChecked == "topwindow" )
            {
                this.Topmost = true;
                rectangle1.Width = Coordinates.x2 - Coordinates.x1 + 9;
                rectangle1.Height = Coordinates.y2 - Coordinates.y1 + 9;
                mycanvas.Children.Clear();
                Canvas.SetLeft(rectangle1, Coordinates.x1 + 3);
                Canvas.SetTop(rectangle1, Coordinates.y1 + 3);
                mycanvas.Children.Add(rectangle1);
            }
            else
            {
                if (MainWindow.whichChecked == "customscreen")
                {
                    this.Topmost = true;
                    rectangle1.Width = RecordVideo.temp - RecordVideo.x +12;
                    rectangle1.Height = RecordVideo.temp1 - RecordVideo.y+9;
                    mycanvas.Children.Clear();
                    Canvas.SetLeft(rectangle1, RecordVideo.x);
                    Canvas.SetTop(rectangle1, RecordVideo.y +3);
                    mycanvas.Children.Add(rectangle1);
                }
                else
                    if (MainWindow.whichChecked == "fullscreen")
                    {
                        this.Topmost = true;
                        rectangle1.Width = Screen.PrimaryScreen.Bounds.Width+12;
                        rectangle1.Height = Screen.PrimaryScreen.Bounds.Height+12;
                        mycanvas.Children.Clear();
                        Canvas.SetLeft(rectangle1, -1);
                        Canvas.SetTop(rectangle1, -1);
                        mycanvas.Children.Add(rectangle1);
                    }
            }  
        }
        static void SendWpfWindowBack(Window window)
        {
            var hWnd = new WindowInteropHelper(window).Handle;
            SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
        }
      /// <summary>
      ///Called by global hooks from MainWindow for starting/stoping/resuming the recording // F5 to start recording.
      /// </summary>
        public void StartRecording2()
        {
            try
            {
                var k = App.Current as App;
                if (k.isPlaying == false)
                {

                    enableStop();
                    changetopausebutton();
                    ni.Visible = true;
                    k.isPlaying = true;
                    popup1.IsOpen = false;
                    System.Threading.Thread.Sleep(2000);
                    RecordVideo.Start();

                    InitializeTimer();

                }
                else
                {
                    if (isPause == false)
                    {
                        popup1.IsOpen = true;
                        ni.Visible = false;
                        isPause = true;
                        changetoresumebutton();
                        RecordVideo.Pause();
                        stop_timer();
                    }
                    else
                    {
                        popup1.IsOpen = false;
                        ni.Visible = true;
                        changetopausebutton();
                        isPause = false;
                        RecordVideo.Resume();
                        start_timer();
                    }
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }
            

        }
        /// <summary>
        ///Called by global hooks from MainWindow for stoping the recording // F6 to stop recording.
        /// </summary>
        public void StopRecording2()
        {
            try
            {

                stop_timer();
                dispose_timer();
                var k = App.Current as App;
                MainWindow main = System.Windows.Application.Current.MainWindow as MainWindow;
                if (k.isPlaying == true)
                {
                    System.Threading.Thread.Sleep(2000);
                    RecordVideo.Stop();
                }
                this.Close();
                ni.Visible = false;
                popup1.IsOpen = false;
                isStop = true;
                k.isPlaying = false;

                if (main.OpenMediaPlayer.IsChecked && main.SaveAs.IsChecked)
                {
                    MainWindow.openSaveAs();
                }
                else
                {
                    if (main.OpenMediaPlayer.IsChecked)
                    {
                        MainWindow.open_media();
                    }
                    else
                        if (main.SaveAs.IsChecked)
                        {
                            MainWindow.openSaveAs();
                        }
                }

                main.SetWindow();
                MainWindow.isReturn = false;
                MainWindow.isCaptureStarted = false;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }
        }
        /// <summary>
        /// Close the Window3 and returns to mainwindow.
        /// </summary>
        public void CloseWindow()
        {
            var k = App.Current as App;
            if(k.isPlaying ==false)
            {
            this.Close();
            if (RecordVideo.DesktopEncoder.RunState == WMEncoderLib.WMENC_ENCODER_STATE.WMENC_ENCODER_RUNNING)
            {
                RecordVideo.StopCancelRecording();
            }
            MainWindow main = System.Windows.Application.Current.MainWindow as MainWindow;
            isCanceled = true;
            popup1.IsOpen = false;
            
            stop_timer();
            dispose_timer();
         
            File.Delete(MainWindow.globalVideoPath);
            popup2.IsOpen = false;
            main.Show();
            main.SetWindow();
            MainWindow.isReturn = false;
            MainWindow.isCaptureStarted = false;
            ni.Visible = false; 
            }
        }
       /// <summary>
       ///Close the current window.
       /// </summary>
        public void Escape()
        {
            this.Close();
            popup1.IsOpen = false;
        }
        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
          
        }
       
    }
}
