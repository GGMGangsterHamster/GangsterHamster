using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    [System.Serializable]
    public class HandModeVO : MonoBehaviour
    {
        public bool isRightHand;

        public HandModeVO(bool isRightHand)
        {
            this.isRightHand = isRightHand;
        }
    }
}