using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation.Lumo
{
    public class LumoPartsAnimation : MonoBehaviour
    {
        enum LumoPartsEnum
        {
            Idle,
            Use,
            Reset
        }

        LumoPartsEnum _curEnum = LumoPartsEnum.Idle;

        private void Update()
        {
            switch(_curEnum)
            {
                case LumoPartsEnum.Idle:
                    break;
                case LumoPartsEnum.Use:
                    break;
                case LumoPartsEnum.Reset:
                    break;
            }
        }
    }
}