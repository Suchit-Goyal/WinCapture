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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Timers;
using System.Windows.Interop;


namespace Camshot
{
    public partial class Video_Wind1 : Window
    {
        public double x, g, h;
        public static string format = "MMM_ddd_yyyy_HH-mm-ff";
        public double y;
        public double width;
        public double height;
        public bool isMouseDown = false, isMouseUp = false;
        ConsoleKeyInfo cki;
        string path;
        MainWindow main = System.Windows.Application.Current.MainWindow as MainWindow;
        public static bool isPicChecked = true;
        private System.Timers.Timer ClickTimer;
        private int ClickCounter;
        double a1 = SystemParameters.VirtualScreenWidth;
        double b1 = SystemParameters.VirtualScreenHeight;
        System.Windows.Media.Color backGroundColor1 = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFEFBC1C");
        Line line1 = new Line();
        Line line2 = new Line();
        Utilities.globalKeyboardHook gkh1 = new Utilities.globalKeyboardHook();
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
        double rectWidth = 31, rectHeight = 31;
        System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
        public Video_Wind1()
        {
            InitializeComponent();
            ClickCounter = 0;
            this.ShowInTaskbar = false;
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
            canvas1.Children.Add(line1);
            canvas1.Children.Add(line2);
            this.Cursor = System.Windows.Input.Cursors.Cross;
            rect.Width = 31;
            rect.Height = 31;
        }


        //ShowWindow3() function displays MainWindow.xaml and exits current handle
        public void ShowWindow3()
        {
            MainWindow m1 = System.Windows.Application.Current.MainWindow as MainWindow;
            Window3 w3 = new Window3();
            MainWindow.w = w3;
            w3.Show();
        }
        private void EvaluateClicks(object source, ElapsedEventArgs e)
        {
            ClickTimer.Stop();
            ClickCounter = 0;
        }
        static void SendWpfWindowBack(Window window)
        {
            var hWnd = new WindowInteropHelper(window).Handle;
            SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
        }
        bool IsWindowSendBack = false;

        //Mouse Movement Event Handler  
       
       
        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            double curx1 = e.GetPosition(null).X;
            double cury1 = e.GetPosition(null).Y;
            line1.X1 = curx1;
            line1.X2 = curx1;
            line1.Y1 = 0;
            line1.Y2 = b1 + 10;
            line2.X1 = 0;
            line2.X2 = a1 + 10;
            line2.Y1 = cury1;
            line2.Y2 = cury1;
            if (this.isMouseDown)
            {

               
                    double curx = e.GetPosition(null).X;
                    double cury = e.GetPosition(null).Y;
                    line1.X1 = curx;
                    line1.X2 = curx;
                    line1.Y1 = 0;
                    line1.Y2 = b1 + 10;

                    line2.X1 = 0;
                    line2.X2 = a1 + 10;
                    line2.Y1 = cury;
                    line2.Y2 = cury;

                    if (curx < x && cury < y)
                    {
                        if (check == false)
                        {
                            System.Windows.Shapes.Rectangle r = new System.Windows.Shapes.Rectangle();
                            SolidColorBrush brush = new SolidColorBrush(backGroundColor1);
                            r.Stroke = brush;
                            r.Fill = new SolidColorBrush(Colors.Transparent);
                            r.StrokeThickness = 2;
                            r.Width = Math.Abs(x - curx + 5);
                            r.Height = Math.Abs(y - cury + 5);
                            if (r.Width > 30 && r.Height > 30)
                            {
                                rect.Width = r.Width;
                                rect.Height = r.Height;
                                check = true;
                            }
                            cnv.Children.Clear();
                            cnv.Children.Add(r);
                            Canvas.SetRight(r, a1 - x + 10);
                            Canvas.SetBottom(r, b1 - y + 10);

                            if (e.LeftButton == MouseButtonState.Released)
                            {
                                double curx2 = e.GetPosition(null).X;
                                double cury2 = e.GetPosition(null).Y;
                                if ((x - curx2) < 5 && (y - cury2) < 5)
                                {
                                    x = 0;
                                    y = 0;
                                    g = SystemParameters.WorkArea.Width;

                                    h = SystemParameters.WorkArea.Height;
                                    width = x - g;
                                    height = y - h;

                                    RecordVideo.x = Convert.ToInt32(g);
                                    RecordVideo.y = Convert.ToInt32(y);
                                    RecordVideo.temp = Convert.ToInt32(x + width);
                                    RecordVideo.temp1 = Convert.ToInt32(y + height);
                                    ShowWindow3();
                                    this.Close();
                                }

                                else
                                {
                                    g = curx2;
                                    h = cury2;
                                    
                                    width = x - g;
                                    height = y - h;

                                    RecordVideo.x = Convert.ToInt32(g);
                                    RecordVideo.y = Convert.ToInt32(h);
                                    RecordVideo.temp = Convert.ToInt32(g + width);
                                    RecordVideo.temp1 = Convert.ToInt32(h + height);
                                    ShowWindow3();
                                 
                                    this.Close();
                                }
                            }
                        }



                    }

                    else
                    {
                        if (curx < x && cury > y)
                        {
                            System.Windows.Shapes.Rectangle r = new System.Windows.Shapes.Rectangle();
                            SolidColorBrush brush = new SolidColorBrush(backGroundColor1);
                            r.Stroke = brush;
                            r.Fill = new SolidColorBrush(Colors.Transparent);
                            r.StrokeThickness = 2;
                            r.Width = Math.Abs(x - curx + 5);
                            r.Height = Math.Abs(cury - y);
                            if (r.Width > 30 && r.Height > 30)
                            {
                                rect.Width = r.Width;
                                rect.Height = r.Height;
                                check = true;
                            }
                            cnv.Children.Clear();
                            cnv.Children.Add(r);

                            Canvas.SetRight(r, a1 - x + 10);
                            Canvas.SetTop(r, y);
                            if (e.LeftButton == MouseButtonState.Released)
                            {
                                double curx2 = e.GetPosition(null).X;
                                double cury2 = e.GetPosition(null).Y;
                                if ((x - curx2) < 5 && (cury2 - y) < 5)
                                {
                                    x = 0;
                                    y = 0;
                                    g = SystemParameters.WorkArea.Width;

                                    h = SystemParameters.WorkArea.Height;
                                    width = g - x;
                                    height = h - y;

                                    RecordVideo.x = Convert.ToInt32(g);
                                    RecordVideo.y = Convert.ToInt32(y);
                                    RecordVideo.temp = Convert.ToInt32(x + width);
                                    RecordVideo.temp1 = Convert.ToInt32(y + height);
                                    ShowWindow3();
                                  
                                    this.Close();
                                }


                                else
                                {

                                    g = curx2;
                                    h = cury2;
                                    
                                    width = x - g;
                                    height = h - y;

                                    RecordVideo.x = Convert.ToInt32(g);
                                    RecordVideo.y = Convert.ToInt32(y);
                                    RecordVideo.temp = Convert.ToInt32(x + width);
                                    RecordVideo.temp1 = Convert.ToInt32(y + height);
                                    ShowWindow3();
                                    
                                    this.Close();
                                }

                            }


                        }

                        else
                            if (cury < y && curx > x)
                            {
                                System.Windows.Shapes.Rectangle r = new System.Windows.Shapes.Rectangle();
                                SolidColorBrush brush = new SolidColorBrush(backGroundColor1);
                                r.Stroke = brush;
                                r.Fill = new SolidColorBrush(Colors.Transparent);
                                r.StrokeThickness = 2;
                                r.Width = Math.Abs(curx - x);
                                r.Height = Math.Abs(y - cury + 5);
                                if (r.Width > 30 && r.Height > 30)
                                {
                                    rect.Width = r.Width;
                                    rect.Height = r.Height;
                                    check = true;
                                }
                                cnv.Children.Clear();
                                cnv.Children.Add(r);
                                Canvas.SetLeft(r, x);
                                Canvas.SetBottom(r, b1 - y + 10);
                                if (e.LeftButton == MouseButtonState.Released)
                                {

                                    double curx2 = e.GetPosition(null).X;
                                    double cury2 = e.GetPosition(null).Y;
                                    if ((curx2 - x) < 5 && (y - cury2) < 5)
                                    {
                                        x = 0;
                                        y = 0;
                                        g = SystemParameters.WorkArea.Width;

                                        h = SystemParameters.WorkArea.Height;
                                        width = g - x;
                                        height = h - y;

                                        RecordVideo.x = Convert.ToInt32(g);
                                        RecordVideo.y = Convert.ToInt32(y);
                                        RecordVideo.temp = Convert.ToInt32(x + width);
                                        RecordVideo.temp1 = Convert.ToInt32(y + height);
                                        ShowWindow3();

                                        this.Close();


                                    }
                                    else
                                    {
                                        g = curx2;
                                        h = cury2;
                                       
                                        width = g - x;
                                        height = y - h;

                                        RecordVideo.x = Convert.ToInt32(x);
                                        RecordVideo.y = Convert.ToInt32(h);
                                        RecordVideo.temp = Convert.ToInt32(x + width);
                                        RecordVideo.temp1 = Convert.ToInt32(y + height);
                                        ShowWindow3();

                                        this.Close();
                                    }


                                }
                            }

                            else
                            {
                             
                                
                                    System.Windows.Shapes.Rectangle r = new System.Windows.Shapes.Rectangle();
                                    SolidColorBrush brush = new SolidColorBrush(backGroundColor1);

                                    r.Stroke = brush;
                                    r.Fill = new SolidColorBrush(Colors.Transparent);
                                    r.StrokeThickness = 2;
                                    r.Width = Math.Abs(curx - x);
                                    r.Height = Math.Abs(cury - y);
                                    if (r.Width > 30 && r.Height > 30)
                                    {
                                        check = true;
                                    }
                                    cnv.Children.Clear();
                                    cnv.Children.Add(r);

                                    Canvas.SetLeft(r, x);
                                    Canvas.SetTop(r, y);

                                    if (e.LeftButton == MouseButtonState.Released)
                                    {
                                        double curx2 = e.GetPosition(null).X;
                                        double cury2 = e.GetPosition(null).Y;
                                        if ((curx2 - x) < 5 && (cury2 - y) < 5)
                                        {
                                            x = 0;
                                            y = 0;
                                            g = SystemParameters.WorkArea.Width;

                                            h = SystemParameters.WorkArea.Height;

                                            width = g - x - 9;
                                            height = h - y - 9;

                                            RecordVideo.x = Convert.ToInt32(x);
                                            RecordVideo.y = Convert.ToInt32(y);
                                            RecordVideo.temp = Convert.ToInt32(g);
                                            RecordVideo.temp1 = Convert.ToInt32(h - 2);
                                            ShowWindow3();
                                         
                                            this.Close();
                                        }


                                        else
                                        {
                                            g = curx2;
                                            h = cury2;
                                            width = g - x;
                                            height = h - y;

                                            RecordVideo.x = Convert.ToInt32(x);
                                            RecordVideo.y = Convert.ToInt32(y);
                                            RecordVideo.temp = Convert.ToInt32(g);
                                            RecordVideo.temp1 = Convert.ToInt32(h - 2);
                                            ShowWindow3();
                                            this.Close();
                                        }

                                    }
                              
                                
                            }
                    }
            }
        }
        bool check = false;

        public void DrawRect()
        {

        }
        //Capturing and Saving Picture 
        public void CaptureScreen(double x, double y, double width, double height)
        {
            MainWindow m1 = System.Windows.Application.Current.MainWindow as MainWindow;
            try
            {
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
                //      ig = Convert.ToInt32(g);
                iq = Convert.ToInt32(h);
                Bitmap image = new Bitmap(iw, ih, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
                Graphics g = Graphics.FromImage(image);
                this.Close();
                g.CopyFromScreen(ix - 4, iy - 4, 0, 0, new System.Drawing.Size(iw, ih), CopyPixelOperation.SourceCopy);
                BackWindow bw = new BackWindow();
                bw.Height = height;
                bw.Width = width;
                bw.Left = x;
                bw.Top = y;
                bw.WindowStyle = System.Windows.WindowStyle.None;
                bw.Show();
                Thread.Sleep(100);
                bw.Hide();
                bw.Close();
                MainWindow.isReturn = false;
                m1.Show();

                m1.SetWindow();
                MainWindow.isCaptureStarted = false;
                MainWindow.bmp = image;
                image.Save(path, ImageFormat.Png);


                if (main.OpenPaint.IsChecked && main.SaveAs.IsChecked)
                {
                    MainWindow.openSaveAs();
                }
                else
                {
                    if (main.OpenPaint.IsChecked)
                    {
                        MainWindow.open_image();
                    }
                    else
                        if (main.SaveAs.IsChecked)
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

        //Capturing and Saving Picture 
        public void CaptureScreen2(double x, double y, double width, double height)
        {
            MainWindow m1 = System.Windows.Application.Current.MainWindow as MainWindow;
            try
            {
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
                Bitmap image = new Bitmap(iw, ih, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
                Graphics g = Graphics.FromImage(image);
                this.Close();
                g.CopyFromScreen(ix, iy, 0, 0, new System.Drawing.Size(iw, ih), CopyPixelOperation.SourceCopy);
                BackWindow bw = new BackWindow();
                bw.Height = height;
                bw.Width = width;
                bw.Left = x;
                bw.Top = y;
                bw.WindowStyle = System.Windows.WindowStyle.None;
                bw.Show();
                Thread.Sleep(100);
                bw.Hide();
                bw.Close();
                MainWindow.isReturn = false;
                m1.Show();
                m1.SetWindow();
                MainWindow.isCaptureStarted = false;
                MainWindow.bmp = image;

                image.Save(path, ImageFormat.Png);


                if (main.OpenPaint.IsChecked && main.SaveAs.IsChecked)
                {
                    MainWindow.openSaveAs();
                }
                else
                {
                    if (main.OpenPaint.IsChecked)
                    {
                        MainWindow.open_image();
                    }
                    else
                        if (main.SaveAs.IsChecked)
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
        internal class NativeMethods
        {

            [DllImport("user32.dll")]
            public extern static IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hwnd);
            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern IntPtr GetForegroundWindow();
            [DllImport("gdi32.dll")]
            public static extern UInt64 BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, System.Int32 dwRop);

        }




        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && e.ClickCount == 1)
            {
                isMouseDown = true;
                x = e.GetPosition(this).X;
                y = e.GetPosition(this).Y;
            }

        }
        Utilities.globalKeyboardHook gkh = new Utilities.globalKeyboardHook();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gkh.HookedKeys.Add(Keys.Escape);
            gkh.KeyDown += new System.Windows.Forms.KeyEventHandler(gkh_KeyDown);

        }

        // Escape keyboard global hook.
        void gkh_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            MainWindow m = System.Windows.Application.Current.MainWindow as MainWindow;
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();

                MainWindow.isReturn = false;
                m.Show();
                m.SetWindow();
                MainWindow.isCaptureStarted = false;
                gkh.unhook();



            }
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            MainWindow m = System.Windows.Application.Current.MainWindow as MainWindow;
            if (e.Key == Key.Escape)
            {
                this.Close();

                MainWindow.isReturn = false;
                m.Show();
                m.SetWindow();
                MainWindow.isCaptureStarted = false;
                gkh.unhook();


            }
        }
    }
}
