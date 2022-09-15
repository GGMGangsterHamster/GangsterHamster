using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDeath : MonoBehaviour
{
    public void Active(GameObject other)
    {
        if(other.name == "ACube")
        {
            CubeSpawner cs = other.transform.parent.GetComponent<CubeSpawner>();

            if(!cs.GetIsRespawn())
            {
                other.transform.parent.GetComponent<CubeSpawner>()?.CubeDestory();
            }
        }
    }
}
