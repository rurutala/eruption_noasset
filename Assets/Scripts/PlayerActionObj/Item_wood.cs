using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_wood : MonoBehaviour, IUsableItem
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UseItem(GameObject target)
    {

    }

    public List<string> GetTargetTags()
    {
        return new List<string> { "Player" }; // このアイテムはPlayerとAllyタグのオブジェクトに使用可能
    }

}