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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Utilities;


namespace Camshot
{
    public partial class MainWindow : Window
    {
        private static IntPtr _previousHandle = IntPtr.Zero;
        private static IntPtr _previousToLastHandle = IntPtr.Zero;
      
        private static IntPtr Handle = IntPtr.Zero;

        public static string strshowUI;
        public static DateTime recordStarttime;
        private int id;
        public static Bitmap bmpSS = null, bmp = null;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
        enum GetWindow_Cmd : uint
        {
        GW_HWNDFIRST = 0,
        GW_HWNDLAST = 1,
        GW_HWNDNEXT = 2,
        GW_HWNDPREV = 3,
        GW_OWNER = 4,
        GW_CHILD = 5,
        GW_ENABLEDPOPUP = 6}
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]

        static extern bool SetForegroundWindow(IntPtr hWnd);

        public static string check_image = "False", check_video = "False", check2;
        static IntPtr lastHandle = IntPtr.Zero;
        public static string path, temp_path_image, temp_path_video, temp_path1, check_int, temp_path2;
        public static string pathString, globalPath, VideopathString, globalVideoPath;
        public static System.Drawing.Point CursorPosition;
        static FolderBrowserDialog changepath = new FolderBrowserDialog();
        public static string whichChecked = "customscreen", outputformat = ".wmv";
        string format = "MMM_ddd_yyyy_HH-mm-ff";
        public static bool isShowDialog = false, isImagePathChanged = false, isVideoPathChanged = false;
        public static bool isRightMouseDown = false;
        double a1 = SystemParameters.WorkArea.Width; 
        double b1 = SystemParameters.WorkArea.Height;
        double a = SystemParameters.VirtualScreenWidth;
        double b = SystemParameters.VirtualScreenHeight;
        public globalKeyboardHook gkh = new globalKeyboardHook();
        Window1 win;
        public static bool isReturn = false;
        private static object syncRoot = new Object();
        public static SpyWindow sw =null, previousSpy =null;
        public static Window3 w = null,previousObject =null;
         public  static bool isCaptureStarted = false;

        //Intialize the mainwindow.
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            InitializeTimer();
            LoadSettings l = new LoadSettings();  // Loads the saved settings 

            var k = App.Current as App;

            string folderName = Environment.GetFolderPath(Environment.SpecialFolder.Personal); //Set the user directory.
            //         k.i = 1;
            pathString = System.IO.Path.Combine(folderName, "ImageCapture"); //Default Path for imagecapture
            VideopathString = System.IO.Path.Combine(folderName, "VideoCapture"); //Default path for videocapture.
            LoadBool(); // Load the bool values whether the path is changed or not
                        // True = Path changed = check_image
                        // False = Path not changed = check_video.

            if (check_image == null)
            {
                check_image = "False";
               
            }
            if (check_video == null)
            {
                check_video = "False";
           
            }
            bool isExists = System.IO.Directory.Exists(pathString);

            if (!isExists)
            {
                System.IO.Directory.CreateDirectory(pathString); //Create image gallery directory if not exists.
            }
            isExists = System.IO.Directory.Exists(VideopathString);
            if (!isExists)
            {
                System.IO.Directory.CreateDirectory(VideopathString); //Create video gallery directory if not exists.
            }
            this.ShowInTaskbar = false;
            MainWindowAnimation();
        }
     
        
        System.Windows.Forms.Timer timer1;

        private void MainWindowAnimation()
        
        {
            button3.Visibility = System.Windows.Visibility.Hidden;
            float left = 650, top =450,setLeft =56;
            this.Show();
            this.Left = a1 - 650;
            this.Top = b1 - 450;
            Thread.Sleep(500);
            while (left != setLeft)
            {
                left = left - 2;
                top =(float) (top -1);
                this.Left = a1 - left;
                this.Top = b1 - top;
                Thread.Sleep(10);
                this.Show();
            }
            button3.Visibility = System.Windows.Visibility.Visible;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            gkh.HookedKeys.Add(Keys.F10); // Change mode.
            gkh.HookedKeys.Add(Keys.F9);// // Start Capturing
            gkh.HookedKeys.Add(Keys.Escape);// //Closing the window
            gkh.HookedKeys.Add(Keys.F5);// //Start/Pause / Resume Recording
            gkh.HookedKeys.Add(Keys.F6);// // Stop Recording
            gkh.KeyDown += new System.Windows.Forms.KeyEventHandler(gkh_KeyDown);

        }
        public  void SetWindow()
        {
            button3.Visibility = System.Windows.Visibility.Visible;
            this.Height = 156;
            this.Width = 120;
            this.Left = a1 - 56;
            this.Top = b1 - 150;
        } //Set the location and margin of MainWindow

        //Global Keyboard handler when key is pressed.
        void gkh_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                //    Thread.Sleep(500);
                if (e.KeyCode == Keys.F9)
                {

                    StartCapture();
                }
                else
                    if (e.KeyCode == Keys.F10)
                    {
                        Thread.Sleep(1000);
                        if (PicCapture.IsChecked)
                        {
                            changeToVideo();
                            PicCapture.IsChecked = false;
                            VideoCapture.IsChecked = true;
                        }
                        else
                            if (VideoCapture.IsChecked)
                            {
                                changeToImage();
                                PicCapture.IsChecked = true;
                                VideoCapture.IsChecked = false;

                            }
                    }
                   
                    else
                        if (e.KeyCode == Keys.Escape)
                        {
                            if (PicCapture.IsChecked)
                            {
                                if (sw != previousSpy)
                                {
                                    previousSpy = sw;
                                    
                                    sw.ua.UninstallHooks();
                                    WindowHighlighter.Refresh(IntPtr.Zero);
                                    sw.Close();
                                    isReturn = false;
                                    this.Show();
                                    SetWindow();
                                    isCaptureStarted = false;
                                }
                            }
                            else
                                if (VideoCapture.IsChecked)
                                {
                                    var k = App.Current as App;
                                    if (k.isPlaying == false)
                                    {
                                        if (sw != previousSpy && w ==null)
                                        {
                                            previousSpy = sw;
                                            sw.ua.UninstallHooks();
                                            WindowHighlighter.Refresh(IntPtr.Zero);
                                            sw.Close();
                                            isReturn = false;
                                            this.Show();
                                            SetWindow();
                                            isCaptureStarted = false;
                                        }
                                        else
                                            if (w != null)
                                            {
                                                isReturn = false;
                                                w.Escape();
                                                this.Show();
                                                SetWindow();
                                                isCaptureStarted = false;
                                            }
                                    }
                                    else
                                        if (k.isPlaying == true)
                                        {
                                            if (w != previousObject)
                                            {
                                                sw = previousSpy;
                                                Window3.popup1.IsOpen = false;
                                                w.CloseWindow();
                                                previousObject = w;
                                            }
                                        }
                                }
                        }
                        else
                            if (e.KeyCode == Keys.F5)
                            {
                                if (w != previousObject)
                                {
                                    w.StartRecording2();
                                    previousObject = w;
                                }
                            }
                            else
                                if (e.KeyCode == Keys.F6)
                                {
                                    var k = App.Current as App;
                                    if (k.isPlaying == true)
                                    {
                                        w.StopRecording2();
                                        previousObject = w;
                                    }

                                }
            }
            catch (Exception e1)
            {
                System.Windows.Forms.MessageBox.Show(e1.ToString());
            }
        }
      
        /// <summary>
        /// Returns the last handle
        /// </summary>
        public static IntPtr LastHandle
        {
            get
            {
                return _previousToLastHandle;
            }
        }
      

     /// <summary>
     ///Take the screenshot of full screen
     /// </summary>
        public void Take_ScreenShot()
        {
            this.Height = 0;
            this.Width = 0;
            Graphics gfxSS = null;
            var k = App.Current as App;
            try
            {
                DateTime saveNow = DateTime.Now;

                LoadBool();
                if (check_image == "False") //check for path changed or not.
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\ImageCapture\" + saveNow.ToString(format) + ".png";
                    globalPath = path;
                }
                else
                {
                    LoadSettings();

                    path = temp_path_image + @"\" + saveNow.ToString(format) + ".png";
                    globalPath = path;
                }
                bmpSS = new Bitmap(SystemInformation.VirtualScreen.Width,
                   SystemInformation.VirtualScreen.Height,
                   System.Drawing.Imaging.PixelFormat.Format16bppRgb565);

                gfxSS = Graphics.FromImage(bmpSS);

               
                gfxSS.CopyFromScreen(                                                          
                    SystemInformation.VirtualScreen.X,
                    SystemInformation.VirtualScreen.Y,
                    0,
                    0,
                    SystemInformation.VirtualScreen.Size,
                    CopyPixelOperation.SourceCopy); // Capture Screen
                BackWindow bw = new BackWindow();
                bw.Height = Screen.PrimaryScreen.Bounds.Height;
                bw.Width = Screen.PrimaryScreen.Bounds.Width;
                bw.Left = 0;
                bw.Top = 0;
                bw.WindowStyle = System.Windows.WindowStyle.None;
                bw.Show();
                Thread.Sleep(100);
                bw.Hide();
                bw.Close();
                bmp = bmpSS;
                bmpSS.Save(path, ImageFormat.Png);

                SetWindow();
                MainWindow.isCaptureStarted = false;
              
                if (OpenPaint.IsChecked && SaveAs.IsChecked) // Check for open paint and save as checkboxes are checked or not.
                {
                    openSaveAs();
                }
                else
                {
                    if (OpenPaint.IsChecked)
                    {
                        open_image();

                    }
                    else
                        if (SaveAs.IsChecked)
                        {
                            openSaveAs();
                        }
                }
                
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("You dont Have permissions to save in this location");
            }

        }
        //Top MenuItem Captureclick handler
        private void CaptureScreen_Click(object sender, RoutedEventArgs e)
        {
            StartCapture();
        }

        //Start capturing after verifying selected mode.
        private void StartCapture()
        {
            isCaptureStarted = true;
            RightClickMenu.IsOpen = false;
            var k = App.Current as App;
            if (PicCapture.IsChecked)
            {
                k.PicChecked = true;
                k.VideoChecked = false;
                Window1.isPicChecked = true;
                capturePic();

            }
            else
                if(VideoCapture.IsChecked)
            {
                k.VideoChecked = true;
                k.PicChecked = false;
                Window1.isPicChecked = false;
                CaptureVideo();
            }
        }

        /// <summary>
        //Set the last window active
        /// </summary>
      
        private void setLastActive()
        {
         //   IntPtr targetHwnd = GetWindow(Process.GetCurrentProcess().MainWindowHandle, (uint)GetWindow_Cmd.GW_HWNDLAST);
            //while (true)
            //{
                IntPtr temp = GetForegroundWindow();
            //    if (temp.Equals(IntPtr.Zero)) break;
            //    targetHwnd = temp;
                IntPtr p = GetWindow(temp, 2);
                SetForegroundWindow(p);
        }
      

        /// <summary>
        /// Called for the image to capture.
        /// Verify the selected mode.(full Screen , custom screen , top window)
        /// TakeScreenShot for full screen
        /// Window1 for custom screen selection
        /// SpyWindow 'sp' for top window selection.
        /// </summary>
        private void capturePic()
        {
            RightClickMenu.IsOpen = false;
            var k = App.Current as App;
            if (GrabScreenShot.IsChecked)
            {
                Take_ScreenShot();
            }
            else
                if (CustomScreenSize.IsChecked)
                {
                    if (isReturn == false)
                    {
                        this.Width = 0;
                        this.Height = 0;
                        Thread.Sleep(200);
                        win = new Window1();
                        win.WindowState = WindowState.Maximized;
                        win.BringIntoView();
                        isReturn =true ;
                        win.ShowDialog();
                     
                    }
                }
                else

                    if (TopWindow.IsChecked)
                    {
                        if (isReturn == false)
                        {
                            this.Width = 0;
                            this.Height = 0;
                            setLastActive();
                            isReturn = true;
                           sw = new SpyWindow();
                            sw.Show();
                            isReturn = false;
                         

                        }
                    }
        }
        /// <summary>
        /// Called for the video to capture.
        /// Verify the selected mode.(full Screen , custom screen , top window)
        /// Window3 object 'w' for full screen
        /// Window1 for custom screen selection
        /// SpyWindow 'sp' for top window selection.
        /// </summary>
        private void CaptureVideo()
        {
            try
            {
                RightClickMenu.IsOpen = false;
                var k = App.Current as App;
                if (GrabScreenShot.IsChecked)
                {
                    this.Width = 0;
                    this.Height = 0;
                    w = new Window3();
                    w.Show();
                }
                else
                    if (CustomScreenSize.IsChecked)
                    {
                        if (isReturn == false)
                        {
                            this.Width = 0;
                            this.Height = 0;
                            Thread.Sleep(200);
                            win = new Window1();
                            win.WindowState = WindowState.Maximized;
                            isReturn = true;
                            win.ShowDialog();
                        }       
                    }
                    else

                        if (TopWindow.IsChecked)
                        {
                            if (isReturn == false)
                            {
                                this.Width = 0;
                                this.Height = 0;
                                setLastActive();
                                isReturn = true;
                                sw = new SpyWindow();
                                sw.ShowDialog();
                            
                           
                            }
                        }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// Changed the selection to full screen mode 
        /// Deselects the custom and top window checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrabScreenShot_Click(object sender, System.EventArgs e)
        {

            CustomScreenSize.IsChecked = false;
            TopWindow.IsChecked = false;
            GrabScreenShot.IsChecked = true;
            whichChecked = "fullscreen";
     
        }
        /// <summary>
        /// Changed the selection to custom screen mode 
        /// Deselects the full screen and top window checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomScreenSize_Click(object sender, RoutedEventArgs e)
        {

            GrabScreenShot.IsChecked = false;
            CustomScreenSize.IsChecked = true;
            TopWindow.IsChecked = false;
            whichChecked = "customscreen";
        }
      /// <summary>
        /// Changed the selection to top screen mode 
        /// Deselects the full screen and custom screen checkbox
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
        private void TopWindow_Click(object sender, RoutedEventArgs e)
        {

            GrabScreenShot.IsChecked = false;
            CustomScreenSize.IsChecked = false;
            TopWindow.IsChecked = true;
            whichChecked = "topwindow";
        }
        /// <summary>
        /// Exits and leave the application.
        /// Release the WmEncoder process resources.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (Process proc in Process.GetProcessesByName("Windows Media Encoder (32 bit)"))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            System.Windows.Application.Current.Shutdown();
            this.Close();
            gkh.unhook();
         
        }

        /// <summary>
        /// Browse the image gallery
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OpenImageDir_Click(object sender, RoutedEventArgs e)
        {
            LoadBool();
            if (check_image == "False")
            {
                path = pathString;
            }
            else
            {
                LoadSettings();
                path = temp_path_image;
            }
            System.Diagnostics.Process.Start(path);
        }
        /// <summary>
        /// Browse the video gallery
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OpenVideoDir_Click(object sender, RoutedEventArgs e)
        {
            LoadBool();
            if (check_video == "False")
            {
                path = VideopathString;
            }
            else
            {
                LoadSettings();
                path = temp_path_video;
             //   System.Windows.Forms.MessageBox.Show(path);
            }
            System.Diagnostics.Process.Start(path);
        }
        /// <summary>
        /// Open saved image in paint.
        /// </summary>
        public static void open_image()
        {
            Process.Start("mspaint", "\"" + globalPath + "\"");
        }
        /// <summary>
        /// Open saved video in paint.
        /// </summary>
        public static void open_media()
        {
            Process.Start("wmplayer.exe", "\"" + globalVideoPath + "\"");
        }
       static string pathName, fileName;

        /// <summary>
        /// OpenSaveAs is called and it will open save dialog bto save the recent capture.
        /// 
        /// </summary>
        public static void openSaveAs()
        {
            var k = App.Current as App;
            MainWindow m = System.Windows.Application.Current.MainWindow as MainWindow;
            if (k.PicChecked == true)
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.InitialDirectory = MainWindow.globalPath;
                saveDialog.FileName = MainWindow.globalPath;
                saveDialog.Filter = "PNG Image|*.png";
                saveDialog.Title = "Save Image as";
                DialogResult res = saveDialog.ShowDialog();
                pathName = Path.GetDirectoryName(saveDialog.FileName);
                fileName = Path.GetFileName(saveDialog.FileName);
                        if (res == System.Windows.Forms.DialogResult.OK)
                        {
                          
                                if (!File.Exists(pathName + @"\" + fileName))
                                {
                                    File.Copy(MainWindow.globalPath, pathName + @"\" + fileName);
                                    File.Delete(MainWindow.globalPath);
                                    globalPath = pathName + @"\" + fileName;
                                  
                                    if (m.OpenPaint.IsChecked)
                                    {
                                        open_image();
                                    }
                                }
                                else
                                {
                                    if (MainWindow.globalPath == pathName + @"\" + fileName)
                                    {
                                        globalPath = pathName + @"\" + fileName;
                                        if (m.OpenPaint.IsChecked)
                                        {
                                            open_image();
                                        }
                                    }
                                    else
                                    {
                                        bmp.Save(pathName + @"\" + fileName);
                                        File.Delete(MainWindow.globalPath);
                                        globalPath = pathName + @"\" + fileName;
                                        if (m.OpenPaint.IsChecked)
                                        {
                                            open_image();
                                        }
                                    }
                                }
                        }
                        else

                            if (res == System.Windows.Forms.DialogResult.Cancel)
                                {
                                    globalPath = pathName + @"\" + fileName;
                                  if (m.OpenPaint.IsChecked)
                                    {
                                        open_image();
                                    }
                                }
                        saveDialog.Dispose();
            }
            else
                if (k.VideoChecked == true)
                {
                    try
                    {
                        SaveFileDialog dialog = new SaveFileDialog();
                        dialog.InitialDirectory = MainWindow.globalVideoPath;
                        dialog.FileName = MainWindow.globalVideoPath;
                        dialog.Title = "Save Video as";
                        dialog.Filter = "Video Files (*" + MainWindow.outputformat + ")|*" + MainWindow.outputformat;
                        dialog.RestoreDirectory = true;  
                        dialog.CheckFileExists = false;
                        DialogResult res = dialog.ShowDialog();
                        pathName = Path.GetDirectoryName(dialog.FileName);
                        fileName = Path.GetFileName(dialog.FileName);
                        if (dialog.FileName != string.Empty)
                        {
                            if (res == System.Windows.Forms.DialogResult.OK)
                            {

                                    if (!File.Exists(pathName + @"\" + fileName))
                                    {
                                        File.Copy(MainWindow.globalVideoPath, pathName + @"\" + fileName);
                                        File.Delete(MainWindow.globalVideoPath);
                                        globalVideoPath = pathName + @"\" + fileName;
                                        if (m.OpenMediaPlayer.IsChecked)
                                        {
                                            open_media();
                                        }
                                    }
                                    else
                                    {
                                        if (MainWindow.globalVideoPath == pathName + @"\" + fileName)
                                        {
                                            globalVideoPath = pathName + @"\" + fileName;
                                            if (m.OpenMediaPlayer.IsChecked)
                                            {
                                                open_media();
                                            }
                                        }
                                        else
                                        {
                                            File.Delete(pathName + @"\" + fileName);
                                            File.Copy(MainWindow.globalVideoPath, pathName + @"\" + fileName);
                                            File.Delete(MainWindow.globalVideoPath);
                                            globalVideoPath = pathName + @"\" + fileName;
                                            if (m.OpenMediaPlayer.IsChecked)
                                            {
                                                open_media();
                                            }
                                        }
                                    }
                            }
                            else
                                if (res == System.Windows.Forms.DialogResult.Cancel)
                                {
                                    globalVideoPath = pathName + @"\" + fileName;
                                    if (m.OpenMediaPlayer.IsChecked)
                                    {
                                        open_media();
                                    }
                                }
                            dialog.Dispose();
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Please provide filename to save");
                        }
                     

                    }
                    catch (Exception e )
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString());
                    }  
                }
        }
        
        /// <summary>
        /// Change the mode.
        /// Change to video capture mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoCapture_Click(object sender, RoutedEventArgs e)
        {
            changeToVideo();
            VideoCapture.IsChecked = true;
            PicCapture.IsChecked = false;
        }
        /// <summary>
        /// Change the mode
        /// Change to photo capture mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PicCapture_Click(object sender, RoutedEventArgs e)
        {
            changeToImage();
            PicCapture.Background = new SolidColorBrush(Colors.Red);
            VideoCapture.IsChecked = false;
            PicCapture.IsChecked = true;
        }
        private const string FileName = "settings.ini";

        /// <summary>
        /// Write the changed path to isolated fille directory for video and photo.
        /// temp_path1 = Photo path
        /// temp_path2 = Video Path
        /// </summary>
        public void SavePathToDisk()
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                using (StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream(FileName, FileMode.Create, isoStore)))
                {

                    writer.WriteLine(temp_path1);
                    writer.WriteLine(temp_path2);
                    SaveBool();
                }
            }
        }
        /// <summary>
        /// Load the changed path at runtime.
        /// temp_path_image = ImagePath
        /// tamp_path_video = VideoPath
        /// </summary>
        public static void LoadSettings()
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                if (isoStore.GetFileNames(FileName).Length > 0)
                {
                    using (StreamReader reader = new StreamReader(new IsolatedStorageFileStream(FileName, FileMode.OpenOrCreate, isoStore)))
                    {
                        temp_path_image = reader.ReadLine();
                        temp_path_video = reader.ReadLine();
                    }
                }
            }

        }
        /// <summary>
        /// Save the bool value whether the path has hanged or not
        /// true = for path changed.
        /// false = for path not changed.
        /// </summary>
        private const string FileName1 = "a.ini";

        public void SaveBool()
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                using (StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream(FileName1, FileMode.Create, isoStore)))
                {

                    writer.WriteLine(isImagePathChanged.ToString());
                    writer.WriteLine(isVideoPathChanged.ToString());
                    writer.Close();
                }
            }
        }
        /// <summary>
        /// Read the changed path at runtime.
        /// check_image = ImagePath
        /// check_video = VideoPath
        /// </summary>
        public static void LoadBool()
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                if (isoStore.GetFileNames(FileName1).Length > 0)
                {
                    using (StreamReader reader = new StreamReader(new IsolatedStorageFileStream(FileName1, FileMode.OpenOrCreate, isoStore)))
                    {
                        check_image = reader.ReadLine();
                        check_video = reader.ReadLine();
                        reader.Close();
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private const string FileName2 = "bool.ini";
        public void SaveBool2()
        {
            var k = App.Current as App;
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                using (StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream(FileName2, FileMode.Create, isoStore)))
                {
                    writer.WriteLine(check2);
                    writer.Close();
                    writer.Close();
                }
            }
        }
        public static void LoadBool2()
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                if (isoStore.GetFileNames(FileName1).Length >= 0)
                {
                    using (StreamReader reader = new StreamReader(new IsolatedStorageFileStream(FileName2, FileMode.OpenOrCreate, isoStore)))
                    {
                        check2 = reader.ReadLine();
                        reader.Close();
                    }
                }
            }

        }
        /// <summary>
        /// Save format settings.
        /// 
        /// </summary>
        private const string FileName3 = "format.ini";
        public void SaveFormat()
        {
            var k = App.Current as App;
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                using (StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream(FileName3, FileMode.Create, isoStore)))
                {
                    writer.WriteLine(isShowDialog);
                    writer.Close();
                }
            }
        }
        /// <summary>
        /// Change to Wmv mode.
        /// Deselect the mpeg and mp4 formats
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Wmv_Click(object sender, RoutedEventArgs e)
        {
            Wmv.IsChecked = true;
            Mp4.IsChecked = false;
            Mpg.IsChecked = false;
            outputformat = ".wmv";
        }
        /// <summary>
        /// Change to Mp4 mode.
        /// Deselect the Wmv and mp4 formats
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Mp4_Click(object sender, RoutedEventArgs e)
        {
            Wmv.IsChecked = false;
            Mp4.IsChecked = true;
            Mpg.IsChecked = false;
            outputformat = ".mp4";
        }
        /// <summary>
        /// Change to Mp4 mode.
        /// Deselects the mpeg and wmv formats 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Mpg_Click(object sender, RoutedEventArgs e)
        {
            Wmv.IsChecked = false;
            Mp4.IsChecked = false;
            Mpg.IsChecked = true;
            outputformat = ".mpeg";
        }
        private void SetPath_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            LoadSettings();
        }
        bool pressed = false;
        /// <summary>
        /// Movement of grid when mouse left click is pressed.
        /// Window will move with the cursor position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
           
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                    pressed = true;
                    ReleaseCapture();
                    WindowInteropHelper oWindowInteropHelper = new WindowInteropHelper(this);
                    SendMessage(oWindowInteropHelper.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                
            }
        
       
            
        }
        bool mouseleavefromgrid = false;
        /// <summary>
        /// Called when the mouse leaves from the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mygrid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (done == true)
            {
                if (isCaptureStarted == false)
                {
                    mouseleavefromgrid = true;
                    if (RightClickMenu.IsOpen == false)
                    {
                        button3.Visibility = System.Windows.Visibility.Visible;

                        if (PicCapture.IsChecked)
                        {
                            img_cam.Source = new BitmapImage(new Uri("/Camshot;component/Images/cam1.png", UriKind.Relative));
                            img_vid.Source = new BitmapImage(new Uri("/Camshot;component/Images/cam1.png", UriKind.Relative));

                        }
                        else
                            if (VideoCapture.IsChecked)
                            {
                                img_vid.Source = new BitmapImage(new Uri("/Camshot;component/Images/vid1.png", UriKind.Relative));
                                img_cam.Source = new BitmapImage(new Uri("/Camshot;component/Images/vid1.png", UriKind.Relative));
                            }
                        CreateLeavingAnimations(56, 6);
                        done = false;
                    }

                }
            }
           
        }

        private void mygrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// Called when the mouse leaves the context menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightClickMenu_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (done == true)
            {
                if (mouseleavefromgrid == true)
                {
                    button3.Visibility = System.Windows.Visibility.Visible;
                    CreateLeavingAnimations(56, 6);
                    done = false;
                    mouseleavefromgrid = false;

                }
                else
                {
                    button3.Visibility = System.Windows.Visibility.Hidden;
                    RightClickMenu.IsOpen = false;
                }
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            changeToImage();
        }
        /// <summary>
        /// It will change the mode to camera mode.
        /// Change the selection to picture type.
        /// </summary>
        private void changeToImage()
        {
            img_vid.Source = new BitmapImage(new Uri("/Camshot;component/Images/cam1.png", UriKind.RelativeOrAbsolute));
            img_cam.Source = new BitmapImage(new Uri("/Camshot;component/Images/cam1.png", UriKind.RelativeOrAbsolute));
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Visible;
            PicCapture.IsChecked = true;
            VideoCapture.IsChecked = false;
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            changeToVideo();
        }
        /// <summary>
         /// It will change the mode to video mode.
        /// Change the selection to video type.
        /// </summary>
        private void changeToVideo()
        {
            img_vid.Source = new BitmapImage(new Uri("/Camshot;component/Images/vid1.png", UriKind.Relative));
            img_cam.Source = new BitmapImage(new Uri("/Camshot;component/Images/vid1.png", UriKind.Relative));
            button1.Visibility = System.Windows.Visibility.Visible;
            button2.Visibility = System.Windows.Visibility.Hidden;
            PicCapture.IsChecked = false;
            VideoCapture.IsChecked = true;
        }
        private void image1_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }
        /// <summary>
        /// Open the context menu on right click on grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void image2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RightClickMenu.IsOpen = false;
           
            var k = App.Current as App;
            if (PicCapture.IsChecked)
            {
                k.PicChecked = true;
                k.VideoChecked = false;
                Window1.isPicChecked = true;
                capturePic();

            }
            else
            {
                k.VideoChecked = true;
                k.PicChecked = false;
                Window1.isPicChecked = false;
                CaptureVideo();
            }

        }
        private void SetPath_Click(object sender, RoutedEventArgs e)
        {
            new SetPath().Visibility = Visibility.Visible;
          

        }
        private void image4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            changeToVideo();
        }
        private void image6_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            double curx1 =  System.Windows.Forms.Cursor.Position.X;
            double cury1 = System.Windows.Forms.Cursor.Position.Y;
            RightClickMenu.HorizontalOffset = curx1-30;
            RightClickMenu.VerticalOffset = cury1 + 15;
                RightClickMenu.IsOpen = true;
        
        }

        private void border1_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
       
            RightClickMenu.IsOpen = true;
            RightClickMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.RelativePoint;
            double curx1 = e.GetPosition(null).X;
            double cury1 = e.GetPosition(null).Y;
            RightClickMenu.HorizontalOffset = curx1 - 47;
            RightClickMenu.VerticalOffset = cury1 - 57;

        }
        private void image6_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        private void image4_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
             if(PicCapture.IsChecked)
        {
            LoadBool();
            if (check_image == "False")
            {
                path = pathString;
            }
            else
            {
                LoadSettings();
                path = temp_path_image;
            }
            System.Diagnostics.Process.Start(path);
        }
             else
                 if(VideoCapture.IsChecked)
       
        {
            LoadBool();
            if (check_video == "False")
            {
                path = VideopathString;
            }
            else
            {
                LoadSettings();
                path = temp_path_video;
            }
            System.Diagnostics.Process.Start(path);
        }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
         
            StartCapture(); //start capturing the video/image
        }
        /// <summary>
        /// Button3 = Change the image accordind to selected modes.
        /// Verify the picture / video mode and then start capturing.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        bool done = false;
        private void CreateAnimation()
        {
            t.Stop();
            t.Dispose();
            int left = 56;
            int setLeft = 102;
            int i = 2;
            while (left != setLeft)
            {
                this.Left = a1 - left;
                left = left + i;
                Thread.Sleep(5);
            }
          
            this.Left = a1 - left-4;
            done = true;
            button4.Margin = new Thickness(51.5, 43, 0, 0);
            button3.Visibility = System.Windows.Visibility.Hidden;
        }
        private void CreateButtonAnimations(int setLeft )
        {
            int left = 6;
            int i = 2;
            while (left != setLeft)
            {
                left = left + i;
                button3.Margin = new Thickness(left, 43, 0, 0);
                Thread.Sleep(100);
            }

        }
        private void CreateLeavingAnimations(int setLeft, int buttonsetLeft)
        {
            int left = 102;
            int i = 2;
            while (left != setLeft)
            {
              
                this.Left = a1 - left;
                left = left - i;
                Thread.Sleep(5);
            }
            done = false;
        }
        private void CreateBorderAnimations()
        {
            t.Stop();
            int left = 46 ,setLeft =0;
            int i = 2;
            while (left != setLeft)
            {
                left = left - i;
                border1.Margin = new Thickness(left, 6, 0, 0);
                button2.Margin = new Thickness(left, 16, 12, 0);
                button5.Margin = new Thickness(left, 90, 12, 0);
            }
        }
        static System.Windows.Forms.Timer t;
        public void InitializeTimer()
        {
            t = new System.Windows.Forms.Timer();
            t.Tick += new EventHandler(timer1_Tick);
        }
       
        /// <summary>
        ///Start the timer tick.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void timer1_Tick(object sender, EventArgs e)
        {
            CreateAnimation();
        }
        private void hideButton()
        {
            button3.Visibility = System.Windows.Visibility.Hidden;
            return;
        }
        private void button3_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            button3.Visibility = System.Windows.Visibility.Hidden;
            t.Start();
            
            if (PicCapture.IsChecked)
            {
                button4.Margin = new Thickness(51.5, 43, 0, 0);
                img_cam.Source = new BitmapImage(new Uri("/Camshot;component/Images/cam1.png", UriKind.Relative));
                img_vid.Source = new BitmapImage(new Uri("/Camshot;component/Images/cam1.png", UriKind.Relative));
            }
            else
                if (VideoCapture.IsChecked)
                {
                    img_cam.Source = new BitmapImage(new Uri("/Camshot;component/Images/vid1.png", UriKind.Relative));
                    img_vid.Source = new BitmapImage(new Uri("/Camshot;component/Images/vid1.png", UriKind.Relative));
                }
        }
        public int from_top1
        {
            get;
            set;
        }
        public int from_left1
        {
            get;
            set;
        }

        //// Using a DependencyProperty as the backing store for myText.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty myTextProperty =
        //    DependencyProperty.Register("myText", typeof(string), typeof(Window1), new PropertyMetadata(null));
        private void Animation()
        {
            from_left1 = 1400;
            from_top1 = 700;
            Storyboard s = (Storyboard)TryFindResource("s1");
            s.Begin();	
       
        }
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            double curx1 = System.Windows.Forms.Cursor.Position.X;
            double cury1 = System.Windows.Forms.Cursor.Position.Y;
          
            RightClickMenu.HorizontalOffset = curx1 - 30;
            RightClickMenu.VerticalOffset = cury1 + 15;
            ContextMenuService.GetContextMenu(button5).IsOpen = true;
            RightClickMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.RelativePoint;
        }
        private void button5_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            ContextMenuService.GetContextMenu(button5).IsOpen = false;
        }

       
    }
      
}
