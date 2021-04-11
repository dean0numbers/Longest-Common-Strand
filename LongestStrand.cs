///////////////////////////////////////////////////////////////////////////
/// 
///     This Programm has been written to locate the largest common
///     binary strand within a given set of files.
///     
///     Project:    Longest Stand
///     Author:     Dean Truex
///     Date:       April 9th 2021.
///     


using System;
using System.IO;
using System.Collections.Generic;


namespace LongestStrand
{    class LongestStrand
    {
        #region [Private Data]
        // Building local storage for file set processing
        private List<List<byte>> fileStorage = new List<List<byte>>();
        private List<string> fileName = new List<string>();
        private Boolean processingStatus;
        private Boolean VerboseOutput=false;

        // Store Results in this section
        private int maxStrand = 0;
        // Data Structure used to store results from comparisions
        private struct results
        {
            public string Name;
            public int Offset;
        }

        private results resultFile1, resultFile2;
        #endregion [Private Data]

        #region [Public Methods]
        public void loadFileSet(string strFilename) 
        {
            // Adding Storage space for loading file
            fileStorage.Add(new List<byte>());
            fileName.Add(strFilename);
        }

        public void setOutput(bool flag)
        {
            VerboseOutput = flag;
        }

        public void ProcessSet() 
        {
            loadFiles();

            int iCount = fileName.Count;
            processingStatus = false;

            Console.WriteLine("Processing selected file sets");
            for (int i = 0; i < (iCount-1); i++ )
            {
                for (int j = (i+1); j < iCount; j++)
                {
                    if (VerboseOutput)
                        Console.WriteLine("Processing " + fileName[i] + " and " + fileName[j]);
                    int strandSize = lcStrand(fileStorage[i], fileStorage[j], out int offSet1, out int offSet2);
 
                    if (strandSize > maxStrand)
                    {
                        maxStrand = strandSize;
                        resultFile1.Name = fileName[i];
                        resultFile1.Offset = offSet1;
                        resultFile2.Name = fileName[j];
                        resultFile2.Offset = offSet2;
                    }
                }
                processingStatus = true;
            }
            showResults();
        }
        #endregion [Public Methods]

        #region [Private Methods]
        private void loadFiles()
        {

            BinaryReader binFile;

            // Find out how many files are in the set for loading
            int iCount = fileName.Count;
            if (iCount < 2)
            {
                Console.WriteLine("Insufficient Files Found in File Set for Processing!");
                processingStatus = false;
                return;
            }

            for (int i = 0; i < iCount; i++)
            {
 
                if (VerboseOutput)
                    Console.WriteLine("Loading File " + fileName[i]);

                long size;
                try
                {
                    binFile = new BinaryReader(new FileStream(fileName[i], FileMode.Open));
                    size = 0;

                    while (binFile.BaseStream.Position != binFile.BaseStream.Length)
                    {
                        Byte value = binFile.ReadByte();

                        fileStorage[i].Add(value);
                        size++;
                    }

                   if(VerboseOutput)
                        Console.WriteLine("Bytes Loaded: " + size);
                   // redundant information
                   // Console.WriteLine("Size of storage: " + fileStorage[i].Count);
                    binFile.Close();
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message + "\n Unable to Open Files");
                    processingStatus = false;
                    return;
                }
 
            }
        }
        
        // Showing the results from file sets processed
        private void showResults() 
        {
            // Checking to make sure processing completed properly
            if (!processingStatus)
            {
                Console.WriteLine("Unable to process all files sets!!");
                Console.WriteLine("Processing Terminated without results!");
                return;
            }

            Console.WriteLine("\n\nResults from processed file sets");
            Console.WriteLine("\nLargest matching strand " + maxStrand);
            Console.WriteLine("in " + resultFile1.Name + " at offset " + resultFile1.Offset);
            Console.WriteLine("and in " + resultFile2.Name + " at offset " + resultFile2.Offset);
        }

        // Naive processing uses O(nm^2) time for processing and O(n+m) space {processing takes too long}
        // Dynamic processing uses O(nm) time for processing and O(nm) space {uses too much storage space}
        // lcStrand uses O(2^n) time for processing and O(n+m) space {more optimal solution}
        // Routine has been adapted for use.. 
        private int lcStrand(List<byte> X, List<byte> Y, out int xOffSet, out int yOffSet)
        {
            xOffSet = 0;
            yOffSet = 0;
            // Find length of both Arrays.
            int m = X.Count;
            int n = Y.Count;

            // Variable to store length of longest common strand.
            int result = 0;

            // Matrix to store result of two consecutive rows at a time.
            int[,] len = new int[2, n];

            // Variable to represent which is the current row.
            int currRow = 0;

            // For a particular value of i and j, len[currRow][j] 
            // stores length of longest common strand found in
            // X[0..i] and Y[0..j].
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == 0 || j == 0)
                    {
                        len[currRow, j] = 0;
                    }
                    else if (X[i - 1] == Y[j - 1])
                    {
                        len[currRow, j] = len[(1 - currRow), (j - 1)] + 1;
                        result = Math.Max(result, len[currRow, j]);

                        xOffSet = i - result;
                        yOffSet = j - result;
                    }
                    else
                    {
                        len[currRow, j] = 0;
                    }
                }

                currRow = 1 - currRow;
            }
            return result;
        }
        #endregion [Private Methods]
    }
}
