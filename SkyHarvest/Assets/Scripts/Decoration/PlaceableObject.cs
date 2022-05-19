using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    private int numberOfBlockers = 0;

    private void OnCollisionEnter(Collision collision)
    {
        numberOfBlockers++;
        if (numberOfBlockers == 1)
        {
            //visual to indicate it can't be placed
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        numberOfBlockers--;
        if (numberOfBlockers == 0)
        {
            //visual to indicate it can be placed
        }
    }

    public bool IsPlaceable()
    {
        return numberOfBlockers == 0;
    }
}
