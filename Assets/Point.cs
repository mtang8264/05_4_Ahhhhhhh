using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GoToPoint();
    }

    void GoToPoint()
    {
        Cell point = Grid.me.FindPointer();
        transform.position = point.transform.position;
    }
}
