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

namespace LongestStrand
{
    class Program
    { 
        // Program entry point
        static void Main(string[] args)
        {
            string strFilename = "sample.";
            Console.WriteLine("Finding the Longest strand");

            LongestStrand ls = new LongestStrand();

            for (int i = 1; i < 11; i++)
                ls.loadFileSet(strFilename + i);

            ls.setOutput(false);
            ls.ProcessSet();
        }
    }
}
