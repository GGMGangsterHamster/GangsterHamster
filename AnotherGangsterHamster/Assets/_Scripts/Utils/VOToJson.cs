using UnityEngine;
using System.IO;

static partial class Utils
{
    /// <summary>
    /// 입력 받은 직렬화 된 클래스인 VO를 json형식으로 변환 한 뒤 
    /// 파일에 입력하는 함수이다
    /// </summary>
    /// <typeparam name="T">VO 타입</typeparam>
    /// <param name="vo">파일에 입력할 vo</param>
    public static void VOToJson<T>(string path, T vo) where T : class
    {
        path = Path.Combine(Directory.GetCurrentDirectory(), path);

        string json = JsonUtility.ToJson(vo);
        
        if(Directory.Exists(path))
            Directory.CreateDirectory(path);

        File.WriteAllText(path, json);
    }
}

