using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int pos;
    public Vector3 goalWorldPos;
    public Vector3 frameGoal;
    public enum Type { RED, ORANGE, BLUE, YELLOW, GREEN, PURPLE, POINT };
    public Type type;
    SpriteRenderer rend;
    public float lerpAmount = 0.05f;
    public static float high = 0.1f;
    public static float low = 0.05f;
    public static float lerpDamp = 0.8f;
    public bool atDesiredPosition;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        Move();
        transform.position = new Vector3(goalWorldPos.x, goalWorldPos.y + 20);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        ColorUpdate();
        TryFall();
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
        ParticleManager.me.Explode(transform);
        Randomize();
        pos = new Vector2Int(pos.x, pos.y + 10  );
        transform.position = new Vector3((float)pos.x + Grid.me.offset.x, pos.y);
        Grid.me.movesLeft = 6;
        Grid.me.score++;
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
                rend.color = Grid.me.blue;
                break;
            case Type.GREEN:
                rend.color = Grid.me.green;
                break;
            case Type.ORANGE:
                rend.color = Grid.me.orange;
                break;
            case Type.PURPLE:
                rend.color = Grid.me.purple;
                break;
            case Type.RED:
                rend.color = Grid.me.red;
                break;
            case Type.YELLOW:
                rend.color = Grid.me.yellow;
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
    }

    public void Randomize()
    {
        int sel = Random.Range(0, 6);
        type = (Type)sel;
    }
}
