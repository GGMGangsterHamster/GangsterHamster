using System.Collections.Generic;
using UnityEngine;

namespace UI.Dialog
{
    public class KeyGuide : MonoBehaviour
    {
        private Dictionary<Key, KeyObject> _keyObject
            = new Dictionary<Key, KeyObject>();

        private void Start()
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                GameObject obj = transform.GetChild(i).gameObject;

                Key key = (Key)Key.Parse(typeof(Key), obj.name);
                _keyObject.Add(key, obj.GetComponent<KeyObject>());
            }
        }

        public void Show(GameObject obj)
        {
            Show((Key)Key.Parse(typeof(Key), obj.name));
        }

        public void Show(Key key)
        {
            foreach (var obj in _keyObject)
            {
                if (obj.Key == key)
                    continue;

                obj.Value.Deactive(null);
            }

            _keyObject[key].Active(null);
        }

        public void Disable()
        {
            foreach (var obj in _keyObject.Values)
            {
                obj.Deactive(null);
            }
        }
    };

    [System.Serializable]
    public enum Key
    {
        Ctrl,
        E,
        LClick,
        RClick,
        RClickPress,
        R,
        Space
    };
}