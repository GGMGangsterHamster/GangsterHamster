using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;

public class DialogManager : MonoSingleton<DialogManager>
{
   private Dictionary<string, Dialog> _dialogDictionary;

   protected override void Awake()
   {
      base.Awake();

      _dialogDictionary = new Dictionary<string, Dialog>();

      List<TextAsset> dialogs = Resources.LoadAll<TextAsset>("Dialogs").ToList();

      dialogs.ForEach(e => {
         Debug.Log("Loaded " + e.name);
         _dialogDictionary.Add(e.name, JsonUtility.FromJson<Dialog>(e.text));
      });
   }

   /// <summary>
   /// 다이얼로그를 불러옵니다.
   /// </summary>
   /// <param name="type">파일 이름</param>
   /// <param name="id">아이디</param>
   /// <returns>못 찾으면 null</returns>
   public InnerDialog GetDialog(string type, int id)
   {
      if (!_dialogDictionary.ContainsKey(type))
      {
         Logger.Log($"Cannot find dialog type: {type}.",
            LogLevel.Error);
         return null;
      }

      InnerDialog dialog =
         _dialogDictionary[type].dialogs.Find(e => e.id == id);

      if (dialog == null)
      {
         Logger.Log($"Cannot find dialog id: {id} for type: {type}",
            LogLevel.Error);
         return null;
      }

      return dialog;
   }
}

[Serializable]
public class Dialog
{
   public List<InnerDialog> dialogs;
}

[Serializable]
public class InnerDialog
{
   public int id;
   public string text;
}