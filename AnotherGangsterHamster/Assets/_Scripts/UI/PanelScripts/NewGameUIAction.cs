using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameUIAction : MonoBehaviour, IUIAction
{
    [Header("������ ����� �ִ� UI��")]
    [SerializeField] private Button _disableButton;
    [SerializeField] private Button _acceptButton;

    public void Actions()
    {
        _disableButton.onClick.AddListener(() =>
        {
            // ���� Ȱ��ȭ �Ǿ� �ִ� �г� ��Ȱ��ȭ
        });

        _acceptButton.onClick.AddListener(() =>
        {
            // ������ ���� ������ ��� ����, ������ ó������ ����� �� "In Game UI"�� Ȱ��ȭ
        });
    }
}
