using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentative_berry : MonoBehaviour,IUsableItem
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
            playerStatus.CurrentHP += 20f;
            Debug.Log("Health potion used. HP restored.");
        }
    }

    public List<string> GetTargetTags()
    {
        return new List<string> { "Player", "Ally" }; // このアイテムはPlayerとAllyタグのオブジェクトに使用可能
    }
    public bool HasQuantity()
    {
        return true; // このアイテムは個数を持つ
    }

}
