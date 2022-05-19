using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_BagCatcher : MonoBehaviour
{
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bag"))
        {
            other.transform.position = new Vector3(player.transform.position.x - 2f, player.transform.position.y, player.transform.position.z);
        }
    }
}
