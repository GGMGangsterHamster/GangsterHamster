using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public List<MovingObject> mObjList = new List<MovingObject>();

    private enum OnOff
    {
        On,
        Off
    }

    private OnOff status = OnOff.Off;

    public void Click()
    {
        NextStatus();

        if (status == OnOff.On) // 켜지는 거 (On으로 되는거)
        {
            On();
        }
        else if(status == OnOff.Off) // 꺼지는 거 (Off으로 되는거)
        {
            Off();
        }
    }

    public virtual void On()
    {
        mObjList.ForEach(x => x.StartDontRepeatMove(true));
    }

    public virtual void Off()
    {
        mObjList.ForEach(x => x.StartDontRepeatMove(false));
    }

    private void NextStatus()
    {
        if(status == OnOff.Off)
        {
            status = OnOff.On;
        }
        else
        {
            status += 1;
        }
    }
}
