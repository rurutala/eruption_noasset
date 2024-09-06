using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Vector3 diff;
    [SerializeField] GameObject target;
    [SerializeField] float followSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // イベント中は追従しない
        if (!StaticValues.Instance.canPlayerMove) return;

        this.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
    }
}
