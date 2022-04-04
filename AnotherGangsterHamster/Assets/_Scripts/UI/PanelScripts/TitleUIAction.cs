using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUIAction : MonoBehaviour, IUIAction
{
    [Header("������ ����� �ִ� UI��")]
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _chooseChapterButton;
    [SerializeField] private Button _optionButton;
    [SerializeField] private Button _exitButton;

    public void Actions()
    {
        _continueButton.onClick.AddListener(() =>
        {
            // ���� ù ������ �ƴ϶��
            // ����Ǿ� �ִ� �����͸� �ҷ����� "In Game UI"�� Ȱ��ȭ ��Ų��
        });

        _newGameButton.onClick.AddListener(() =>
        {
            // "New Game UI"�� Ȱ��ȭ ��Ų��
        });

        _chooseChapterButton.onClick.AddListener(() =>
        {
            // "Choose Chapter UI"�� Ȱ��ȭ ��Ų��
        });

        _optionButton.onClick.AddListener(() =>
        {
            // "Option UI"�� Ȱ��ȭ ��Ų��
        });

        _exitButton.onClick.AddListener(() =>
        {
            // ������ �����Ų��
        });
    }
}
