using UnityEngine;

public class CameraPainting : BaseCamera
{
    [SerializeField] private TextureDrawer drawer;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanInput())
		{
            return;
		}

        // Cheat a bit, hehe
        var cast = Physics.RaycastAll(ray, 3.0f);

        if (cast.Length > 0)
		{
            for (int i = 0; i < cast.Length; i++)
            {     
                TextureDrawer hitDrawer = cast[i].transform.GetComponent<TextureDrawer>();

                if (drawer == null || drawer != hitDrawer)
                {
                    SetDrawer(hitDrawer);
                }

                drawer.UpdateTexturePixel(cast[i].textureCoord);
            }
        }
	}

    public void CameraReadyToDunk()
	{
        GameController.Dunk();
	}

	public void SetDrawer(TextureDrawer _drawer)
	{
        drawer = _drawer;
	}
}
