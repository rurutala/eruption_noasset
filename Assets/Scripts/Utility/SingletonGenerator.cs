using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] singletonPrefabs;

    void Awake()
    {
        foreach (GameObject prefab in singletonPrefabs)
        {
            Instantiate(prefab);
        }
    }
}
