using UnityEngine;
using Effects.Camrea;

public class CameraShakeTest : MonoBehaviour {


    private void Update() {
        if(Input.GetKey(KeyCode.G)) {
            CameraShake.Instance.Shake(0.1f, 0.1f, 0.2f);
        }
    }
}