using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticUIManager : MonoSingleton<StaticUIManager>
{
    public enum UIEnum
    {
        Title,
        NewGame,
        ChooseChapter,
        InGame,
        Pause,
    }

    // On, Off 할 UI패널들 받아 놓은 곳
    [SerializeField] private Transform _titleUI;
    [SerializeField] private Transform _newGameUI;
    [SerializeField] private Transform _chooseChapterUI;
    [SerializeField] private Transform _inGameUI;
    [SerializeField] private Transform _pauseUI;

    private Dictionary<UIEnum, Transform> _uiDict = new Dictionary<UIEnum, Transform>();
    private Stack<Transform> _pauseStack = new Stack<Transform>(); // panel들을 stack에서 관리

    private void Start()
    {
        _uiDict.Add(UIEnum.Title, _titleUI);
        _uiDict.Add(UIEnum.NewGame, _newGameUI);
        _uiDict.Add(UIEnum.ChooseChapter, _chooseChapterUI);
        _uiDict.Add(UIEnum.InGame, _inGameUI);
        _uiDict.Add(UIEnum.Pause, _pauseUI);
    }

    // Esc 누르면 Pause 되도록 만들기 (InGame 에서만)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseStack.Count == 0)
            {
                // InGame이라면 PauseUI 시키기?
            }
            else
            {
                ClosePanel();
            }
        }
    }

    // UIEnum 에 있는거 선택하면 그게 활성화됨
    public void OpenPanel(UIEnum uiEnum)
    {
        if (_uiDict[uiEnum].gameObject.activeSelf)
        {
            return;
        }

        if (_pauseStack.Count >= 1)
        {
            _pauseStack.Peek().gameObject.SetActive(false);
        }
        else
        {
            // 여기 Pause 걸어놓기?
        }

        _uiDict[uiEnum].gameObject.SetActive(true);
        _pauseStack.Push(_uiDict[uiEnum]);
    }

    public void ClosePanel()
    {
        if (_pauseStack.Count >= 1)
        {
            _pauseStack.Pop().gameObject.SetActive(false);

            if (_pauseStack.Count != 0)
            {
                _pauseStack.Peek().gameObject.SetActive(true);
            }
            else
            {
                // 여기 DePause 하기?
            }
        }
    }
}
