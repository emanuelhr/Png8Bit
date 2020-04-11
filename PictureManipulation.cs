﻿using nQuant;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Png8Bit
{
    public static class PictureManipulation
    {
       public enum Filters
        {
            png,
            jpg,
            tif

        }


        //public static string FilterPaths(string path, Filters filter)
        //{ 
        //    return 

        //}

        public static void ConvertPicture(string path, bool isFolder=true)
        {
            string[] allFiles = Directory.GetFiles(path+"\\", "*.*", SearchOption.AllDirectories);

            foreach (var file in allFiles)
            {
                FileInfo fileInfo = new FileInfo(file);
                var newPath = new StringBuilder(file);
                newPath.Remove(newPath.Length - 3, 3);
                newPath.Append("png");
                var originalImage = new Bitmap(file);
                if (originalImage.Size != new Size(400, 400) & originalImage.PixelFormat != PixelFormat.Format32bppArgb)
                {
                    Bitmap newImage = OriginalImageScale(originalImage, 400, 400);
                    if (newPath.ToString() == file)
                    {
                        for (int tries = 0; IsFileLocked(fileInfo) && tries < 5; tries++)
                        {
                            Thread.Sleep(1000);
                            Console.WriteLine("Waiting");
                        }

                        File.Delete(newPath.ToString());
                    }

                    To8BitPng(newPath.ToString(), newImage);
                    Console.WriteLine("Converted picture");
                }

            }
        }
        public static void ConvertPicture(string path)
        {
            string[] allFiles = Directory.GetFiles(@"D:\Programiranje_slike\", "*.*", SearchOption.AllDirectories);

            foreach (var file in allFiles)
            {
                FileInfo fileInfo = new FileInfo(file);
                var newPath = new StringBuilder(file);
                newPath.Remove(newPath.Length - 3, 3);
                newPath.Append("png");
                var originalImage = new Bitmap(file);
                if (originalImage.Size != new Size(400, 400) & originalImage.PixelFormat != PixelFormat.Format32bppArgb)
                {
                    Bitmap newImage = OriginalImageScale(originalImage, 400, 400);
                    if (newPath.ToString() == file)
                    {
                        for (int tries = 0; IsFileLocked(fileInfo) && tries < 5; tries++)
                        {
                            Thread.Sleep(1000);
                            Console.WriteLine("Waiting");
                        }

                        File.Delete(newPath.ToString());
                    }

                    To8BitPng(newPath.ToString(), newImage);
                    Console.WriteLine("Converted picture");
                }

            }
        }


        public static void To8BitPng(string path, Bitmap image)
        {
            var quantizer = new WuQuantizer();
            using (var quantized = quantizer.QuantizeImage(image))
            {
                quantized.Save(path, ImageFormat.Png);
            }
        }

        //returns original image scaled
        public static Bitmap OriginalImageScale(Bitmap image, float width, float height)
        {
            float scale = Math.Min(width / image.Width, height / image.Height);
            var brush = new SolidBrush(Color.White);
            var bmp = new Bitmap((int)width, (int)height);
            using (var graph = Graphics.FromImage(bmp))
            {
                graph.InterpolationMode = InterpolationMode.High;
                graph.CompositingQuality = CompositingQuality.HighQuality;
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                var scaleWidth = (int)(image.Width * scale);
                var scaleHeight = (int)(image.Height * scale);
                graph.FillRectangle(brush, new RectangleF(0, 0, width, height));
                graph.DrawImage(image, ((int)width - scaleWidth) / 2, ((int)height - scaleHeight) / 2, scaleWidth, scaleHeight);
            }
            return bmp;

        }


        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }



    }
}