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
    static async Task TestTask()
    {
        // Replace with your Azure Function URL
        string functionUrl = "https://pappfruzsinathesis.azurewebsites.net/api/first_function";
        string requestBody = "{\"name\": \"Fruzsi\"}";
        using (HttpClient client = new HttpClient())
        {       
            HttpResponseMessage response = await client.PostAsync(functionUrl, new StringContent(requestBody, Encoding.UTF8, "application/json"));

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
    void Start()
    {
        TestTask();
    }
}
