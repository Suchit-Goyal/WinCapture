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
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
namespace Camshot
{
	/// <summary>
	/// Summary description for WindowHighlighter.
	/// </summary>
	public class WindowHighlighter
    {
        /// <summary>
        /// Set foreground window.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd); 
     /// <summary>
     /// Set active window.
     /// </summary>
     /// <param name="hWnd"></param>
     /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
     
        public static extern IntPtr SetActiveWindow(IntPtr hWnd); 
        /// <summary>
        /// Stores the handle of previous window
        /// </summary>
        static  IntPtr previousWindow = IntPtr.Zero; 

        /// <summary>
        /// Stores the handle of current window
        /// </summary>
        static IntPtr currentWindow = IntPtr.Zero; 
        static Rectangle rect = new Rectangle();      //      
      /// <summary>
      ///Highlights the window handle under mouse and Draws rectangle arounds the boundary 
      ///of handle.
      /// </summary>
      /// <param name="hWnd"></param>
		public static void Highlight(IntPtr hWnd)
		{
            System.Windows.Media.Color backGroundColor1 = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFEFBC1C");
            System.Windows.Media.SolidColorBrush brush = new System.Windows.Media.SolidColorBrush(backGroundColor1);
			const float penWidth =3;
			Win32.Rect rc = new Win32.Rect();
            currentWindow = hWnd;
			Win32.GetWindowRect(hWnd, ref rc);
            IntPtr hDC = Win32.GetWindowDC(hWnd);
          IntPtr hDC1 = Win32.GetWindowDC(IntPtr.Zero);
          IntPtr hDC2 = Win32.GetDesktopWindow();
          IntPtr parent = Win32.GetParent(hWnd);
            
				using (System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Gold, penWidth))
				{
                    if (parent != null)
                    {
                        using (Graphics g = Graphics.FromHdc(hDC1))
                        {
                            if (currentWindow != previousWindow)
                            {
                         //       SetForegroundWindow(parent);

                                Win32.GetWindowRect(parent, ref rc);
                                rect.X = Convert.ToInt32(rc.left - penWidth + 1);
                                rect.Y = Convert.ToInt32(rc.top - penWidth + 1);
                                rect.Width = Convert.ToInt32(rc.Width + penWidth);
                                rect.Height = Convert.ToInt32(rc.Height + penWidth);
                                previousWindow = hWnd;
                                g.DrawRectangle(pen, rect);
                                g.Dispose();
                                g.ReleaseHdc(hDC1);
                          //      Refresh(IntPtr.Zero);
                            }
                        }
                    }
                    else
                    {
                        using (Graphics g = Graphics.FromHdc(hDC1))
                        {
                            if (currentWindow != previousWindow)
                            {
                             //   SetForegroundWindow(hWnd);
                                rect.X = Convert.ToInt32(rc.left - penWidth + 1);
                                rect.Y = Convert.ToInt32(rc.top - penWidth + 1);
                                rect.Width = Convert.ToInt32(rc.Width + penWidth);
                                rect.Height = Convert.ToInt32(rc.Height + penWidth);
                                previousWindow = hWnd;
                                g.DrawRectangle(pen, rect);
                                g.Dispose();
                                g.ReleaseHdc(hDC1);
                        //        Refresh(IntPtr.Zero);
                            }
                        }
                    }
				}
            Win32.ReleaseDC(IntPtr.Zero ,hDC1);
        //    Refresh(IntPtr.Zero);
		}

      /// <summary>
      /// Refreshes the provided handle and removes the rectangle drawn.
      /// </summary>
      /// <param name="hWnd"></param>

		public static void Refresh(IntPtr hWnd)
		{
			Win32.InvalidateRect(hWnd, IntPtr.Zero, 1 /* TRUE */);
			Win32.UpdateWindow(hWnd);
			Win32.RedrawWindow(hWnd, IntPtr.Zero, IntPtr.Zero, Win32.RDW_FRAME | Win32.RDW_INVALIDATE | Win32.RDW_UPDATENOW | Win32.RDW_ALLCHILDREN);		
		}
       

	}
  
}
