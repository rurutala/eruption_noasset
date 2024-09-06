using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    const int SEA_SIZE = 8;
    const int ISLAND_SIZE = 64;

    [SerializeField] GameObject glassPrefab;
    [SerializeField] GameObject seaPrefab;
    [SerializeField] GameObject beachPrefab;
    [SerializeField] GameObject volcanoPrefab;
    [SerializeField] Transform tileParent;

    [SerializeField] Sprite[] glassImages;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateMap()
    {
        for (int x = -SEA_SIZE; x < ISLAND_SIZE+SEA_SIZE; x++)
        {
            for (int y = -SEA_SIZE; y < ISLAND_SIZE+SEA_SIZE; y++)
            {
                var pos = new Vector2(x, y);
                if (x < 0 || x >= ISLAND_SIZE || y < 0 || y >= ISLAND_SIZE) GenerateSea(pos);
                else if (x == 0 || x == ISLAND_SIZE - 1 || y == 0 || y == ISLAND_SIZE - 1) GenerateBeach(pos);
                else GenerateGlass(pos);
            }
        }
        Instantiate(volcanoPrefab, new Vector3(ISLAND_SIZE / 2, ISLAND_SIZE / 2, 0), Quaternion.identity, tileParent); // 火山を生成
    }

    void GenerateGlass(Vector2 pos)
    {
        var tile = Instantiate(glassPrefab, pos, Quaternion.identity, tileParent);
        if (Random.Range(0f, 1f) < 0.1f)
        {
            tile.GetComponent<SpriteRenderer>().sprite = glassImages[Random.Range(1, glassImages.Length)];
        }
    }
    
    void GenerateSea(Vector2 pos)
    {
        Instantiate(seaPrefab, pos, Quaternion.identity, tileParent);
    }

    void GenerateBeach(Vector2 pos)
    {
        Instantiate(beachPrefab, pos, Quaternion.identity, tileParent);
    }
}
