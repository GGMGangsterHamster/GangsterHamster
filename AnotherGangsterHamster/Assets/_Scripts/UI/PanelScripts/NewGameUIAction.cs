using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameUIAction : MonoBehaviour, IUIAction
{
    [Header("각자의 기능이 있는 UI들")]
    [SerializeField] private Button _disableButton;
    [SerializeField] private Button _acceptButton;

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
            // 현재 활성화 되어 있는 패널 비활성화
        });

        _acceptButton.onClick.AddListener(() =>
        {
            // 기존의 저장 데이터 모두 삭제, 게임을 처음부터 재시작 후 "In Game UI"를 활성화
        });
    }
}
