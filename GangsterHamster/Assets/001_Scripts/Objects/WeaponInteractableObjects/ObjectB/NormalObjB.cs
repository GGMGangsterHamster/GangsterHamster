using Objects.Interactable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalObjB : ObjectB
{
    public override void Interact(Action callback = null)
    {
        callback?.Invoke();
    }

    public override void Initialize(Action callback = null)
    {

    }

    public override void Release()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        transform.parent = null;
    }
}
