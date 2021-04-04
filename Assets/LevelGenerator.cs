using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
  
  public static GameObject tile;
  public GameObject tileProxy;
  public GameObject selectedObjectProxy;
  public int levelXSide = 10;
  public int levelYSide = 10;
  private float levelXLim = 10;
  private float levelYLim = 10;
  public static GameObject selectedObject = null;
  private List<List<Tile>> tiles = new List<List<Tile>>();
  private GameObject board;
  
  float ConvertRange(float value, float minOld, float maxOld, float minNew, float maxNew)
  {
    return value * (maxNew - minNew) / (maxOld - minOld - 1) + minNew;
  }
  void GetOnMouseOver()
  {
    Vector3 cursorPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20);
    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(cursorPosition);
    RaycastHit2D hitData = Physics2D.Raycast(new Vector2(worldPosition.x, worldPosition.y), Vector2.zero, 0);
    if (hitData)
    {
      selectedObject = hitData.transform.gameObject;
    }
  }
  // Initiates the board tiles
  void CreateTiles()
  {
    float sideX = 2 * levelXLim / levelXSide;
    float sideY = 2 * levelYLim / levelYSide;
    for (int x = 0; x < levelXSide; x++)
    {
      tiles.Add(new List<Tile>());
      for (int y = 0; y < levelYSide; y++)
      {
        float centre_x = ConvertRange(x, 0f, levelXSide, -levelXLim, levelXLim);
        float centre_y = ConvertRange(y, 0f, levelYSide, -levelYLim, levelYLim);
        Tile t = board.AddComponent<Tile>();
        t.Initialize(new Vector2(centre_x, centre_y), new Vector2(sideX, sideY), new Vector2(x, y));
        tiles[x].Add(t);
      }
    }
  }
  // Start is called before the first frame update
  void Start()
  {
    board = new GameObject("Board");
    tile = tileProxy;
    CreateTiles();
  }

  // Update is called once per frame
  void Update()
  {
    GetOnMouseOver();
    selectedObjectProxy = selectedObject;
  }
}