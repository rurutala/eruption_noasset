using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    const int ISLAND_SIZE = 64;

    [SerializeField] GameObject treePrefab;
    [SerializeField] GameObject berrytreePrefab;
    [SerializeField] GameObject rockPrefab;
    [SerializeField] Transform fieldObjParent;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] prefabs = { treePrefab, berrytreePrefab, rockPrefab };
        GenerateFieldObj(prefabs);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateFieldObj(GameObject[] prefabs)
    {
        for (int x = 0; x < ISLAND_SIZE; x++)
        {
            for (int y = 0; y < ISLAND_SIZE; y++)
            {
                var pos = new Vector2(x, y);
                if (x > 0 && x < ISLAND_SIZE &&  y > 0 && y < ISLAND_SIZE)
                {
                    if (Random.Range(0f, 1f) < 0.02f)
                    {
                        Instantiate(prefabs[Random.Range(0,prefabs.Length)], pos, Quaternion.identity, fieldObjParent);
                    }
                }
            }
        }
    }
}
