using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_water : MonoBehaviour,IUsableItem
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
        PlayerStatus playerStatus = target.GetComponent<PlayerStatus>();
        if (playerStatus != null)
        {
            playerStatus.CurrentThirst += 20f;
            Debug.Log($"渇きが{playerStatus.CurrentThirst}になりました。");
        }
    }

    public List<string> GetTargetTags()
    {
        return new List<string> { "Player"}; // このアイテムはPlayerとAllyタグのオブジェクトに使用可能
    }

}