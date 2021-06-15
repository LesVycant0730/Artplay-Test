using UnityEngine;

public class CameraDrag : BaseCamera
{
    [SerializeField] private Transform swipeModel;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.GetTouch(0).position);

            swipeModel.position = new Vector3(pos.x, pos.y, swipeModel.position.z);
        }
    }
}
