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
        string json = JsonUtility.ToJson(vo);

        FileStream fs = new FileStream(path, FileMode.Create);

        StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Unicode);

        sw.Write(json);
        sw.Close();
    }
}

