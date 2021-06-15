using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BrushButton : Selectable
{
    [SerializeField] private Sprite[] brushSprites;

    [SerializeField] private Image icon;

	protected override void Start()
	{
		BrushSetting.OnBrushSelected += SwapBrush;
	}

	protected override void OnDestroy()
	{
		BrushSetting.OnBrushSelected -= SwapBrush;
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);

		BrushSetting.SwapBrush();
	}

	private void SwapBrush(BrushSetting.BrushType _brushType)
	{
		icon.sprite = brushSprites[(int)_brushType];
	}
}
