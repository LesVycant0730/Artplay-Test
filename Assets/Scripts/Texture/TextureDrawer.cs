using UnityEngine;

public class TextureDrawer : MonoBehaviour
{
    [Header ("Image")]
    [SerializeField] public MeshRenderer mesh;

    [SerializeField] private Texture2D textureDefault;
    private Texture2D textureNew;

    [SerializeField] private bool isDrawer = false;

    private bool isDefault = false;

    private bool isSet = false;
    private PixelCoor previousPixel;

    [ContextMenu ("Start")]
	private void Start()
	{
        mesh = GetComponent<MeshRenderer>();

		textureNew = new Texture2D(1024, 1024)
		{
			filterMode = FilterMode.Bilinear
		};

		textureNew.SetPixels32(textureDefault.GetPixels32());
        textureNew.Apply();

        mesh.material.SetTexture("_MainTex", textureNew);

        // Apply and override the texture
        if (mesh.material.HasProperty("_MainTex"))
        {
            mesh.material.mainTexture = textureNew;
        }

        if (isDrawer)
		{
            GameController.OnModeChanged += ResetDrawerTexture;
		}
    }

	private void OnDestroy()
	{
        GameController.OnModeChanged -= ResetDrawerTexture;
    }

    public void UpdateTexturePixel(Vector2 _texCoord)
	{
        _texCoord.x *= textureNew.width;
        _texCoord.y *= textureNew.height;

        BrushSetting.BrushType brush = BrushSetting.GetBrush();

        switch (brush)
        {
            case BrushSetting.BrushType.Circle:

                // Draw circle shape of pixels from a list
                BrushSetting.GetCirclePixels((int)_texCoord.x, (int)_texCoord.y).ForEach(x =>
                {
                    textureNew.SetPixel(x.X, x.Y, BrushSetting.GetPixelColor());
                });

                break;
            case BrushSetting.BrushType.Square:

                // Get 2D vector arrays based on brush size
                var array = BrushSetting.GetSquarePixels((int)_texCoord.x, (int)_texCoord.y);

                // Loop through two arrays and set pixels 
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        var vec = array[i, j];
                        textureNew.SetPixel(vec.X, vec.Y, BrushSetting.GetPixelColor());
                    }
                }

                break;
            case BrushSetting.BrushType.Mix:

                if (isSet)
				{
                    // Mix pixels
                    BrushSetting.GetPixelNearby(previousPixel.X, previousPixel.Y).ForEach(x =>
                    {
                        textureNew.SetPixel(x.X, x.Y, BrushSetting.GetPixelColor());
                    });
                }

                previousPixel = new PixelCoor((int)_texCoord.x, (int)_texCoord.y);
                isSet = true;
                break;
            default:
                Debug.LogError($"Not implemented brush type: {brush}");
                break;
        }

        // Apply updated texture
		textureNew.Apply();
    }

    // Used in Button only
    public void ResetTexture()
	{
        textureNew = new Texture2D(1024, 1024)
        {
            filterMode = FilterMode.Bilinear
        };

        textureNew.SetPixels32(textureDefault.GetPixels32());
        textureNew.Apply();

        mesh.material.mainTexture = textureNew;
    }

    private void ResetDrawerTexture(GameMode _mode)
	{
        if (_mode == GameMode.Painting)
		{
            ResetTexture();
		}
	}

    // Used in Button only
    public void SwapTexture()
	{
		isDefault = !isDefault;

		mesh.material.SetTexture("_MainTex", isDefault ? textureDefault : textureNew);
		mesh.material.mainTexture = isDefault ? textureDefault : textureNew;
	}

    public void SwapTexture(bool _isDefault)
	{
        mesh.material.SetTexture("_MainTex", _isDefault ? textureDefault : textureNew);
        mesh.material.mainTexture = _isDefault ? textureDefault : textureNew;
    }
}
