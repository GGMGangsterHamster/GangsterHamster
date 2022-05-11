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
        Option,
        Pause,
    }

    // On, Off �� UI�гε� �޾� ���� ��
    [SerializeField] private Transform _titleUI;
    [SerializeField] private Transform _newGameUI;
    [SerializeField] private Transform _chooseChapterUI;
    [SerializeField] private Transform _inGameUI;
    [SerializeField] private Transform _optionUI;
    [SerializeField] private Transform _pauseUI;

    private Dictionary<UIEnum, Transform> _uiDict = new Dictionary<UIEnum, Transform>();
    private Stack<Transform> _pauseStack = new Stack<Transform>(); // panel���� stack���� ����

    private void Start()
    {
        _uiDict.Add(UIEnum.Title, _titleUI);
        _uiDict.Add(UIEnum.NewGame, _newGameUI);
        _uiDict.Add(UIEnum.ChooseChapter, _chooseChapterUI);
        _uiDict.Add(UIEnum.InGame, _inGameUI);
        _uiDict.Add(UIEnum.Option, _optionUI);
        _uiDict.Add(UIEnum.Pause, _pauseUI);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _inGameUI.gameObject.SetActive(true);
    }

    // Esc ������ Pause �ǵ��� ����� (InGame ������)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseStack.Count == 0)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                OpenPanel(UIEnum.Pause);
                // InGame�̶�� PauseUI ��Ű��?
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                ClosePanel();
                _inGameUI.gameObject.SetActive(true);
            }
        }
    }

    // UIEnum �� �ִ°� �����ϸ� �װ� Ȱ��ȭ��
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
            _inGameUI.gameObject.SetActive(false);
            // ���� Pause �ɾ����?
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
                _inGameUI.gameObject.SetActive(true);
                // ���� DePause �ϱ�?
            }
        }
    }
}
