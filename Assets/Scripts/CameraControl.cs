using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl me;
    public int cellsDestroyed;
    private Vector3 defaultPos;
    public Vector2 intensity;
    public float timer;

    private void Awake()
    {
        me = this;
        defaultPos = transform.position;
    }

    void Start()
    {
        cellsDestroyed = 0;
    }

    void Update()
    {
        if (cellsDestroyed > 0)
        {
            timer = cellsDestroyed / 3f / 5f;
            cellsDestroyed = 0;
        }
        if(timer >= 0)
        {
            timer -= Time.deltaTime;
            transform.position = defaultPos + new Vector3(Random.Range(-intensity.x, intensity.x),
                                                          Random.Range(-intensity.y, intensity.y));
        }
        else
        {
            transform.position = defaultPos;
        }
    }
}
