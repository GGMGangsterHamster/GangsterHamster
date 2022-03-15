using UnityEngine;
using Effects.Global;

public class ChromaticAberrationTest : MonoBehaviour {

    private void Update() {

        if(Input.GetKeyDown(KeyCode.F)) {
            GlobalChromaticAberration.Instance.Decrease(1.0f, 0.0f, 1.0f, () => {
                Debug.Log("Done!");
            });
        }
    }
}