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
            // TODO: 일단 라이트는 생각하지 말고 프리팹 동적 로딩을 해 봅시다
        }
    }
}