using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Core.ResourceStreaming
{
    public class Streamer : MonoBehaviour
    {
        [SerializeField]
        List<AssetReferenceGameObject> grandStages 
            = new List<AssetReferenceGameObject>();

        public void Start()
        {
            
        }
    }
}