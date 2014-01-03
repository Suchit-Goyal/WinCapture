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

namespace Camshot
{
    public class LoadSettings
    {


       public LoadSettings()
        {
            var k = App.Current as App;
            MainWindow m = Application.Current.MainWindow as MainWindow;
            if (k.PicChecked == true)
            {
                m.PicCapture.IsChecked = true;
                m.VideoCapture.IsChecked = false;
            }
            else
                if(k.VideoChecked == true)
            {
                m.VideoCapture.IsChecked = true;
                m.PicCapture.IsChecked = false;
            }


            if (MainWindow.whichChecked == "customscreen")
            {
                m.GrabScreenShot.IsChecked = false;
                m.CustomScreenSize.IsChecked = true;
                m.TopWindow.IsChecked = false;
                MainWindow.whichChecked = "customscreen";
            }
            else
                if (MainWindow.whichChecked == "topwindow")
                {
                    m.GrabScreenShot.IsChecked = false;
                    m.CustomScreenSize.IsChecked = false;
                    m.TopWindow.IsChecked = true;
                    MainWindow.whichChecked = "topwindow";
                }
                else
                    if (MainWindow.whichChecked == "fullscreen")
                {
                    m.GrabScreenShot.IsChecked = true;
                    m.CustomScreenSize.IsChecked = false;
                    m.TopWindow.IsChecked = false;
                    MainWindow.whichChecked = "fullscreen";
                }

        
        }
    }
}
