using UnityEngine;
using UnityEngine.UI;

namespace Objects.UI
{
    public class FloatingUI : MonoBehaviour
    {
        [SerializeField] private Text messageText;
        [SerializeField] private Image background;

        private Transform targetTransform;

        private void Awake()
        {
            gameObject.SetActive(false);
        }


        public void Set(string msg, Transform trm)
        {
            messageText.text = msg;
            gameObject.SetActive(true);
            targetTransform = trm;
        }

        public void UnSet()
        {
            messageText.text = "";
            gameObject.SetActive(false);
            targetTransform = null;
        }

        private void Update()
        {
            if(targetTransform != null) {
                transform.position = targetTransform.position;
                transform.rotation = targetTransform.rotation;
            }
        }
    }
}