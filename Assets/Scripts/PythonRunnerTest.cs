using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using UnityEngine.Windows;
using System.Text.RegularExpressions;

public class PythonRunnerTest : MonoBehaviour
{
    [SerializeField] static private GameObject panelPrefab;
    private void Start()
    {
        panelPrefab = Resources.Load<GameObject>("CustomSolarPanel");
    }
    public void CallableFunction()
    {
        GetPositionsButton getPositionButton = new GetPositionsButton();
        List<Vector2> dotPositions = getPositionButton.GetPositionsForPython();
        TestTask(dotPositions);
    }
    
    public static async Task TestTask(List<Vector2> dotPositions)
    {
        List<Vector2> positions = dotPositions;
        UnityEngine.Debug.Log(positions[0][0] + " " + positions[0][1] + " " + positions[1][0] + " " + positions[1][1]);
        string calcPos1_x = ((int)(positions[0][0] * 100)).ToString();
        string calcPos1_y = ((int)(positions[0][1] * 100)).ToString();
        string calcPos2_x = ((int)(positions[1][0] * 100)).ToString();
        string calcPos2_y = ((int)(positions[1][1] * 100)).ToString();
        string calcPos3_x = ((int)(positions[2][0] * 100)).ToString();
        string calcPos3_y = ((int)(positions[2][1] * 100)).ToString();
        string calcPos4_x = ((int)(positions[3][0] * 100)).ToString();
        string calcPos4_y = ((int)(positions[3][1] * 100)).ToString();
        UnityEngine.Debug.Log(calcPos1_x + " " + calcPos1_y + " " + calcPos2_x + " " + calcPos2_y + calcPos3_x + " " + calcPos3_y + " " + calcPos4_x + " " + calcPos4_y);

        string functionUrl = "https://pappfruzsinathesis.azurewebsites.net/api/first_function";
        string requestBody = $@"{{
            ""name"": ""Fruzsi"",
            ""calcPos1_x"": ""{calcPos1_x}"",
            ""calcPos1_y"": ""{calcPos1_y}"",
            ""calcPos2_x"": ""{calcPos2_x}"",
            ""calcPos2_y"": ""{calcPos2_y}"",
            ""calcPos3_x"": ""{calcPos3_x}"",
            ""calcPos3_y"": ""{calcPos3_y}"",
            ""calcPos4_x"": ""{calcPos4_x}"",
            ""calcPos4_y"": ""{calcPos4_y}""
            }}";
        UnityEngine.Debug.Log(requestBody);
        using (HttpClient client = new HttpClient())
        {       
            HttpResponseMessage response = await client.PostAsync(functionUrl, new StringContent(requestBody, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                // Handle a successful response
                string responseContent = await response.Content.ReadAsStringAsync();
                UnityEngine.Debug.Log("Azure Function response: " + responseContent);
                ProcessResponse(responseContent);
            }
            else
            {
                UnityEngine.Debug.Log("Error: " + response.StatusCode);
            }
        }
    }
    
    static List<float> ExtractNumbers(string input)
    {
        List<float> numbers = new List<float>();
        string pattern = @"[0-9.]+";

        MatchCollection matches = Regex.Matches(input, pattern);

        foreach (Match match in matches)
        {
            if (float.TryParse(match.Value, out float number))
            {
                numbers.Add(number);
            }
        }

        return numbers;
    }

    public static void ProcessResponse(string response)
    {
        UnityEngine.Debug.Log("RESPONSE: " + response);

        List<float> numbers2 = ExtractNumbers(response);

        foreach (double number in numbers2)
        {
            UnityEngine.Debug.Log(number);
        }

        for (int i = 0; i < numbers2.Count; i += 2)
        {
            if (i + 1 < numbers2.Count+1)
            {
                float x = numbers2[i] / 100f;
                float y = numbers2[i + 1] / 100f;

                x += 0.2f;


                float oldZ = ProjectionCode.originalZPosition;
                float oldAngle = ProjectionCode.originalAngleTwoDots;

                UnityEngine.Debug.Log("Old Z: " + ProjectionCode.originalAngleTwoDots);
                UnityEngine.Debug.Log("Old angle: " + oldAngle);
                UnityEngine.Debug.Log("Old angle 2: " + ProjectionCode.originalAngleTwoDots);

                if (oldAngle > 90)
                    oldAngle -= 90;
                else
                    oldAngle = 90 - oldAngle;
                Quaternion rotation = Quaternion.Euler(oldAngle, 0f, 0f);

                Vector3 panelPosition = new Vector3(x, y);
                Vector3 newCoordinates = PenTool.zeroMarkerScript.UseRealCoordinatesVector(panelPosition);

                newCoordinates.z = oldZ-(1.2f);

                UnityEngine.Debug.Log("new z: " + newCoordinates.z);
                GameObject panel = Instantiate(panelPrefab, newCoordinates, rotation);
            }
        }
        
    }
}
