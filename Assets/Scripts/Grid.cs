using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Grid : MonoBehaviour
{
    [Header("SCORE AND STUFF")]
    public int movesLeft;
    public int score;

    public static Grid me;
    public List<Cell> cells;
    public Vector2Int dimensions;
    public Vector2 offset;
    public float spacing;
    public GameObject cellPrefab;
    public TextMeshPro movesLeftText;
    public SpriteRenderer pointSprite;
    public bool checkFlag;

    [Header("COLORS")]
    public Color red;
    public Color blue;
    public Color purple;
    public Color green;
    public Color yellow;
    public Color orange;
    public Gradient pointColor;

    [Header("RPG STATS")]
    public int playerHealth;
    public int enemyHealth;
    public int playerDamage;
    public int redClear, blueClear, yellowClear, greenClear;

    [Header("RPG START STATS")]
    public int playerStartHealth;
    public int enemyStartHealth;
    public int playerStartDamage;
    public int enemyDamage;
    public int healPerCube;

    private Vector2Int pointerPlace = new Vector2Int(2, 2);

    private void Awake()
    {
        me = this;
        checkFlag = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitGrid();
        StartMatchCheck();
        PlacePointer(pointerPlace.x, pointerPlace.y);
        RPGSetup();
    }

    // Update is called once per frame
    void Update()
    {
        InputStuff();
        if(checkFlag)
            DestroyMatches();

        movesLeftText.text = "" + movesLeft;
        movesLeftText.color = pointColor.Evaluate(movesLeft/6f);
        pointSprite.color = pointColor.Evaluate(movesLeft / 6f);
        CubeChecks();
        MoveCheck();
    }


    void MoveCheck()
    {
        if(movesLeft <= 0)
        {
            movesLeft = 6;
            playerHealth -= enemyDamage;
        }
        if (playerHealth > playerStartHealth)
            playerHealth = playerStartHealth;
    }
    void CubeChecks()
    {
        if(yellowClear > 0)
        {
            yellowClear = 0;
            playerDamage++;
        }
        if(redClear > 0)
        {
            enemyHealth -= redClear * playerDamage;
            redClear = 0;
        }
        if(greenClear >0)
        {
            playerHealth += greenClear * healPerCube;
            greenClear = 0;
        }
    }
    void RPGSetup()
    {
        playerHealth = playerStartHealth;
        enemyHealth = enemyStartHealth;
        playerDamage = playerStartDamage;
        redClear = blueClear = yellowClear = greenClear = 0;
    }

    void InputStuff()
    {
        Cell point = FindPointer();
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(InGrid(point.pos.x, point.pos.y+1))
            {
                GetCell(point.pos.x, point.pos.y + 1).pos = point.pos;
                point.SetPos(point.pos.x, point.pos.y + 1);
                movesLeft--;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (InGrid(point.pos.x, point.pos.y - 1))
            {
                GetCell(point.pos.x, point.pos.y - 1).pos = point.pos;
                point.SetPos(point.pos.x, point.pos.y - 1);
                movesLeft--;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (InGrid(point.pos.x + 1, point.pos.y))
            {
                GetCell(point.pos.x + 1, point.pos.y).pos = point.pos;
                point.SetPos(point.pos.x + 1, point.pos.y);
                movesLeft--;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (InGrid(point.pos.x - 1, point.pos.y))
            {
                GetCell(point.pos.x - 1, point.pos.y).pos = point.pos;
                point.SetPos(point.pos.x - 1, point.pos.y);
                movesLeft--;
            }
        }
    }
    public Cell FindPointer()
    {
        foreach(Cell c in cells)
        {
            if (c.type == Cell.Type.POINT)
                return c;
        }
        return null;
    }
    void PlacePointer(int x,int y)
    {
        GetCell(x, y).type = Cell.Type.POINT;
    }
    void StartMatchCheck()
    {
        if (FindAllMatches().Length == 0)
            return;
        Match[] matches = FindAllMatches();
        foreach(Match m in matches)
        {
            GetCell(m.pos.x, m.pos.y).Randomize();
        }
        StartMatchCheck();
    }
    void DestroyMatches()
    {
        if (FindAllMatches().Length == 0)
            return;
        Match[] matches = FindAllMatches();
        foreach(Match m in matches)
        {
            if(m.direction == Match.Direction.HOR)
            {
                if(GetCell(m.pos.x, m.pos.y) != null)
                    GetCell(m.pos.x, m.pos.y).Unassign();
                if(GetCell(m.pos.x + 1, m.pos.y) != null)
                    GetCell(m.pos.x+1, m.pos.y).Unassign();
                if (GetCell(m.pos.x + 2, m.pos.y) != null)
                    GetCell(m.pos.x+2, m.pos.y).Unassign();
                continue;
            }
            if (m.direction == Match.Direction.VER)
            {
                if (GetCell(m.pos.x, m.pos.y) != null)
                    GetCell(m.pos.x, m.pos.y).Unassign();
                if (GetCell(m.pos.x, m.pos.y + 1) != null)
                    GetCell(m.pos.x, m.pos.y+1).Unassign();
                if (GetCell(m.pos.x, m.pos.y+2) != null)
                    GetCell(m.pos.x, m.pos.y+2).Unassign();
                continue;
            }
        }
    }
    Match[] FindAllMatches()
    {
        List<Match> matches = new List<Match>();
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                if (GetCell(x, y) == null)
                    continue;
                // Horizontal matches
                if(InGrid(x + 1, y) && InGrid(x+2, y))
                {
                    if (GetCell(x + 1, y) == null || GetCell(x + 2, y) == null)
                        continue;
                    if(GetCell(x,y).type == GetCell(x+1,y).type && GetCell(x,y).type == GetCell(x+2,y).type)
                    {
                        matches.Add(new Match(x, y, Match.Direction.HOR));
                    }
                }
                // Vertical matches
                if (InGrid(x , y + 1) && InGrid(x, y + 2))
                {
                    if (GetCell(x, y + 1) == null || GetCell(x, y + 2) == null)
                        continue;
                    if (GetCell(x, y).type == GetCell(x, y + 2).type && GetCell(x, y).type == GetCell(x, y + 1).type)
                    {
                        matches.Add(new Match(x, y, Match.Direction.VER));
                    }
                }
            }
        }
        return matches.ToArray();
    }

    bool InGrid(int x, int y)
    {
        return x >= 0 && x < dimensions.x && y >= 0 && y < dimensions.y;
    }
    public Cell GetCell(int x, int y)
    {
        foreach(Cell c in cells)
        {
            if (c.pos.x == x && c.pos.y == y)
                return c;
        }
        return null;
    }

    void InitGrid()
    {
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                GameObject obj = Instantiate(cellPrefab);
                cells.Add(obj.GetComponent<Cell>());
                obj.GetComponent<Cell>().pos = new Vector2Int(x, y);
                obj.GetComponent<Cell>().Randomize();
            }
        }
    }
}

public class Match
{
    public Vector2Int pos;
    public enum Direction { HOR, VER };
    public Direction direction;
    public Match(int x, int y, Direction d)
    {
        pos = new Vector2Int(x, y);
        direction = d;
    }
}