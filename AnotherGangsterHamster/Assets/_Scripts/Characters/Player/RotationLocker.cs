using Matters.Gravity;
using UnityEngine;


namespace Characters.Player
{    
    public class RotationLocker : MonoBehaviour
    {
        Vector3 _lock = new Vector3(0.0f, 1.0f, 0.0f);

        private void Start()
        {
            FindObjectOfType<GravityManager>()
                .OnGravityChanged
                .AddListener(gravity => {
                Vector3 dir = gravity._direction;
                _lock = new Vector3(Mathf.Abs(dir.x),
                                    Mathf.Abs(dir.y),
                                    Mathf.Abs(dir.z));

            });
        }

        private void Update()
        {
            Debug.LogError(_lock);
            Debug.LogError(Utils.Multiply(transform.eulerAngles, _lock));
            transform.eulerAngles = Utils.Multiply(transform.eulerAngles, _lock);
        }
    }
}