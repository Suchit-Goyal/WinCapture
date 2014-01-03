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

namespace Camshot
{
    /// <summary>
    /// Interaction logic for menu.xaml
    /// </summary>
    public partial class menu : UserControl
    {
        public menu()
        {
            InitializeComponent();
        //    StartStop.IsEnabled = true;
           // StartStop.Background = Brushes.Green;

            image2.IsEnabled = false;
           
        //     PauseResume.Background = brush1;
    //        Stop.IsEnabled = false;
        }

        private void image1_MouseEnter(object sender, MouseEventArgs e)
        {
            var k =App.Current as App;
            if (k.isPlaying ==false)
            {
                image1.Source = new BitmapImage(new Uri("/Camshot;component/Images/play-btn_hover.png", UriKind.Relative));
            }
            else
                if (k.isPlaying == true)
                {
                    if (Window3.isPause ==false)
                    {
                        image1.Source = new BitmapImage(new Uri("/Camshot;component/Images/pause-btn_hover.png", UriKind.Relative));
                    }
                    else
                        if (Window3.isPause == true)
                        {
                            image1.Source = new BitmapImage(new Uri("/Camshot;component/Images/play-btn_hover.png", UriKind.Relative));

                        }
                }
          //  image1.ToolTip = "Stop And Save Video";
        }

        private void image2_MouseEnter(object sender, MouseEventArgs e)
        {
            image2.Source = new BitmapImage(new Uri("/Camshot;component/Images/stop-btn_hover.png", UriKind.Relative));
        }

        private void image1_MouseLeave(object sender, MouseEventArgs e)
        {
            var k = App.Current as App;
            if (k.isPlaying == false)
            {
                image1.Source = new BitmapImage(new Uri("/Camshot;component/Images/play-btn.png", UriKind.Relative));
            }
            else
                if (k.isPlaying == true)
                {
                    if (Window3.isPause == false)
                    {

                        image1.Source = new BitmapImage(new Uri("/Camshot;component/Images/pause-btn.png", UriKind.Relative));

                    }
                    else
                        if (Window3.isPause == true)
                        {
                            image1.Source = new BitmapImage(new Uri("/Camshot;component/Images/play-btn.png", UriKind.Relative));
                        }
                }
        }

        private void image2_MouseLeave(object sender, MouseEventArgs e)
        {
            image2.Source = new BitmapImage(new Uri("/Camshot;component/Images/stop-btn.png", UriKind.Relative));
        }

        private void image3_MouseEnter(object sender, MouseEventArgs e)
        {
            image3.Source = new BitmapImage(new Uri("/Camshot;component/Images/close-btn_hover.png", UriKind.Relative));
        }

        private void image3_MouseLeave(object sender, MouseEventArgs e)
        {
            image3.Source = new BitmapImage(new Uri("/Camshot;component/Images/close-btn1.png", UriKind.Relative));
        }

    

       
        }

       
    
    }

