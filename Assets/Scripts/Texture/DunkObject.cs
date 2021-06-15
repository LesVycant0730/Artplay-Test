using UnityEngine;
using System;

public class DunkObject : MonoBehaviour
{
	[SerializeField] private TextureDrawer[] drawers = new TextureDrawer[0];

	private Animator anim;
	private Vector3 initPos;
	private Quaternion initRot;

	[SerializeField] private Vector3 dunkPos = Vector3.zero;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		initPos = transform.position;

		GameController.OnModeChanged += OnModeChanged;
		GameController.OnDunk += Dunk;
	}

	private void OnDestroy()
	{
		GameController.OnModeChanged -= OnModeChanged;
		GameController.OnDunk -= Dunk;
	}

	private void Update()
	{
		if (Input.touchCount > 0)
		{
			var touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Moved)
			{
				transform.Rotate(-touch.deltaPosition.x * Vector3.up, Space.World);
			}
		}
	}

	public void ToDefault()
	{
		transform.SetPositionAndRotation(initPos, initRot);
		ToggleAnimator(1);
		anim.Play("Default");

		UpdateDrawers(x =>
		{
			x.ResetTexture();
			x.gameObject.SetActive(true);
		});
	}

	private void OnModeChanged(GameMode _mode)
	{
		switch (_mode)
		{
			case GameMode.Painting:
				ToDefault();
				break;
			case GameMode.Dunking:
				UpdateDrawers(x => x.gameObject.SetActive(true));
				transform.position = dunkPos;
				break;
			case GameMode.PreviewTransition:
				break;
		}
	}

	private void Dunk()
	{
		UpdateTextures(0);

		anim.Play("Dunk");
	}

	public void ToggleAnimator(int _enabled)
	{
		anim.enabled = _enabled == 1;
	}

	public void StartPreviewTransition()
	{
		GameController.StartPreviewTransition();
	}

    public void UpdateTextures(int _defaultParam)
	{
		bool isDefault = _defaultParam == 0;

		UpdateDrawers(x => x.SwapTexture(isDefault));
	}

	private void UpdateDrawers(Action<TextureDrawer> _action)
	{
		Array.ForEach(drawers, _action);
	}
}
