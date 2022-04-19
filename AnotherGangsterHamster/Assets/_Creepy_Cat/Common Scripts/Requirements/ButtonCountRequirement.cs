using Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCountRequirement : MonoBehaviour
{
    [SerializeField] private List<CollisionInteractableObject> countCheckList 
        = new List<CollisionInteractableObject>();

    private int _count = 0;
    public bool Checked
    {
        get
        {
            _count = 0;

            for(int i = 0; i < countCheckList.Count; i++)
            {
                if(countCheckList[i]._activated)
                {
                    _count++;
                }
            }

            return _count == countCheckList.Count;
        }
    }

}
