using Objects;
using Objects.StageObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCountRequirement : MonoBehaviour
{
    [SerializeField] private List<TriggerInteractableObject> countCheckList 
        = new List<TriggerInteractableObject>();

    [SerializeField]
    private List<InteractionButton> icountCheckList
       = new List<InteractionButton>();

    private int _count = 0;
    public bool Checked
    {
        get
        {
            _count = 0;

            for(int i = 0; i < countCheckList.Count; i++)
            {
                if(countCheckList[i].Activated) 
                {
                    _count++;
                }
            }

            for (int i = 0; i < icountCheckList.Count; i++)
            {
                if (icountCheckList[i].Activated)
                {
                    _count++;
                }
            }

            return _count == countCheckList.Count + icountCheckList.Count;
        }
    }

}
