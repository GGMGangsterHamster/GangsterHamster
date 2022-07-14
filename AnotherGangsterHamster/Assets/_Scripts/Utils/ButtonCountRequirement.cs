using Objects;
using Objects.Interaction;
using Objects.StageObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCountRequirement : MonoBehaviour
{
    public bool reverse = false; // 말 그대로 조건을 반전 시키는 변수

    public Action<bool> changedEvent;

    [SerializeField]
    private List<Interactable> _checkList
        = new List<Interactable>();

    private bool beforeChecked;

    private int _count = 0;
    public bool Checked
    {
        get
        {
            _count = 0;

            for (int i = 0; i < _checkList.Count; i++)
            {
                if (_checkList[i].Activated)
                {
                    _count++;
                }
            }

            bool result = _count == _checkList.Count;

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
