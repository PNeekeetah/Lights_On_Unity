using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
  private GameObject _gO;
  private SpriteRenderer _sR;
  private Transform _t;
  private bool _isLit;
  private Vector2Int _index;
  private Coroutine _glowEffect;
  private bool _coroutineMutex;
  private WaitForSeconds _cachedWait;
  private bool _waitMutex;
  private bool _hoveredOver;

  public GameObject GO
  {
    get { return _gO; }
  }
  public Vector2Int Index

  {
    get { return _index; }
    set { _index = value; }
  }
  public bool IsLit
  {
    get { return _isLit; }
  }

  private void SetSize(Vector2 size)
  {
    _t.localScale = size;
  }
  private void SetPosition(Vector2 position)
  {
    _t.position = position;
  }
  private void ToggleLit()
  {
    _isLit = !_isLit;
  }

  private void SetColor(Color color)
  {
    _sR.color = color;
  }

  public void HoveredOver(bool hovered) 
  {
    _hoveredOver = hovered;
  }

  public void Initialize(Vector2 position, Vector2 size, Vector2Int index)
  {
    _gO = Instantiate(LevelGenerator.tile, position, Quaternion.identity);
    _gO.name = "Tile " + index.x + index.y;
    _sR = _gO.GetComponent<SpriteRenderer>();
    _t = _gO.transform;
    _gO.SetActive(true);
    _isLit = false;
    SetPosition(position);
    SetSize(size);
    SetColor(Constants.COLOR_SILVER);
    Index = index;
    _coroutineMutex = true;
    _waitMutex = true;
    _cachedWait = new WaitForSeconds(0.1f);
    _hoveredOver = false;
  }

  IEnumerator Glow()
  {
    _coroutineMutex = false;
    Color first_color;
    Color second_color;
    if (_isLit)
    {
      first_color = Constants.COLOR_SILVER;
      second_color = Constants.COLOR_GOLD;
    }
    else
    {
      first_color = Constants.COLOR_GOLD;
      second_color = Constants.COLOR_SILVER;
    }

    while (true)
    {
      float time_increment = 0.0f;
      while (_sR.color != first_color)
      {
        time_increment += Time.deltaTime;
        _sR.color = Color.Lerp(_sR.color, first_color, Constants.COLOR_INTERPOLATION_SPEED * time_increment);
        yield return null;
      }
      time_increment = 0.0f;
      while (_sR.color != second_color)
      {
        time_increment += Time.deltaTime;
        _sR.color = Color.Lerp(_sR.color, second_color, Constants.COLOR_INTERPOLATION_SPEED * time_increment);
        yield return null;
      }
    }
  }

  void SetStateColor()
  {
    if (!_isLit)
    {
      _sR.color = Constants.COLOR_SILVER;
    }
    else
    {
      _sR.color = Constants.COLOR_GOLD;
    }
  }

  void Highlight()
  {
    if ((_hoveredOver) && (_coroutineMutex))
    {
      _glowEffect = StartCoroutine(Glow());
    }
    else if (!_hoveredOver)
    {
      if (_glowEffect != null)
      {
        StopCoroutine(_glowEffect);
        _coroutineMutex = true;
        SetStateColor();
      }
    }
  }

  IEnumerator ToggleLightWithBreak() 
  {
    _waitMutex = false;
    yield return _cachedWait;
    ToggleLit();
    yield return _cachedWait;
    _waitMutex = true;
    yield return null;
  }

  void ToggleTileLight() 
  {
    if ((_hoveredOver) && ( Input.GetMouseButton((int) Constants.MOUSE_STATES.LEFT_CLICK)) && _waitMutex)
    {
      StartCoroutine(ToggleLightWithBreak());
    }
  }

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    Highlight();
    ToggleTileLight();
  }
}
