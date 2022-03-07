using Objects.Interactable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalObjA : ObjectA
{
    public override void Interact(Action callback = null)
    {
        callback?.Invoke();
    }

    public override void Release()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        transform.parent = null;
    }
}
