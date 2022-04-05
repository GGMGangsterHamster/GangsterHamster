using UnityEngine;
using System.IO;

static partial class Utils
{
    /// <summary>
    /// �Է� ���� ����ȭ �� Ŭ������ VO�� json�������� ��ȯ �� �� 
    /// ���Ͽ� �Է��ϴ� �Լ��̴�
    /// </summary>
    /// <typeparam name="T">VO Ÿ��</typeparam>
    /// <param name="vo">���Ͽ� �Է��� vo</param>
    public static void VOToJson<T>(string path, T vo) where T : class
    {
        string json = JsonUtility.ToJson(vo);

        FileStream fs = new FileStream(path, FileMode.Create);

        StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Unicode);

        sw.Write(json);
        sw.Close();
    }
}

