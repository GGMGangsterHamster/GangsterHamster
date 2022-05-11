using UnityEngine;
using Objects.Callback;

public class Callback2 : MonoBehaviour, ICallbackable
{
    public void Invoke(object param) {
        // TODO: test
        Debug.Log("B");
    }
}