
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
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Shapes;

namespace Camshot
{
    class ActiveWindow
    {
        public static string format = "MMM_ddd_yyyy_HH-mm-ff"; 
        public enum CaptureMode
        {
            Screen, Window
        }
          [DllImport("user32.dll")]
          private static extern IntPtr GetForegroundWindow();

          [DllImport("user32.dll")]
          private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
          [StructLayout(LayoutKind.Sequential)]
          private struct Rect {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
          }

          [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
          public static extern IntPtr GetDesktopWindow();

     
          public static void CaptureAndSave(CaptureMode mode = CaptureMode.Window ) {
      
                ImageSave(Capture(mode));
          }
          public static Bitmap Capture(CaptureMode mode = CaptureMode.Window) {
                return Capture(mode == CaptureMode.Screen ? GetDesktopWindow() : GetForegroundWindow());
          }
       //   public static Bitmap Capture(Control c) {
         //       return Capture(c.Handle);
          //}
          public static Bitmap Capture(IntPtr handle) {
                    
             
                  System.Drawing.Rectangle bounds;
                  var rect = new Rect();
                  GetWindowRect(handle, ref rect);
                  bounds = new System.Drawing.Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

                  //   MessageBox.Show(rect.Left + " ," + rect.Top + "," +temp + "," + temp1 );

                  CursorPosition = new Point(Cursor.Position.X - rect.Left, Cursor.Position.Y - rect.Top);

              
                 var result = new Bitmap(bounds.Width, bounds.Height);
            
                  using (var g = Graphics.FromImage(result))
                  {
                      g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                      Window2.bmp = result;
                      MainWindow.bmp = result;
                  }
                  //   MessageBox.Show(CursorPosition.X + " " + CursorPosition.Y);
                  return result;
            
             
          }
     
          public static Point CursorPosition;

          static void ImageSave(Image image) {
              MainWindow main = System.Windows.Application.Current.MainWindow as MainWindow;

              try
              {
                  DateTime saveNow = DateTime.Now;
                  MainWindow.LoadBool();
                  string path;
                  var k = App.Current as App;
                  if (MainWindow.check_image == "false")
                  {
                      path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\ImageCapture\" + saveNow.ToString(format) + ".png";
                      MainWindow.globalPath = path;
                  }
                  else
                  {
                      MainWindow.LoadSettings();
                      path = MainWindow.temp_path_image  +@"\" + saveNow.ToString(format) + ".png";
                      MainWindow.globalPath = path;
                  }
                  
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
                  System.Windows.Forms.MessageBox.Show("Invalid Path");
              }
          }
     }
}
