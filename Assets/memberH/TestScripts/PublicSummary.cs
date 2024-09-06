using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicSummary : MonoBehaviour
{
    [SerializeField] VolcanoEventManager volcanoEvent;

    // Start is called before the first frame update
    void Start()
    {

    }

    public async UniTask StartEvent()
    {
        await volcanoEvent.StartVolcanoEvent();
    }
    
}
