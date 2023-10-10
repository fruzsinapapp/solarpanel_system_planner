using UnityEditor.Scripting.Python;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PythonRunnerTest : MonoBehaviour
{
    void Start()
    {
        PythonRunner.RunFile("Assets/main.py");
    }
}
