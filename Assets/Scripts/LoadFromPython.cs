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
        GetPositionsButton getPositionButton  = new GetPositionsButton();
        List<Vector2> dotPositions = getPositionButton.GetPositionsForPython();
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = @"C:\\Python311\\python.exe",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        string arguments = @"C:\\Users\\fruzs\\Code\\Szakdolgozat\\knapsack-packing\\KnapsackPacking\\problem_experiments.py";
        
        foreach(var dot in dotPositions)
        {
            arguments += " ";
            arguments += dot.x;
            arguments += " ";
            arguments += dot.y;
        }
        psi.Arguments = arguments;
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
            UnityEngine.Debug.Log("Error: " + e.Message);
        }
    }
}
