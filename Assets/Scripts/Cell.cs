using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int pos;
    public enum Type { RED, ORANGE, BLUE, YELLOW, GREEN, PURPLE, POINT };
    public Type type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 goalPos = new Vector3(pos.x + Grid.me.offset.x, pos.y + Grid.me.offset.y);
        transform.position = Vector3.Lerp(transform.position, goalPos, 0.1f);
    }
}
