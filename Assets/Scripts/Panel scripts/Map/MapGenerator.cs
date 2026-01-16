using UnityEngine;
using UnityEngine.UI;


public class MapGenerator : MonoBehaviour
{
    [SerializeField] Transform startingNode;
    [SerializeField] GameObject linePrefab;
    [SerializeField] RawImage im;
    //private Transform canvasParent;
    public static MapGenerator instance;

    [Header("Perlin Noise numbers")]
    [SerializeField] int width = 256;
    [SerializeField] int height = 256;
    [SerializeField] float scale = 20f;
    [Header("offsets")]
    [SerializeField] int xOffSet;
    [SerializeField] int yOffSet;//these 2 numbers are basically the seed, could use for save?


    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Debug.LogError("we got 2 Map Generators in the scene");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        RandomizeOffsets();

        //Renderer renderer = GetComponent<Renderer>();
        im.texture = GenerateTexture();
    }

    private void OnEnable()
    {
        float x = startingNode.position.x;
        float y = startingNode.position.y;

        for (int lcv=0;lcv<365;lcv+=15)
        {
            var quat = Quaternion.Euler(0, 0, lcv);

            float xCalc = x * Mathf.Cos(lcv);
            float yCalc = y * Mathf.Sin(lcv);

            Vector2 nodePos = new Vector3(xCalc, yCalc);
            //var nodePos = line.GetComponent<MapLine>().getNodePos();
            if (GetValue(nodePos)>0.55f)
            {
                var line = Instantiate(linePrefab, startingNode.transform.position, quat, startingNode);
                line.GetComponent<RectTransform>().SetAsFirstSibling();
                line.GetComponent<MapLine>().SetCount(3);
            }
        }
        //I expect this to make many spokes
    }

    public GameObject GetPrefab() => linePrefab;

#region Visualize Perlin
    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for(int x =0;x<width;x++)
        {
            for(int y=0;y<height;y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scale + xOffSet;
        float yCoord = (float)y / height * scale + yOffSet;
        
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }
#endregion

    public float GetValue(Vector2 pos)
    {
        float xCoord = (float)pos.x / width * scale + xOffSet;
        float yCoord = (float)pos.y / height * scale + yOffSet;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        //Debug.Log(sample + "");
        return sample;
    }

    void RandomizeOffsets()
    {
        xOffSet = Random.Range(0, 9999999);
        yOffSet = Random.Range(0, 9999999);
    }


}
/*Refrences
 * perlin noise Brakeys https://www.youtube.com/watch?v=bG0uEXV6aHQ
 * 
 * 
 */