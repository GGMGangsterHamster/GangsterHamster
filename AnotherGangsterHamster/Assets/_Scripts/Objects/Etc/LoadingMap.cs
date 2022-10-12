using Effects.Fullscreen;
using Objects.Trigger;
using UnityEngine;

namespace Objects.Etc
{
    [RequireComponent(typeof(SceneLoadTrigger))]
    public class LoadingMap : MonoBehaviour, IEventable
    {
        public float fadeDuration = 2.0f;
        public float postWaitDuration = 0.5f;

        private Fade _fade = null;
        private SceneLoadTrigger _sceneLoader;
        
        private void Awake()
        {
            _sceneLoader = GetComponent<SceneLoadTrigger>();
            _fade        = FindObjectOfType<Fade>();
        }

        public void Active(GameObject other)
        {
            _fade.FadeOut(fadeDuration, () => {
                Invoke(nameof(LoadScene), postWaitDuration);
            });
        }

        private void LoadScene()
        {
            _sceneLoader.Active(null);
        }

        public void Deactive(GameObject other) { }
    }
}