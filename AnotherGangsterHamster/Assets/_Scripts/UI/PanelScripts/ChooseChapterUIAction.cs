using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseChapterUIAction : MonoBehaviour, IUIAction
{
    [Header("각자의 기능이 있는 UI들")]
    [SerializeField] private Button _disableButton;
    [SerializeField] private List<Button> _stageButtons;

    public void Actions()
    {
        _disableButton.onClick.AddListener(() =>
        {
            // 활성화 된 현재 패널을 비활성화시킨다.
        });

        foreach (Button stageButton in _stageButtons)
            stageButton.onClick.AddListener(() =>
            {
                // 버튼에 저장 된 스테이지 로딩 (아직 클리어 하지 못한 스테이지는 블러 처리)
            });
    }
}
