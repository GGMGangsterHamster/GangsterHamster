using UnityEngine;
using System.IO;

static partial class Utils
{
   /// <summary>
   /// Json 을 로드한 뒤 VO 에 넣어주는 함수
   /// </summary>
   /// <typeparam name="T">VO 타입</typeparam>
   /// <param name="path">Json 경로 (Resoruces 폴더 안의)</param>
   /// <returns>Json 이 들어간 vo</returns>
   public static T JsonToVO<T>(string path) where T : class
   {
      path = Path.Combine(Directory.GetCurrentDirectory(), path);
      string json = File.ReadAllText(path);

      return JsonUtility.FromJson<T>(json);
   }
}
