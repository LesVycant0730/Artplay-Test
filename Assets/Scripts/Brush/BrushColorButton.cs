using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BrushColorButton : Selectable
{
	[SerializeField] private Color brushColor;

	[SerializeField] private bool isDefault = false;

	protected override void Start()
	{
		BrushSetting.OnColorSelected += Toggle;

		if (isDefault)
		{
			BrushSetting.SetColor(brushColor);
		}
	}

	protected override void OnDestroy()
	{
		BrushSetting.OnColorSelected -= Toggle;
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);

		BrushSetting.SetColor(brushColor);
	}

	private void Toggle(Color _color)
	{
		interactable = brushColor != _color;
	}
}
