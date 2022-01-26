using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRayRayyyyy : MonoBehaviour
{
    public LayerMask whatIsInteraction;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10, whatIsInteraction))
            {
                if (hit.collider.TryGetComponent<Lever>(out Lever lever))
                {
                    Debug.Log("asdfkasldfjadsf");
                    lever.Click();
                }
            }
        }
    }
}
