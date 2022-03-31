using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIManagement : MonoBehaviour
{
    [SerializeField] private Scrollbar _gameSoundScrollbar;
    [SerializeField] private Scrollbar _gameSensitivityScrollbar;

    [SerializeField] private Button _fullScreenModeBtn;
    [SerializeField] private Button _windowScreenModeBtn;
    [SerializeField] private Button _1920x1080ResolutionBtn;
    [SerializeField] private Button _2560x1080ResolutionBtn;
    [SerializeField] private Button _goTitleBtn;
    [SerializeField] private Button _gameRestartBtn;

    [SerializeField] private Button _disabledBtn;

    // ���Ǹ� ���ؼ� ��� ���⿡ �� �������� ������ ���߿� �� �������� �̵� ��ų����
    private FullScreenMode _screenMode = FullScreenMode.FullScreenWindow;
    private int _width = 1920;
    private int _height = 1080;

    private float _gameSoundValue = 0f;
    private float _gameSensitivityValue = 0f;

    public float GameSoundValue
    {
        get
        {
            if(_gameSoundScrollbar != null)
            {
                _gameSoundValue = _gameSoundScrollbar.value;
            }

            return _gameSoundValue;
        }
    }

    public float GameSensitivityValue
    {
        get
        {
            if(_gameSensitivityScrollbar != null)
            {
                _gameSensitivityValue = _gameSensitivityScrollbar.value;
            }

            return _gameSensitivityValue;
        }
    }

    private void Start()
    {
        _fullScreenModeBtn.onClick.AddListener(() =>
        {
            // ���߿� ���⼭ �� ���� ȭ�� ���� �޾ƿͼ� �װɷ� �ػ� ���ؾ� ��
            _screenMode = FullScreenMode.FullScreenWindow;
            Screen.SetResolution(_width, _height, _screenMode);
        });

        _windowScreenModeBtn.onClick.AddListener(() =>
        {
            // ���⵵ ���������� �ػ� �޾ƿͼ� ����� ��
            _screenMode = FullScreenMode.Windowed;
            Screen.SetResolution(_width, _height, _screenMode);
        });

        _1920x1080ResolutionBtn.onClick.AddListener(() =>
        {
            _width = 1920;
            _height = 1080;
            Screen.SetResolution(_width, _height, _screenMode);
        });

        _2560x1080ResolutionBtn.onClick.AddListener(() =>
        {
            _width = 2560;
            _height = 1080;
            Screen.SetResolution(_width, _height, _screenMode);
        });

        _goTitleBtn.onClick.AddListener(() =>
        {
            // Ÿ��Ʋ ȭ������ �̵�
        });

        _gameRestartBtn.onClick.AddListener(() =>
        {
            // �� ���� ���� �ٽý���
        });

        _disabledBtn.onClick.AddListener(() =>
        {
            StaticUIManager.Instance.ClosePanel();
        });
    }

}
