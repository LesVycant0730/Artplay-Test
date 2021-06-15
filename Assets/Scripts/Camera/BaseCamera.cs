using UnityEngine;

public class BaseCamera : MonoBehaviour
{
    protected Camera cam;
    protected Ray ray;

    private Animator anim;

    [SerializeField] protected GameMode cameraMode;

    [SerializeField, Range(0.01f, 1.0f)] protected float updateDelay = 0.05f;
    [SerializeField] protected float currentUpdate = 0.0f;

    protected virtual void Awake()
	{
        cam = Camera.main;
        anim = GetComponent<Animator>();

        GameController.OnModeChanged += OnModeChanged;
    }

    protected virtual void OnDestroy()
	{
        GameController.OnModeChanged -= OnModeChanged;
    }

    private void OnModeChanged(GameMode _mode)
    {
        enabled = _mode == cameraMode;

        anim.SetBool("isDunking", _mode == GameMode.Dunking);

        if (_mode == GameMode.PreviewTransition)
            anim.SetBool("isPreviewing", true);
        else if (_mode == GameMode.Painting)
            anim.SetBool("isPreviewing", false);
    }

    public void StartPreview()
	{
        GameController.StartPreview();
	}

    protected virtual bool CanInput()
	{
        // Control update time to improve performance
        // Raycast is expensive
        if (currentUpdate < updateDelay)
        {
            currentUpdate += Time.deltaTime;
            return false;
        }

        // PC mouse input
        if (Input.GetMouseButton(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
        }
        // Touch input
        else if (Input.touchCount > 0)
        {
            ray = cam.ScreenPointToRay(Input.GetTouch(0).position);
        }
        else
        {
            return false;
        }

        // Reset update and allow input registration to raycast
        currentUpdate = 0.0f;
        return true;
    }
}
