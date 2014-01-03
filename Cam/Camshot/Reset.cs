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
using System.IO.IsolatedStorage;
using System.Windows;

namespace Camshot
{
    class Reset
    {
        MainWindow m = Application.Current.MainWindow as MainWindow;
        public Reset()
        {
            m.PicCapture.IsChecked = true;
            Reset_Path();
            Reset_VideoFormat();
            Reset_Mode();
            MainWindow.isShowDialog = false;
           
           
        }
        public void Reset_Path()
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            isoStore.DeleteFile("a.ini");
            isoStore.DeleteFile("settings.ini");
            isoStore.DeleteFile("bool.ini");
            MainWindow.check_image = "false";
            MainWindow.check_video = "false";
        }
        public void Reset_VideoFormat()
        {
            m.Wmv.IsChecked = true;
        }
        public void Reset_Mode()
        {
            m.CustomScreenSize.IsChecked = true;
        }

    }
}
