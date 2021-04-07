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
  public static GameObject lastSelectedObject = null;
  private Dictionary<GameObject,Tile> gOTileDictionary = new Dictionary<GameObject,Tile>();
  private List<List<Tile>> indexTileList = new List<List<Tile>>();
  private GameObject board;
  private Vector2Int[] steps;
  private bool _cross = false;
  private bool _playMode = false;
  private WaitForSeconds _cachedWait;
  
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

  void GameMode() 
  {
    if (_cross)
    {
      steps = Constants.CROSS_STEPS;
    }
    else 
    {
      steps = Constants.ORTHOGONAL_STEPS;
    }
  }

  // Initiates the board tiles
  void CreateTiles()
  {
    float sideX = 2 * levelXLim / levelXSide;
    float sideY = 2 * levelYLim / levelYSide;
    for (int x = 0; x < levelXSide; x++)
    {
      indexTileList.Add(new List<Tile>());
      for (int y = 0; y < levelYSide; y++)
      {
        float centre_x = ConvertRange(x, 0f, levelXSide, -levelXLim, levelXLim);
        float centre_y = ConvertRange(y, 0f, levelYSide, -levelYLim, levelYLim);
        Tile t = board.AddComponent<Tile>();
        t.Initialize(new Vector2(centre_x, centre_y), new Vector2(sideX, sideY), new Vector2Int(x, y));
        gOTileDictionary.Add(t.GO, t);
        indexTileList[x].Add(t);
      }
    }
  }

  //
  Vector2Int[] AdjacentTilesIndices(GameObject gO) 
  {
    if (gO == null)
    {
      return null;
    }
    Tile selectedTile = gOTileDictionary[gO];
    Vector2Int[] neighbourIndices = new Vector2Int[4];
    for (int s = 0; s < steps.Length; s ++ ) 
    {
      neighbourIndices[s] = selectedTile.Index + steps[s];
    }
    return neighbourIndices;
  }

  bool IsBounded(Vector2Int index) 
  {
    return (index.x >= 0 && index.x < levelXSide) && (index.y >= 0 && index.y < levelYSide);
  }
  List<Vector2Int> ValidAdjacentTileIndices(GameObject gO) 
  {
    if (gO == null)
    {
      return null;
    }
    Vector2Int[] adjacentTiles = AdjacentTilesIndices(gO);
    List<Vector2Int> validAdjacentTileIndices = new List<Vector2Int>();
    foreach (Vector2Int neighbourIndex in adjacentTiles) 
    {
      if (IsBounded(neighbourIndex))
        validAdjacentTileIndices.Add(neighbourIndex);
    }
    return validAdjacentTileIndices;
  }

  List<Tile> GetAdjacentTiles(GameObject gO) 
  {
    if (gO == null)
    {
      return null;
    }
    List<Tile> tilesList = new List<Tile>();
    List<Vector2Int> validAdjacentTileIndices = ValidAdjacentTileIndices(gO);
    foreach (Vector2Int index in validAdjacentTileIndices) 
    {
      tilesList.Add(indexTileList[index.x][index.y]);
    }
    return tilesList;
  }
  void OnHover(GameObject gO) 
  {
    if (gO == null)
    {
      return;
    }
    Tile selectedTile = gOTileDictionary[selectedObject];
    if (_playMode) { 
      List<Tile> surroundingTiles = GetAdjacentTiles(gO);
      foreach (Tile t in surroundingTiles) 
      {
        t.HoveredOver(true);
      }
    }
    selectedTile.HoveredOver(true);
  }

  void OnUnhover(GameObject gO) 
  {
    if (gO == null) 
    {
      return;
    }
    Tile lastSelectedTile = gOTileDictionary[gO];
    if (_playMode)
    {
      List<Tile> surroundingTiles = GetAdjacentTiles(gO);
      foreach (Tile t in surroundingTiles)
      {
        t.HoveredOver(false);
      }
    }
    lastSelectedTile.HoveredOver(false);
  }

  // Start is called before the first frame update
  void Start()
  {
    board = new GameObject("Board");
    tile = tileProxy;
    CreateTiles();
    GameMode();
    lastSelectedObject = indexTileList[0][0].GO;
  }

  /** Something tells me this should be part of a command manager.
  void TogglePlayMode() 
  {
    _playMode = !_playMode;
  }
  IEnumerator TogglePlayModeWithBreak()
  {
    _waitMutex = false;
    yield return _cachedWait;
    TogglePlayMode();
    yield return _cachedWait;
    _waitMutex = true;
    yield return null;
  }
  */
  // Update is called once per frame
  void Update()
  {
    selectedObjectProxy = selectedObject;
    GetOnMouseOver();
    OnHover(selectedObject);
    if (lastSelectedObject != selectedObject) 
    {
      OnUnhover(lastSelectedObject);
      lastSelectedObject = selectedObject; 
    }
    if (Input.GetKeyDown(KeyCode.P)) 
    {
      _playMode = !_playMode;
      GameMode();
    }
    if (Input.GetKeyDown(KeyCode.X)) 
    {
      _cross = !_cross;
    }
    if (Input.GetKeyDown(KeyCode.Escape)) 
    {
      Application.Quit();
    }


  }
}