using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Objects.UI.Dialog.VO;

// 다이얼로그 구조
/*
{
    dialogs: [{
        id: 0,
        dialog [
            {
                index: 0,
                message: "여기에 다이얼로그 입력."
                iconID: 0,
                customCallbackID: -1,
            },
            {
                index: 1,
                message: "전기는 국산이지만 원료는 수입입니다."
                iconID: 1,
                customCallbackID: 1,
            }
        ]
    },
    {},
    {},
    ...
    }]
}
*/



namespace Objects.UI.Dialog
{
    public class DialogManager : MonoSingleton<DialogManager>
    {
        [SerializeField] private DialogUI _dialogUI;

        // 현재 다이얼로그 정보
        private int _currentDialogID = -1;
        private int _currentDialogIndex = 0;
        public int CurrentDialogID => _currentDialogID;
        public int CurrentDialogIndex => _currentDialogIndex;

        private DialogVO _dialog; // 다이얼로그
        private DialogDataVO _currentDialog; // 현재 다이얼로그


        private void Awake()
        {
            string dialogJson = (Resources.Load("Dialog") as TextAsset).text;
            if(dialogJson == null) Logger.Log("DialogManager > 다이얼로그를 찾을 수 없습니다.", LogLevel.Fatal);
            Debug.Log(dialogJson);

            _dialog = JsonUtility.FromJson<DialogVO>(dialogJson);
        }

        /// <summary>
        /// 다이얼로그를 띄웁니다
        /// </summary>
        /// <param name="id">띄울 다이얼로그 아이디</param>
        public void Show(int id = -1)
        {
            if(CurrentDialogID != -1) { // Next() 에서 호출
                Logger.Log($"DialogManager > 다이얼로그 id:{CurrentDialogID} 가 이미 실행 중. _currentDialogIndex:{_currentDialogIndex}");
            }
            else { // 다이얼로그 처음 요청
                _currentDialog = _dialog.dialogs.Find(e => e.id == id); // id 에 해당하는 다이얼로그 탐색
                _currentDialogID = id;
            }


            if(_currentDialog == null) { // null 체크
                Logger.Log($"DialogManager > 요청한 id:{id} 를 찾을 수 없습니다.", LogLevel.Error);
                return;
            }


            _dialogUI.Show(_currentDialog.dialog[_currentDialogIndex].message, null);
        }

        /// <summary>
        /// 다음 다이얼로그로 이동합니다.
        /// </summary>
        public void Next()
        {
            if(CurrentDialogID == -1) return;

            if(_currentDialog.dialog.Count <= _currentDialogIndex + 1) {
                Close();
            }
            else {
                ++_currentDialogIndex;
                Show();
            }
        }
        
        /// <summary>
        /// 다이얼로그를 닫습니다.
        /// </summary>
        public void Close()
        {
            _currentDialogID = -1;
            _currentDialogIndex = 0;
            _dialogUI.Close();
        }

    }
}