﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int pos;
    public Vector3 goalWorldPos;
    public Vector3 frameGoal;
    public enum Type { RED, BLUE, YELLOW, GREEN, POINT };
    public Type type;
    SpriteRenderer rend;
    public float lerpAmount = 0.1f;
    public static float lerpDamp = 0.73f;
    public static float stretchMulti = 2f;
    public static float stretchReductionMulti = 0.75f;
    public bool atDesiredPosition;
    public bool debug;

    [Header("ROTATION TOWN")]
    private float averageRotation = 10;
    private float rotationVariance = 0.2f;
    private float currentRotation;
    private bool direction;
    private float averageSpeed = 5;
    private float speedVariance = 0.9f;
    private float currentSpeed;
    public static bool doRotate = false;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        Move();
        transform.position = new Vector3(goalWorldPos.x, goalWorldPos.y + 20);

        currentRotation = Random.Range(averageRotation - rotationVariance, averageRotation + rotationVariance);
        currentSpeed = Random.Range(averageSpeed - speedVariance, averageSpeed + speedVariance);
        direction = Random.value < 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        ColorUpdate();
        TryFall();

        if(doRotate)
            Rotate();
    }

    void Rotate()
    {
        transform.Rotate(new Vector3(0, 0, direction ? currentSpeed * Time.deltaTime : -currentSpeed * Time.deltaTime));
        if((transform.rotation.eulerAngles.z > currentRotation) || (transform.rotation.eulerAngles.z < 360 - currentRotation))
        {
            currentRotation = Random.Range(averageRotation - rotationVariance, averageRotation + rotationVariance);
            currentSpeed = Random.Range(averageSpeed - speedVariance, averageSpeed + speedVariance);
            direction = !direction;
        }
    }

    void TryFall()
    {
        if (pos.y == 0)
            return;
        if(Grid.me.GetCell(pos.x,pos.y-1) == null)
        {
            pos = new Vector2Int(pos.x, pos.y - 1);
        }
    }
    public void Unassign()
    {
        switch (type)
        {
            case Type.BLUE:
                Grid.me.blueClear++;
                Debug.Log("Blue");
                break;
            case Type.GREEN:
                Grid.me.greenClear++;
                Debug.Log("Green");
                break;
            case Type.RED:
                Grid.me.redClear++;
                Debug.Log("Red");
                break;
            case Type.YELLOW:
                Grid.me.yellowClear++;
                Debug.Log("Yellow");
                break;
        }
        ParticleManager.me.Explode(transform);
        Randomize();
        pos = new Vector2Int(pos.x, pos.y + 10  );
        transform.position = new Vector3((float)pos.x + Grid.me.offset.x, pos.y);
        CameraControl.me.cellsDestroyed++;


    }

    public void SetPos(Vector2Int p)
    {
        pos = p;
    }
    public void SetPos(int x, int y)
    {
        pos = new Vector2Int(x, y);
    }
    void ColorUpdate()
    {
        switch(type)
        {
            case Type.BLUE:
                rend.sprite = Grid.me.shield;
                break;
            case Type.GREEN:
                rend.sprite = Grid.me.cross;
                break;
            case Type.RED:
                rend.sprite = Grid.me.sword;
                break;
            case Type.YELLOW:
                rend.sprite = Grid.me.arrow;
                break;
            case Type.POINT:
                rend.color = new Color(0, 0, 0, 0);
                break;
        }
    }

    void Move()
    {
        goalWorldPos = new Vector3(pos.x * Grid.me.spacing + Grid.me.offset.x, pos.y * Grid.me.spacing + Grid.me.offset.y);
        frameGoal += transform.position - Vector3.Lerp(transform.position, goalWorldPos, lerpAmount);
        frameGoal *= lerpDamp;
        transform.position -= frameGoal;
        atDesiredPosition = transform.position == goalWorldPos;
        transform.localScale = new Vector3(1 + Mathf.Abs(frameGoal.x) * stretchMulti - Mathf.Abs(frameGoal.y) * stretchReductionMulti,
                                           1 + Mathf.Abs(frameGoal.y)* stretchMulti - Mathf.Abs(frameGoal.x) * stretchReductionMulti);
    }

    public void Randomize()
    {
        int sel = Random.Range(0, 4);
        type = (Type)sel;
    }
}
