﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Png8Bit
{
    public class FilteredList
    {
        private readonly string[] _allFiles;
        private readonly List<string> _fitlers;

        public FilteredList(string[] allFiles, List<string> fitlers)
        {
            _allFiles = allFiles;
            _fitlers = fitlers;
        }

        public enum Filters
        {
            png,
            jpg,  
            tif
        }

        public async Task<List<string>> FilterPaths()
        {
            List<string> filteredFiles = new List<string>();
            // string[] allFiles = Directory.GetFiles(path + "\\", "*.*", SearchOption.AllDirectories);
            foreach (var file in _allFiles)
            {
                //check filters

                foreach (var filter in _fitlers)
                {
                    if (file.Contains("." + filter.ToString()))
                    {
                        //add to list
                        filteredFiles.Add(file);
                    }
                }
            }
            //return filteredFiles;
            return await Task.FromResult(filteredFiles);
        }
    }
}