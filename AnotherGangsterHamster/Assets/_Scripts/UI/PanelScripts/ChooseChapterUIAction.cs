using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseChapterUIAction : MonoBehaviour, IUIAction
{
    [Header("������ ����� �ִ� UI��")]
    [SerializeField] private Button _disableButton;
    [SerializeField] private List<Button> _stageButtons;

    public void Actions()
    {
        _disableButton.onClick.AddListener(() =>
        {
            // Ȱ��ȭ �� ���� �г��� ��Ȱ��ȭ��Ų��.
        });

        foreach (Button stageButton in _stageButtons)
            stageButton.onClick.AddListener(() =>
            {
                // ��ư�� ���� �� �������� �ε� (���� Ŭ���� ���� ���� ���������� �� ó��)
            });
    }
}
