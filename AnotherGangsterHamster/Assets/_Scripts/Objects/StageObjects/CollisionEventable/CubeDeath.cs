using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDeath : MonoBehaviour
{
    public void Active(GameObject other)
    {
        Debug.Log("무언가 충돌함");
        if(other.name == "ACube")
        {
            CubeSpawner cs = other.transform.parent.GetComponent<CubeSpawner>();

            Debug.Log($"무언가 충돌함 {cs.CanRespawn()}");
            if (cs.CanRespawn())
            {
                other.transform.parent.GetComponent<CubeSpawner>()?.CubeDestory();
            }
        }
    }
}
