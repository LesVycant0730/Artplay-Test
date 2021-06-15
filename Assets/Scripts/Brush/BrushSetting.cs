using System;
using System.Collections.Generic;
using UnityEngine;

public class BrushSetting : MonoBehaviour
{
    public enum BrushType
	{
        Circle = 0, Square = 1, Mix = 2
	}

    private static BrushSetting instance;

    [Header("Brush Setting")]
    [SerializeField] private BrushType brushType = BrushType.Circle;

    [Header ("Brush Setting")]
    [SerializeField] private int brushRadius = 2;

    [Header("Brush Color")]
    [SerializeField] private Color colorInner;
    [SerializeField] private Color colorOuter;
    [SerializeField] private AnimationCurve colorCurve;

    public static Action<Color> OnColorSelected;
    public static Action<BrushType> OnBrushSelected;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public static void SetBrushSize(int _size)
	{
        if (instance)
		{
            instance.brushRadius = _size;
		}
	}

    /// <summary>
    /// Get pixel coordinates of square shape.
    /// </summary>
    /// <param name="x">Coordinate x</param>
    /// <param name="y">Coordinate y</param>
    /// <returns></returns>
    public static PixelCoor[,] GetSquarePixels(int x, int y)
	{
        if (instance)
        {
            PixelCoor[,] pixels = new PixelCoor[instance.brushRadius, instance.brushRadius];

            // Get the center point of the brush
            int radius = instance.brushRadius % 2 == 0 ? instance.brushRadius / 2 : (instance.brushRadius - 1) / 2;

            for (int i = -radius; i < radius; i++)
			{
                for (int j = -radius; j < radius; j++)
				{
                    pixels[i + radius, j + radius] = new PixelCoor(x + i, y + j);
				}
			}

            return pixels;
        }

        return new PixelCoor[0, 0];
	}

    /// <summary>
    /// Get pixel coordinates of circle shape.
    /// </summary>
    /// <param name="x">Coordinate x</param>
    /// <param name="y">Coordinate y</param>
    /// <returns></returns>
    public static List<PixelCoor> GetCirclePixels(int x, int y)
	{
        if (instance)
		{
            List<PixelCoor> pixelList = new List<PixelCoor>();

            int radius = instance.brushRadius;

            for (int i = x - radius; i < x + radius; i++)
			{
                for (int j = y - radius; j < y + radius; j++)
				{
                    bool isInCircle = (Mathf.Pow(i - x, 2) + Mathf.Pow(j - y, 2)) <= Mathf.Pow(radius, 2);

                    if (isInCircle)
				    {
                        pixelList.Add(new PixelCoor(i, j));
                    }
                }
			}

            return pixelList;
        }

        return null;
	}

    public static List<PixelCoor> GetPixelNearby(int prevX, int prevY)
	{
        if (instance)
        {
            List<PixelCoor> pixelList = new List<PixelCoor>();

            int radius = instance.brushRadius;

            for (int i = 0; i < radius; i++)
			{
                pixelList.Add(new PixelCoor(prevX + i, prevY));
                pixelList.Add(new PixelCoor(prevX - i, prevY));
                pixelList.Add(new PixelCoor(prevX, prevY + i));
                pixelList.Add(new PixelCoor(prevX, prevY - i));
            }

            return pixelList;
        }

        return null;
	}

    public static Color GetPixelColor()
	{
        if (instance)
		{
            //return Color.Lerp(instance.colorInner, instance.colorOuter, 0.1f);
            return instance.colorInner;
		}

        return Color.white;
	}

    public static float GetPixelDistance(float current, float max, float midpoint)
	{
        float value = (float)(current - midpoint) / max;

        if (value < 0)
		{
            value *= -1;
		}

        return value;
	}

    public static void SetColor(Color color)
	{
        if (instance)
		{
            instance.colorInner = color;

            OnColorSelected?.Invoke(color);
		}
	}

    public static BrushType GetBrush()
	{
        return instance != null ? instance.brushType : BrushType.Circle;
	}

    public static void SwapBrush()
	{
        if (instance)
		{
            instance.brushType = instance.brushType == BrushType.Circle ? BrushType.Square : BrushType.Circle;

            OnBrushSelected?.Invoke(instance.brushType);
		}
	}
}
