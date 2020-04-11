using System.Collections.Generic;
using System.IO;
using System.Windows;
using static Png8Bit.FilteredList;

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
            {
                list_drop.Items.Add(file);
                
            }
        }

        private void btn_convert_Click(object sender, RoutedEventArgs e)
        {
            tbox_output.Clear();
            //Add filters
            var filters = new List<Filters>();
            if (chk_jpg.IsChecked == true) filters.Add(Filters.jpg);
            if (chk_png.IsChecked == true) filters.Add(Filters.png);
            if (chk_tif.IsChecked == true) filters.Add(Filters.tif);
            //Loop trough each file/folder in drop list
            foreach (var file in list_drop.Items)
            {
                //if it is a directory use directory convert
                if (Directory.Exists((string)file))
                {
                    string[] allFiles = Directory.GetFiles((string)file + "\\", "*.*", SearchOption.AllDirectories);
                    var filteredList = new FilteredList(allFiles, filters);

                    foreach (var item in filteredList.FilterPaths())
                    {
                        if (PictureManipulation.ConvertPicture(item))
                        {
                            tbox_output.Text += "Converted" + item + "\n";
                        }
                    }
                }
                //if it is a file use file convert
                if (File.Exists((string)file))
                {
                    if (PictureManipulation.ConvertPicture((string)file))
                    {
                        tbox_output.Text += "Converted" + file + "\n";
                    }
                }

                if (tbox_output.Text.Length==0)
                {
                    tbox_output.Text += "No files were converted";
               }
            }
        }
    }
}