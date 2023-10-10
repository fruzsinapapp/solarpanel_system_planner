import UnityEngine
import numpy as np


all_objects = UnityEngine.Object.FindObjectsOfType(UnityEngine.GameObject)
for go in all_objects:
    if go.name[-1] != '_' and go.name == "TextTest":
        go.text = "hello"
        go.name = go.name + go.text

