using UnityEngine;
using Objects;

namespace Timeline.Extensions
{    
    public class SimpleActiveClass : MonoBehaviour, IActivated, IEventable
    {
        public bool Activated => activated;
        public bool activated = false;

        public void Active(GameObject other)
        {
            activated = true;
        }

        public void Deactive(GameObject other)
        {
            activated = false;
        }
    }
}