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
        path = Path.Combine(Directory.GetCurrentDirectory(), path);

        string json = JsonUtility.ToJson(vo);
        
        if(Directory.Exists(path))
            Directory.CreateDirectory(path);

        File.WriteAllText(path, json);
    }
}

