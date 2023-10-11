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

public class PythonRunnerTest : MonoBehaviour
{
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
        // Replace with your Azure Function URL
        string functionUrl = "https://pappfruzsinathesis.azurewebsites.net/api/first_function";
        string requestBody2 = @"{
            ""name"": ""Fruzsi""
            }";
        string requestBody = "{\"name\": \"Fruzsi\"}";
        using (HttpClient client = new HttpClient())
        {       
            HttpResponseMessage response = await client.PostAsync(functionUrl, new StringContent(requestBody2, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                // Handle a successful response
                string responseContent = await response.Content.ReadAsStringAsync();
                UnityEngine.Debug.Log("Azure Function response: " + responseContent);
            }
            else
            {
                UnityEngine.Debug.Log("Error: " + response.StatusCode);
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
