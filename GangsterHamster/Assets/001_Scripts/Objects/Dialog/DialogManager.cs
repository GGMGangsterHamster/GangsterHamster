using UnityEngine;
using UnityEngine.UI;

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
                customCallbackId: -1,
            },
            {
                index: 1,
                message: "전기는 국산이지만 원료는 수입입니다."
                iconID: 1,
                customCallbackId: 1,
            }
        ]
    },
    {},
    {},
    ...
    }]
}
*/



namespace Objects.UI
{
    public class DialogManager : MonoSingleton<DialogManager>
    {
        [SerializeField] private DialogUI dialogUI;
        

        private void Awake()
        {
            string dialogJson = (Resources.Load("Dialog") as TextAsset).text;
            if(dialogJson == null) Logger.Log("DialogManager > 다이얼로그를 찾을 수 없습니다.", LogLevel.Fatal);
            Debug.Log(dialogJson);
        }


        public void Show(int id) { }
        public void Next() { }
        public void Close() { }
    }
}