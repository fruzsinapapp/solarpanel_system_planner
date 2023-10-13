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

public class PythonRunnerTest : MonoBehaviour
{
    [SerializeField] static private GameObject panelPrefab;
    private void Start()
    {
        panelPrefab = Resources.Load<GameObject>("solar");
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
        // Replace with your Azure Function URL
        string functionUrl = "https://pappfruzsinathesis.azurewebsites.net/api/first_function";
        string requestBody = $@"{{
            ""name"": ""Fruzsi"",
            ""dot_1_x"": ""{calcPos1_x}"",
            ""dot_1_y"": ""{calcPos1_y}"",
            ""dot_2_x"": ""{calcPos2_x}"",
            ""dot_2_y"": ""{calcPos2_y}"",
            ""dot_3_x"": ""{calcPos3_x}"",
            ""dot_3_y"": ""{calcPos3_y}"",
            ""dot_4_x"": ""{calcPos4_x}"",
            ""dot_4_y"": ""{calcPos4_y}""
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
    public static void ProcessResponse(string response)
    {

        string[] parts = response.Split(' ');

        List<int> numbers = new List<int>();

        foreach (string part in parts)
        {
            if (int.TryParse(part, out int number))
            {
                numbers.Add(number);
            }
        }
        for (int i = 0; i < numbers.Count; i += 2)
        {
            if (i + 1 < numbers.Count)
            {
                int x = numbers[i] / 100;
                int y = numbers[i + 1] / 100;
                Vector3 panelPosition = new Vector3(x, y);
                GameObject panel = Instantiate(panelPrefab, panelPosition, Quaternion.identity);
            }
        }
        
    }
    /*
    void Start()
    {
        TestTask();
    }
    */
}
