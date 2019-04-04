using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid me;
    public List<Cell> cells;
    public Vector2Int dimensions;
    public Vector2 offset;
    public GameObject cellPrefab;

    private void Awake()
    {
        me = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
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
            }
        }
    }
}
