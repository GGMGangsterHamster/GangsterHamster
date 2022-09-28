using Sound;
using UnityEngine;

namespace _Core.Initialize.InitScripts
{
    public class AutoSpawn : InitBase
    {
        public override RunLevel RunLevel => RunLevel.SCENE_LOAD;

        public override void Call()
        {
            Debug.Log($"Loading {SoundManager.Instance.GetType()}");
        }
    }
}