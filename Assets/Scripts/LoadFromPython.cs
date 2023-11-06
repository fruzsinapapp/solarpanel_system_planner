using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;

public class LoadFromPython : MonoBehaviour
{
    #region Testing python script (without Azure)
    [SerializeField] private GameObject panelPrefab;
    public void LoadFromPythonScript()
    {
        GetPositionsButton getPositionButton  = new GetPositionsButton();
        List<Vector2> dotPositions = getPositionButton.DefineCoordinatesToSendToAzure();
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

                string pythonOutput = process.StandardOutput.ReadToEnd();
                string errorMessage = process.StandardError.ReadToEnd();
                PlaceAccordingToPythonScript(pythonOutput);
                process.WaitForExit();
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log("Error: " + e.Message);
        }
    }
    public void PlaceAccordingToPythonScript(string pythonOutput)
    {
        UnityEngine.Debug.Log(pythonOutput);
        string pattern = @"\((-?\d+), (-?\d+)\)";
        MatchCollection matches = Regex.Matches(pythonOutput, pattern);

        List<float> xCoordinates = new List<float>();
        List<float> yCoordinates = new List<float>();

        foreach (Match match in matches)
        {
            float x = float.Parse(match.Groups[1].Value);
            float y = float.Parse(match.Groups[2].Value);

            xCoordinates.Add(x);
            yCoordinates.Add(y);
            Vector3 panelPosition = new Vector3(x/100, y/100);
            GameObject panel = Instantiate(panelPrefab, panelPosition, Quaternion.identity);
        }
        for (int i = 0; i < 4; i++)
        {
            UnityEngine.Debug.Log("X: " + xCoordinates[i]/100 + ", y: " + yCoordinates[i]/100);
        }   
    }
    #endregion
}
