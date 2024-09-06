using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class DestroyDontDestroyOnLoadObjects : MonoBehaviour
{
    private void Start()
    {
        DestroyAllDontDestroyOnLoadObjects();
    }

    private void DestroyAllDontDestroyOnLoadObjects()
    {
        // DontDestroyOnLoadオブジェクトが含まれる特別なシーンを取得
        GameObject[] allObjects = FindObjectsOfType<GameObject>();  // すべてのGameObjectを取得

        // DontDestroyOnLoadオブジェクトは通常シーン内には存在しないため、特別なシーンから取り出す
        GameObject dontDestroyOnLoadParent = null;

        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.name == null || obj.scene.name == "DontDestroyOnLoad")
            {
                // 親オブジェクトも含めてAudioManagerを持っているかをチェック
                if (HasAudioManager(obj))
                {
                    continue; // AudioManagerがある場合は削除しない
                }
                // DontDestroyOnLoadオブジェクトを削除
                Destroy(obj);
                Debug.Log($"Destroyed: {obj.name}");
            }
        }
    }

    // オブジェクトまたはその親オブジェクトがAudioManagerを持っているかを確認
    bool HasAudioManager(GameObject obj)
    {
        // 自身がAudioManagerを持っているかを確認
        if (obj.GetComponent<AudioManager>() != null)
        {
            return true;
        }

        // 親オブジェクトが存在する場合、親もチェック
        Transform parent = obj.transform.parent;
        while (parent != null)
        {
            if (parent.GetComponent<AudioManager>() != null)
            {
                return true;
            }
            parent = parent.parent; // さらに上の親を確認
        }

        return false;
    }
}
