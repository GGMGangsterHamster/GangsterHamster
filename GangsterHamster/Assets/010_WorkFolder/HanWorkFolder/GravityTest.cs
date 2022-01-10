using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTest : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) {
            Gravity.Object.Management.GravityManager.Instance.ChangeGlobalGravityDirection(Vector3.up);
        }
        if(Input.GetKeyDown(KeyCode.Semicolon)) {
            Gravity.Object.Management.GravityManager.Instance.ChangeGlobalGravityDirection(Vector3.down);
        }
        if(Input.GetKeyDown(KeyCode.L)) {
            Gravity.Object.Management.GravityManager.Instance.ChangeGlobalGravityDirection(Vector3.left);
        }
        if(Input.GetKeyDown(KeyCode.Quote)) {
            Gravity.Object.Management.GravityManager.Instance.ChangeGlobalGravityDirection(Vector3.right);
        }

    }
}
