using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_down : MonoBehaviour
{
    public GameObject nomal;
    public GameObject down;
    public GameObject targetObject;
    private PlayerStatus playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = targetObject.GetComponent<PlayerStatus>();
        down.SetActive(false);
        nomal.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStatus.CurrentHP <= 0 || playerStatus.CurrentHunger <= 0 || playerStatus.CurrentThirst <= 0)
        {
            down.SetActive(true);
            nomal.SetActive(false);
        }
    }
}
