using Objects;
using UnityEngine;

namespace Matters.Behaviour
{
    public class ObjectActiveStatus : MonoBehaviour, IEventable
    {
        public void Active(GameObject other)
        {
            gameObject.SetActive(true);
        }

        public void Deactive(GameObject other)
        {
            gameObject.SetActive(false);
        }
    }
}