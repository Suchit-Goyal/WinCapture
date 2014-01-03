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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Windows.Forms;
using gma.System.Windows;
using System.Threading;
using System.Windows.Interop;
using System.Runtime.InteropServices;
namespace Camshot
{
    /// <summary>
    /// Interaction logic for SpyWindow.xaml
    /// </summary>
    public partial class SpyWindow : Window
    {
        double a1 = SystemParameters.VirtualScreenWidth;
        double b1 = SystemParameters.VirtualScreenHeight;
        System.Windows.Media.Color backGroundColor1 = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFEFBC1C");
        Line line1 = new Line();
        Line line2 = new Line();
        BackWindow bw = new BackWindow();
        const int WM_COMMAND = 0x111;
        const int MAX_ALL = 419;
        const int MIN_ALL = 419;
        const int MAX_ALL_UNDO = 416;
        public static int x, y, temp, temp1;
  
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
      
        static  IntPtr HWND_BOTTOM = IntPtr.Zero;
        public IntPtr _hPreviousWindow;
        public IntPtr hWnd1;

        public SpyWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SpyWindow_Loaded);
           
            this.Cursor = System.Windows.Input.Cursors.Cross;
            line1.Visibility = System.Windows.Visibility.Visible;
            line1.StrokeThickness = 2.5;
            line1.Stroke = new SolidColorBrush(backGroundColor1);
            DoubleCollection dashes = new DoubleCollection();
            dashes.Add(3);
            dashes.Add(3);
          
            line1.StrokeDashCap = PenLineCap.Round;
           
            line2.Visibility = System.Windows.Visibility.Visible;
            line2.StrokeThickness = 2.5;
            line2.Stroke = new SolidColorBrush(backGroundColor1);
            DoubleCollection dashes2 = new DoubleCollection();
            dashes2.Add(3);
            dashes2.Add(3);
          
            line2.StrokeDashCap = PenLineCap.Round;
          
            this.Height = 0;
            this.Width = 0;
            this.ShowInTaskbar = false;
        }
       public  UserActivityHook ua1 ,ua;

        //Installs the global keyboard hooks.
        private void SpyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ua = new UserActivityHook();
            ua.OnMouseActivity += new System.Windows.Forms.MouseEventHandler(HookManager_MouseMove);
        }

        //Mouse movement global keyboard hooks.
        private void HookManager_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.OpnWin();
            if (e.Button == MouseButtons.Left)
            {
                CaptureTopWindow();
            }
        }
        bool IsWindowSendBack = false;

        //GetWindowText gets the process name of current handle;
        private string GetWindowText(IntPtr hWnd)
        {
            StringBuilder sb = new StringBuilder(256);
            Win32.GetWindowText(hWnd, sb, 256);
            return sb.ToString();
        }
        //GetWindowText gets the class name of current handle;
        private string GetClassName(IntPtr hWnd)
        {
            StringBuilder sb = new StringBuilder(256);
            Win32.GetClassName(hWnd, sb, 256);
            return sb.ToString();
        }

        public bool isMouseDown = false, isMouseUp = false;
        public static string format = "MMM_ddd_yyyy_HH-mm-ff";

        //@CaptureTopWindow captures the Foreground window
        //@Coordinates will be set for the current active window.
        private void CaptureTopWindow()
        {
            MainWindow m1 = System.Windows.Application.Current.MainWindow as MainWindow;
            string a = string.Format("{0}", hWnd1.ToInt32().ToString());
            int handle = int.Parse(a);
            try
            {
                Win32.Rect rc = new Win32.Rect();
                Win32.GetWindowRect(hWnd1, ref rc);
                if (m1.PicCapture.IsChecked)
                {
                    CaptureScreen2(rc.left, rc.top, rc.Width, rc.Height);
                }
                else
                    if (m1.VideoCapture.IsChecked)
                    {
                        ua.UninstallHooks();
                        WindowHighlighter.Refresh(_hPreviousWindow);
                        this.Close();
                        Thread.Sleep(5000);
                        Coordinates.x1 = rc.left;
                        Coordinates.y1 = rc.top;
                        Coordinates.x2 = rc.right;
                        Coordinates.y2 = rc.bottom;
                        Window3 w3 = new Window3();
                        MainWindow.w = w3;
                        w3.Show();
                            
                       
                    }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
               
            }
        }
        
        int h;

        //Copying/ capturing and saving the top window.It will then
        // return to the mainwindow after closing the current handle.
        public void CaptureScreen2(double x, double y, double width, double height)
        {
            MainWindow m1 = System.Windows.Application.Current.MainWindow as MainWindow;
            try
            {
                string path;
                DateTime saveNow = DateTime.Now;
                MainWindow.LoadBool();
                var k = App.Current as App;
                MainWindow.LoadBool();

                if (MainWindow.check_image == "False")
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\ImageCapture\" + saveNow.ToString(format) + ".png";
                    MainWindow.globalPath = path;
                }
                else
                {
                    MainWindow.LoadSettings();
                    path = MainWindow.temp_path_image + @"\" + saveNow.ToString(format) + ".png";
                  
                    MainWindow.globalPath = path;
                }

                int ix, iy, iw, ih, ig, iq;
                ix = Convert.ToInt32(x);
                iy = Convert.ToInt32(y);
                iw = Convert.ToInt32(width);
                ih = Convert.ToInt32(height);
                
                iq = Convert.ToInt32(h);
                System.Drawing.Bitmap image = new System.Drawing.Bitmap(iw, ih, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);

                ua.UninstallHooks();
                MainWindow.isReturn = false;

                MainWindow.isCaptureStarted = false;
                WindowHighlighter.Refresh(_hPreviousWindow);
               
                this.Close();
                g.CopyFromScreen(ix, iy, 0, 0, new System.Drawing.Size(iw, ih), System.Drawing.CopyPixelOperation.SourceCopy);
                BackWindow bw = new BackWindow();
                bw.Height = height;
                bw.Width = width;
                bw.Left = x;
                bw.Top = y;
                bw.Topmost = true;
                bw.Activate();
                bw.WindowStyle = System.Windows.WindowStyle.None;
                bw.Show();
                Thread.Sleep(200);
                bw.Hide();
                bw.Close();

                m1.Show();
                m1.SetWindow();
                
                image.Save(path, ImageFormat.Png);
             
                MainWindow.bmp = image;
               
                if (m1.OpenPaint.IsChecked && m1.SaveAs.IsChecked)
                {
                    MainWindow.openSaveAs();
                }
                else
                {
                    if (m1.OpenPaint.IsChecked)
                    {
                        MainWindow.open_image();
                    }
                    else
                        if (m1.SaveAs.IsChecked)
                        {
                            MainWindow.openSaveAs();
                        }
                }



            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }
        }

        //Called by mouse movement hook manager
        //Gets the handle of window under cursor position.
        public void OpnWin()
        {
            
            try
            {
                
                IntPtr hWnd = Win32.WindowFromPoint(System.Windows.Forms.Cursor.Position);
                hWnd1 = hWnd;
          
                if (_hPreviousWindow != IntPtr.Zero && _hPreviousWindow != hWnd)
                    WindowHighlighter.Refresh(_hPreviousWindow);

              
                if (hWnd == IntPtr.Zero)
                {
                    System.Windows.Forms.MessageBox.Show("No Window Found");

                }
                else
                {
                   
                    _hPreviousWindow = hWnd;
                    Win32.Rect rc = new Win32.Rect();
                    Win32.GetWindowRect(hWnd, ref rc);

              
                    WindowHighlighter.Highlight(hWnd);
                 
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
      //  Utilities.globalKeyboardHook gkh1 = new Utilities.globalKeyboardHook();
        
        //Send current handle to  back.
        void SendWpfWindowBack(Window window)
        {
         
            var hWnd = new WindowInteropHelper(window).Handle;
            SetWindowPos(hWnd,_hPreviousWindow, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
        }
        
    }
}
