using Objects;
using Objects.StageObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCountRequirement : MonoBehaviour
{
    public bool reverse = false; // 말 그대로 조건을 반전 시키는 변수

    public Action<bool> changedEvent;

    // TriggetInteractableObject 를 조건으로 받는 리스트
    [SerializeField]
    private List<TriggerInteractableObject> triggerCheckList
        = new List<TriggerInteractableObject>();

    // InteractionButton 를 조건으로 받는 리스트
    [SerializeField]
    private List<InteractionButton> interactionCheckList
       = new List<InteractionButton>();

    private bool beforeChecked;

    private int _count = 0;
    public bool Checked
    {
        get
        {
            _count = 0;

            for (int i = 0; i < triggerCheckList.Count; i++)
            {
                if (triggerCheckList[i].Activated)
                {
                    _count++;
                }
            }

            for (int i = 0; i < interactionCheckList.Count; i++)
            {
                if (interactionCheckList[i].Activated)
                {
                    _count++;
                }
            }

            bool result = _count == triggerCheckList.Count + interactionCheckList.Count;

            return (reverse ? !result : result);
        }
    }

    private void Awake()
    {
        beforeChecked = Checked;
    }

    private void Update()
    {
        if (beforeChecked != Checked)
        {
            beforeChecked = Checked;
            Debug.Log("Action : " + Checked);
            changedEvent?.Invoke(Checked);
        }
    }
}
