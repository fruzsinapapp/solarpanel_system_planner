import UnityEngine
import numpy as np


all_objects = UnityEngine.Object.FindObjectsOfType(UnityEngine.GameObject)
for go in all_objects:
    if go.name[-1] != '_' and go.name == "Cube":
        go.name = go.name + '_'
        go.transform.position = UnityEngine.Vector3(0, 200, 0)
