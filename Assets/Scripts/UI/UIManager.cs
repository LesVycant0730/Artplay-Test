using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] uiObjects;

	[SerializeField] private GameObject previewUI;
	[SerializeField] private Text textBrushSize;

	private void Awake()
	{
		GameController.OnModeChanged += ToggleUI;
	}

	private void OnDestroy()
	{
		GameController.OnModeChanged -= ToggleUI;
	}

	public void ToggleUI(GameMode _mode)
	{
		Array.ForEach(uiObjects, x =>
		{
			if (x != null)
			{
				x.SetActive(_mode == GameMode.Painting);
			}
		});

		previewUI.SetActive(_mode == GameMode.Preview);
	}

	public void OnSliderValueChanged(float _value)
	{
		textBrushSize.text = $"Brush Size: {_value}";
		BrushSetting.SetBrushSize((int)_value);
	}
}
