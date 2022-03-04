using Objects.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicButton : MonoBehaviour
{
    public List<GameObject> mObjList = new List<GameObject>(); // On Off �Ҷ� �����ؾ��ϴ� ������Ʈ ����Ʈ

    public Color OnColor;   // Ȱ��ȭ ���� ����
    public Color OffColor;  // ��Ȱ��ȭ ���� ����

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
            // ���⼭ ����Ǿ� �ִ� ������Ʈ���� Ȱ��ȭ ��Ű�� ��
            On();
        }
    }

    // �ϴ��� ��ȸ�����ν� ����ϹǷ� �ּ�ȭ ���ѳ��� ���߿� �ּ��� Ǯ�� ��ȸ���� �ƴϰԵ�
    //private void OnCollisionExit(Collision collision)
    //{
    //    Off();
    //}

    public void On() // Ȱ��ȭ ��
    {
        mObjList.ForEach(x => x.GetComponent<IActivateObject>().On());
        _myMat.color = OnColor;
    }

    public void Off() // ��Ȱ��ȭ ��
    {
        mObjList.ForEach(x => x.GetComponent<IActivateObject>().Off());
        _myMat.color = OffColor;
    }
}
