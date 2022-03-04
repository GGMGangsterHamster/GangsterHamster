using Objects.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicButton : MonoBehaviour
{
    public List<GameObject> mObjList = new List<GameObject>(); // On Off 할때 반응해야하는 오브젝트 리스트

    public Color OnColor;   // 활성화 때의 색깔
    public Color OffColor;  // 비활성화 때의 색깔

    private Material _myMat;
    private void Awake()
    {
        _myMat = GetComponent<MeshRenderer>().material;
        _myMat.color = OffColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out IInteractableObject a) 
        || collision.gameObject.TryGetComponent(out WeaponSkill w))
        {
            // 여기서 연결되어 있는 오브젝트들을 활성화 시키면 됨
            On();
        }
    }

    // 일단은 일회용으로써 사용하므로 주석화 시켜놓은 나중에 주석을 풀면 일회용이 아니게됨
    //private void OnCollisionExit(Collision collision)
    //{
    //    Off();
    //}

    public void On() // 활성화 됨
    {
        mObjList.ForEach(x => x.GetComponent<IActivateObject>().On());
        _myMat.color = OnColor;
    }

    public void Off() // 비활성화 됨
    {
        mObjList.ForEach(x => x.GetComponent<IActivateObject>().Off());
        _myMat.color = OffColor;
    }
}
