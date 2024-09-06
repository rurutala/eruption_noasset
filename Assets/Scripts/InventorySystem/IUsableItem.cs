using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IUsableItem
{
    void UseItem(GameObject target); // ターゲットにアイテムを使用する
    List<string> GetTargetTags();    // 使用対象となるオブジェクトのタグのリストを取得する

}
