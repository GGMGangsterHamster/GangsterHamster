using UnityEngine;

namespace Utils.Camera
{
	public class ManagedCamera : MonoBehaviour
	{
		public 	string 				  	_identifier;
		private 	UnityEngine.Camera 	_camera;

		private void Awake()
		{
			_camera = GetComponent<UnityEngine.Camera>();

			#if UNITY_EDITOR
			NULL.Check(_camera, () => {
				this.enabled = false;
			});
			#endif

			CameraChanger.Instance.AddToList(_identifier, _camera);
		}
	}
}