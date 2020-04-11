using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Png8Bit;

namespace Png8Bit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            list_drop.AllowDrop = true;  
          
            list_drop.Drop += listBoxFiles_DragEnter;

        }


        private void listBoxFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effects = DragDropEffects.Copy;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
                list_drop.Items.Add(file);
        }
        
        private void btn_convert_Click(object sender, RoutedEventArgs e)
        {
            
            foreach (var file in list_drop.Items)
            {
                
                if (Directory.Exists((string)file))
                {
                    tbox_output.Text += "Directory" + "\n";
                }
                if (File.Exists((string)file))
                {
                 tbox_output.Text += "File" + "\n";
                }
                tbox_output.Text += (string)file + "\n";

              //  PictureManipulation.ConvertPicture(path);
            }



        }
    }
}
