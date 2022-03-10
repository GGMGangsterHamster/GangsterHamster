using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gravity.Object.Management;

public class GravityTest : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            // GravityManager.Instance.ChangeGlobalGravityDirection(45.0f);
            GravityManager.Instance.ChangeGlobalGravityDirection(new Vector3(0.0f, -1.0f, 1.0f).normalized);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            GravityManager.Instance.ChangeGlobalGravityDirection(Vector3.down);
        }

        // if(Input.GetKeyDown(KeyCode.P)) {
        //     Gravity.Object.Management.GravityManager.Instance.ChangeGlobalGravityDirection(Vector3.up);
        // }
        // if(Input.GetKeyDown(KeyCode.Semicolon)) {
        //     Gravity.Object.Management.GravityManager.Instance.ChangeGlobalGravityDirection(Vector3.down);
        // }
        // if(Input.GetKeyDown(KeyCode.L)) {
        //     Gravity.Object.Management.GravityManager.Instance.ChangeGlobalGravityDirection(Vector3.left);
        // }
        // if(Input.GetKeyDown(KeyCode.Quote)) {
        //     Gravity.Object.Management.GravityManager.Instance.ChangeGlobalGravityDirection(Vector3.right);
        // }

    }
}
