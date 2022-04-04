using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUIAction : MonoBehaviour, IUIAction
{
    [Header("각자의 기능이 있는 UI들")]
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _chooseChapterButton;
    [SerializeField] private Button _optionButton;
    [SerializeField] private Button _exitButton;

    public void Actions()
    {
        _continueButton.onClick.AddListener(() =>
        {
            // 만약 첫 실행이 아니라면
            // 저장되어 있는 데이터를 불러오고 "In Game UI"를 활성화 시킨다
        });

        _newGameButton.onClick.AddListener(() =>
        {
            // "New Game UI"를 활성화 시킨다
        });

        _chooseChapterButton.onClick.AddListener(() =>
        {
            // "Choose Chapter UI"를 활성화 시킨다
        });

        _optionButton.onClick.AddListener(() =>
        {
            // "Option UI"를 활성화 시킨다
        });

        _exitButton.onClick.AddListener(() =>
        {
            // 게임을 종료시킨다
        });
    }
}
