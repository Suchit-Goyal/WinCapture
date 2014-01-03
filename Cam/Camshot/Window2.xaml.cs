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
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;
using System.Diagnostics;
using System.IO;

namespace Camshot
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public static Bitmap bmp;
        public string SavedPath , openPath;
        public bool isSaved = false;
     

        public Window2()
        {
            InitializeComponent();
              isSaved = false;
             var k = App.Current as App;
             if (k.PicChecked == true)
             {
                 this.textBlock1.Text = MainWindow.globalPath;
                 this.button2.Content = "Open With Paint";
             }
             else
                 if (k.VideoChecked == true)
                 {
                     this.textBlock1.Text = MainWindow.globalVideoPath;
                     this.button2.Content = "Open With WMP";
                 }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
             var k = App.Current as App;
             if (k.PicChecked == true)
             {
                 
            
                 SaveFileDialog saveDialog = new SaveFileDialog();
                 saveDialog.FileName = MainWindow.globalPath;
                 saveDialog.Filter = "PNG Image|*.png";
                 saveDialog.Title = "Save Image as";
                 saveDialog.ShowDialog();
                 if (bmp != null)
                 {
                     if (saveDialog.FileName != string.Empty)
                     {
                         SavedPath = saveDialog.InitialDirectory;
                         openPath = saveDialog.FileName;
                        bmp.Save(saveDialog.FileName, ImageFormat.Png);
                        isSaved = true;
                     }
                 }
                 else
                 {
                     System.Windows.Forms.MessageBox.Show("No File To Save");
                 }
             }
             else
                 if (k.VideoChecked == true)
                 {
                     try
                     {
                         
                             SaveFileDialog dialog = new SaveFileDialog();
                             dialog.FileName = MainWindow.globalVideoPath;
                             dialog.Title = "Save Video as";
                             dialog.Filter = "Video Files (*"+MainWindow.outputformat+")|*" + MainWindow.outputformat;
                          
                             DialogResult res = dialog.ShowDialog();
                             if (res != System.Windows.Forms.DialogResult.Cancel)
                             {
                                 if (File.Exists(MainWindow.globalVideoPath))
                                 {
                                     isSaved = true;
                                     SavedPath = dialog.InitialDirectory;
                                     openPath = dialog.FileName;
                                     File.Copy(MainWindow.globalVideoPath, dialog.FileName);
                                     File.Delete(MainWindow.globalVideoPath);
                                 }
                             }
                            
                     }
                     catch
                     {
                         System.Windows.Forms.MessageBox.Show("Can't Stop it.Please,Save Current Recording and Restart Application.");
                     }
                 }
        
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var k = App.Current as App;
            if (k.PicChecked == true)
            {
                if (isSaved == true)
                {

                    Process.Start("mspaint", openPath);
                }
                else
                {
                    Process.Start("mspaint", MainWindow.globalPath);

                }
            }
            else
                if (k.VideoChecked == true)
                {
                    if (isSaved == true)
                    {
                        Process.Start("wmplayer.exe", openPath);
                    }
                    else
                    {
                        Process.Start("wmplayer.exe", MainWindow.globalVideoPath);
                    }
                }


        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            OpenDir();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (isSaved == true)
            {
                System.Diagnostics.Process.Start(openPath);
            }
            else
            {
                OpenDir();

            }
        }
        public void OpenDir()
        {
            MainWindow.LoadBool();
            var k = App.Current as App;
            string path;
            if (k.PicChecked == true)
            {
                if (MainWindow.check_image == "false")
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\ImageCapture\";
                    System.Diagnostics.Process.Start(path);
                }
                else
                {
                    MainWindow.LoadSettings();

                    path = MainWindow.temp_path_image;
                }
                System.Diagnostics.Process.Start(path);
            }
            else
            {

                path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\VideoCapture\";
                System.Diagnostics.Process.Start(path);

            }
        }
    }
}
