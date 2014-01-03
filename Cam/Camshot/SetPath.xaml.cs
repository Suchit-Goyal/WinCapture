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
using System.Windows.Forms;
using System.IO.IsolatedStorage;

namespace Camshot
{
    /// <summary>
    /// Interaction logic for SetPath.xaml
    /// </summary>
    public partial class SetPath : Window
    {
        static FolderBrowserDialog changepath = new FolderBrowserDialog();
        public static MainWindow m = System.Windows.Application.Current.MainWindow as MainWindow;



        public SetPath()
        {
            InitializeComponent();
            MainWindow.LoadBool();
            if (MainWindow.check_image == "False")
            {
                string path1 = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\ImageCapture\";

                this.textBox1.Text = path1;

                this.textBox2.Text = path1;

            }
            else
            {
                MainWindow.LoadSettings();
                this.textBox1.Text = MainWindow.temp_path_image;
                string path1 = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\ImageCapture\";
                this.textBox2.Text = path1;

            }
            if (MainWindow.check_video == "False")
            {

                string path2 = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\VideoCapture\";

                this.textBox3.Text = path2;

                this.textBox4.Text = path2;
            }
            else
            {
                MainWindow.LoadSettings();
                string path2 = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\VideoCapture\";
                this.textBox3.Text = MainWindow.temp_path_video;

                this.textBox4.Text =path2;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.isImagePathChanged = true;
            MainWindow.temp_path2 = textBox3.Text;
            DialogResult result = changepath.ShowDialog();
            MainWindow.temp_path1 = changepath.SelectedPath;
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                MainWindow.isImagePathChanged = true;
            }
            m.SavePathToDisk();
            this.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.isVideoPathChanged = true;
            MainWindow.temp_path1 = textBox1.Text;
            DialogResult result = changepath.ShowDialog();
            MainWindow.temp_path2 = changepath.SelectedPath;
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                MainWindow.isVideoPathChanged = true;
            }
            m.SavePathToDisk();
            this.Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            string path1 = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\ImageCapture\";
            this.textBox1.Text = path1;
            MainWindow.temp_path1 = path1;
            m.SavePathToDisk();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            string path2 = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\VideoCapture\";

            this.textBox3.Text = path2;
            MainWindow.temp_path2 = path2;
            m.SavePathToDisk();
        }
    }
}
