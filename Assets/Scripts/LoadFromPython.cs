using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class LoadFromPython : MonoBehaviour
{
    public void LoadFromPythonScript()
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = @"C:\\Python311\\python.exe",
            Arguments = @"C:\\Users\\fruzs\\Code\\Szakdolgozat\\knapsack-packing\\KnapsackPacking\\problem_experiments.py test",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        try
        {
            using (Process process = new Process())
            {
                process.StartInfo = psi;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string errorMessage = process.StandardError.ReadToEnd();
                UnityEngine.Debug.Log(output);
                process.WaitForExit();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }

    }
}
