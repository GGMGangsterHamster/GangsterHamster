using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;
using UI.PanelScripts;

public class PressUIButtonSound : SoundController
{
    [SerializeField] private TitleUIAction _titleUIAction;
    [SerializeField] private ChooseChapterUIAction _chooseChapterUIAction;
    [SerializeField] private NewGameUIAction _newGameUIAction;
    [SerializeField] private OptionAction _optionAction;
    [SerializeField] private PauseUIAction _pauseUIAction;

    void Start()
    {
        _titleUIAction.continueButton.onClick.AddListener(PressButton);
        _titleUIAction.newGameButton.onClick.AddListener(PressButton);
        _titleUIAction.chooseChapterButton.onClick.AddListener(PressButton);
        _titleUIAction.optionButton.onClick.AddListener(PressButton);
        _titleUIAction.exitButton.onClick.AddListener(PressButton);

        _chooseChapterUIAction.disableButton.onClick.AddListener(PressButton);

        _newGameUIAction.acceptButton.onClick.AddListener(PressButton);
        _newGameUIAction.disableButton.onClick.AddListener(PressButton);

        _optionAction.disableButton.onClick.AddListener(PressButton);

        _pauseUIAction.goTitleButton.onClick.AddListener(PressButton);
        _pauseUIAction.gameRestartButton.onClick.AddListener(PressButton);
        _pauseUIAction.disableButton.onClick.AddListener(PressButton);
    }

    public void PressButton()
    {
        SoundManager.Instance.Play("UIButtonClick");
    }

    public override void PlaySound(object obj)
    {
        
    }
}
