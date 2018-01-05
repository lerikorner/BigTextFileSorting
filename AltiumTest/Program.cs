﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AltiumTest
{
    class Program
    {
        static int sliceSize = 8900;
        static Int32 fileSize = 9000;
        public static void Main()
        {
            string fileName = "c:\\temp\\out_small.txt";
            int fsize = 0;
            List<string> strBlock = new List<string>();
            //создаем случайные списки и пишем их в файл поблочно
            while (fsize < fileSize)
            {             
                if (fileSize % sliceSize == 0)
                {
                    fsize += sliceSize;
                    strBlock = FileManager.StringListRandomizer(sliceSize);
                    File.AppendAllLines(fileName, strBlock);
                }
                else if (fileSize - fsize < sliceSize)
                {
                    fsize += fileSize % sliceSize;
                    strBlock = FileManager.StringListRandomizer(fileSize % sliceSize);
                    File.AppendAllLines(fileName, strBlock);
                }
                else
                {
                    fsize += sliceSize;
                    strBlock = FileManager.StringListRandomizer(sliceSize);
                    File.AppendAllLines(fileName, strBlock);
                }
            }
             
            
            //FileManager.FileFromList(fileName, strBlock, false);

            var sWatch = System.Diagnostics.Stopwatch.StartNew();           

            //делим файл на куски заданного размера           

            if (sliceSize <fileSize)
            {
                int counter = FileManager.FileSplit("c:\\temp\\out_small.txt", fileSize, sliceSize);
                Console.WriteLine("количество временных файлов: {0}", counter);

                //просеиваем строки в кусках и пишем в итоговый файл
                //FileManager.MergeSortedFile("c:\\temp\\splits\\out_slice", counter, fileName, "c:\\temp\\out_merged_sorted.txt");
                FileManager.MergeByQueues("c:\\temp\\splits\\", fileName, "c:\\temp\\out_merged_sorted.txt");
            }
            else
            {
                //считываем все строки файла в массив
               string[] stringbuf = File.ReadAllLines(fileName);

                List<string> trSorted = Sorting.TRSortedtoStrings(stringbuf.ToList<string>());

                //пишем сортированные данные в файл
                fileName = "c:\\temp\\out_small_sorted.txt";

                FileManager.FileFromList(fileName, trSorted, true);               
            }
            sWatch.Stop();
            Console.WriteLine("затрачено времени: {0}", sWatch.Elapsed);
            Console.WriteLine("количество строк в файле: {0}", FileManager.FileSizeinStrings(fileName));

            Console.ReadKey();           
        }
    }
}
