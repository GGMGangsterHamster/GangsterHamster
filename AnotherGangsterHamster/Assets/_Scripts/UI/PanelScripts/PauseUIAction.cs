using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIAction : MonoBehaviour, IUIAction
{
    [Header("각자의 기능이 있는 UI들")]
    [SerializeField] private Button _fullScreenModeButton;
    [SerializeField] private Button _windowScreenModeButton;
    [SerializeField] private Button _1920x1080ResolutionButton;
    [SerializeField] private Button _2560x1080ResolutionButton;
    [SerializeField] private Button _goTitleButton;
    [SerializeField] private Button _gameRestartButton;

    [SerializeField] private Scrollbar _soundScrollbar;
    [SerializeField] private Scrollbar _sensitivityScrollbar;

    public void ActivationActions()
    {
        // _soundScrollbar.value 를 지금 사운드 설정에 따라서 초기화 시켜주고 변환되는 값을 적용 시켜주기도 해야 함
        // _sensitivityScrollbar.value 를 지금 민감도 설정에 따라서 초기화 시켜주고 변환되는 값을 적용 시켜주기도 해야 함
    }

    public void DeActivationActions()
    {

    }

    public void InitActions()
    {
        _fullScreenModeButton.onClick.AddListener(() =>
        {
            // 전체화면으로 변환
        });

        _windowScreenModeButton.onClick.AddListener(() =>
        {
            // 창화면으로 전환
        });

        _1920x1080ResolutionButton.onClick.AddListener(() =>
        {
            // 1920x1080 해상도로 전환
        });

        _2560x1080ResolutionButton.onClick.AddListener(() =>
        {
            // 2560x1080 해상도로 전환
        });

        _goTitleButton.onClick.AddListener(() =>
        {
            // 현재 UI 비활성화 시키고 "Title UI"로 변환
        });

        _gameRestartButton.onClick.AddListener(() =>
        {
            // 현재의 스테이지를 재시작
        });
    }
}
