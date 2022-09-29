using UnityEngine;
using Objects;

namespace Timeline.Extensions
{    
    public class CompareAndActiveClass : MonoBehaviour, IActivated
    {
        public bool Activated => activated;
        public bool activated = false;
        public int number = 0;

        public void Active(int number)
        {
            if (this.number == number)
                activated = true;
        }

        public void Deactive(int number)
        {
            activated = false;
        }
    }
}