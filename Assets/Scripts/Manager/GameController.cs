using System;
using UnityEngine;

public enum GameMode
{
	Painting, Dunking, PreviewTransition, Preview
}

public class GameController : MonoBehaviour
{
	[SerializeField] private GameMode mode;

	public static event Action<GameMode> OnModeChanged;
	public static event Action OnDunk;

	public void ChangeMode(GameMode _mode)
	{
		mode = _mode;

		OnModeChanged?.Invoke(_mode);
	}

	public static void Dunk()
	{
		OnDunk?.Invoke();
	}

	public static void StartPaint()
	{
		OnModeChanged?.Invoke(GameMode.Painting);
	}

	public static void StartDunk()
	{
		OnModeChanged?.Invoke(GameMode.Dunking);
	}

	public static void StartPreviewTransition()
	{
		OnModeChanged?.Invoke(GameMode.PreviewTransition);
	}

	public static void StartPreview()
	{
		OnModeChanged?.Invoke(GameMode.Preview);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
