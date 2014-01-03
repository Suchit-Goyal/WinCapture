using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

using System.Windows;

namespace Camshot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public int i
        {
            get;
            set;
        }

        public bool isPlaying
        {
            get;
            set;
        }
        public bool PicChecked
        {
            get;
            set;
        }
        public bool VideoChecked
        {
            get;
            set;
        }
      
    }
    
}
