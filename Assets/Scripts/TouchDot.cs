using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDot : MonoBehaviour
{
    GameObject myGameObject;
    // Start is called before the first frame update
    void Start()
    {
        myGameObject = gameObject;
    }
    public void TouchDotWithHand()
    {
        Selection s = new Selection();
        s.SelectDotAfterTheFirst(myGameObject);
    }
}
