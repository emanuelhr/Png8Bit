using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
            label2.AllowDrop = true;

            label2.Drop += listBoxFiles_DragEnter;
            label2.Drop += List_drop_Drop;
            list_drop.Drop += listBoxFiles_DragEnter;

            list_drop.Drop += List_drop_Drop;
            list_drop.KeyDown += List_drop_KeyDown;
            btn_delete.Click += Btn_delete_Click;
            converted.Content = "";


            tbox_additionalFormats.GotFocus += Tbox_additionalFormats_GotFocus;
            tbox_additionalFormats.LostFocus += Tbox_additionalFormats_LostFocus;
           
        }

        #region Event Handlers

        #endregion
        private void Tbox_additionalFormats_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbox_additionalFormats.Text))
            {
                tbox_additionalFormats.Text = "Add..";
            }
        }

        private void Tbox_additionalFormats_GotFocus(object sender, RoutedEventArgs e)
        {
            tbox_additionalFormats.Text = "";
        }

        private void Btn_delete_Click(object sender, RoutedEventArgs e)
        {
            list_drop.Items.Remove(list_drop.SelectedItem);
        }

        private void List_drop_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                list_drop.Items.Remove(list_drop.SelectedItem);
            }
        }

        private void List_drop_Drop(object sender, DragEventArgs e)
        {
           
            label2.Visibility = Visibility.Hidden;
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


        private async Task ConvertAsync()
        {
            converted.Content = "";
            tbox_output.Clear();
            //Add filters
            var filters = new List<string>();
            if (chk_jpg.IsChecked == true) filters.Add("jpg");
            if (chk_png.IsChecked == true) filters.Add("png");
            if (chk_tif.IsChecked == true) filters.Add("tif");

            foreach (var item in lbox_Formats.Items)
            {
                filters.Add(item.ToString());
            }
            //Loop trough each file/folder in drop list
            int count = 0;
            foreach (var file in list_drop.Items)
            {
                //if it is a directory use directory convert
                if (Directory.Exists((string)file))
                {
                    string[] allFiles = Directory.GetFiles((string)file + "\\", "*.*", SearchOption.AllDirectories);
                    var filteredList = new FilteredList(allFiles, filters);

                    foreach (var item in await filteredList.FilterPaths())
                    {
                        if (await PictureManipulation.ConvertPicture(item))
                        {
                            tbox_output.Text += "Converted: " + item + "\n";
                            count++;
                        }
                    }
                }
                //if it is a file use file convert
                if (File.Exists((string)file))
                {
                    if (await PictureManipulation.ConvertPicture((string)file))
                    {
                        tbox_output.Text += "Converted: " + file + "\n";
                        count++;
                    }
                }

                if (tbox_output.Text.Length == 0)
                {
                    tbox_output.Text += "No files were converted";
                }
            }
            converted.Content += "Converted : " + count + " Pictures";

        }


        private async void btn_convert_Click(object sender, RoutedEventArgs e)
        {
            await ConvertAsync();
        }

        private void btn_AddFormat_Click(object sender, RoutedEventArgs e)
        {
            lbox_Formats.Items.Add(tbox_additionalFormats.Text);
        }

        private void label2_Drop(object sender, DragEventArgs e)
        {

        }
    }
}