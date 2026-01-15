using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    [SerializeField] Transform startingNode;
    [SerializeField] GameObject linePrefab;
    //private Transform canvasParent;
    private MapGenerator instance;

    [Header("Perlin Noise numbers")]
    [SerializeField] int width = 256;
    [SerializeField] int height = 256;
    float scale = 20f;
    [Header("offsets")]
    [SerializeField] int xOffSet;
    [SerializeField] int yOffSet;


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

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }

    private void OnEnable()
    {
        for(int lcv=0;lcv<365;lcv+=15)
        {
            var quat = Quaternion.Euler(0, 0, lcv);
            var line =Instantiate(linePrefab, startingNode.transform.position, quat, startingNode);
            line.GetComponent<RectTransform>().SetAsFirstSibling();
        }
        //I expect this to make many spokes
    }

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