using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
  public static Color COLOR_SILVER = new Color(192f, 192f, 192f, 255f) / 255f;
  public static Color COLOR_GOLD = new Color(255f, 215f, 0f, 255f) / 255f;
  public enum MOUSE_STATES { LEFT_CLICK, MIDDLE_CLICK, RIGHT_CLICK };
  public static float COLOR_INTERPOLATION_SPEED = 0.4F;
  public static Vector2Int[] CROSS_STEPS = { new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1) };
  public static Vector2Int[] ORTHOGONAL_STEPS = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };
}
