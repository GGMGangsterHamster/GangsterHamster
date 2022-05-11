using UnityEngine;
using Effects.Global;

public class DoFTest : MonoBehaviour
{
    private void Update() {
        if(Input.GetKeyDown(KeyCode.H)) {
            GlobalDoF.Instance.Decrease(1.0f, 0.0f, 1.0f, () => {
                Debug.Log("Done!");
            });
        }
    }
}