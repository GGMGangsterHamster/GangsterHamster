using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class ChooseChapterUIAction : MonoBehaviour, IUIAction
    {
        [Header("������ ����� �ִ� UI��")]
        [SerializeField] private Button _disableButton;
        [SerializeField] private Transform _stageButtonParent;

        public void ActivationActions()
        {

        }

        public void DeActivationActions()
        {

        }

        public void InitActions()
        {
            _disableButton.onClick.AddListener(() =>
            {
                // Ȱ��ȭ �� ���� �г��� ��Ȱ��ȭ��Ų��.
            });

            for(int i = 0; i < _stageButtonParent.childCount; i++)
            {
                Button stageButton = _stageButtonParent.GetChild(i).GetComponent<Button>();

                stageButton.onClick.AddListener(() =>
                {
                    // ��ư�� ���� �� �������� �ε� (���� Ŭ���� ���� ���� ���������� �� ó��)
                });
            }
        }
    }

}