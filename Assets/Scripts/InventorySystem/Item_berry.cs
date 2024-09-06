using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_berry : MonoBehaviour,IUsableItem
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
            playerStatus.CurrentHunger += 20f;
            Debug.Log($"空腹度が{playerStatus.CurrentHunger}になりました。");
        }
    }

    public List<string> GetTargetTags()
    {
        return new List<string> { "Player"}; // このアイテムはPlayerとAllyタグのオブジェクトに使用可能
    }

}
