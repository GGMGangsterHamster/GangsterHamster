using System.IO;
using UnityEngine;

public class JsonFileManager : MonoBehaviour
{
    /// <summary>
    /// Reads json data from path/folderName/fileName
    /// </summary>
    /// <param name="fileName">Filename</param>
    /// <param name="folderName">Folder name<br/>default is wave</param>
    /// <param name-"fullpath">foldername is full path to file</param>
    /// <returns>Json Data, null when there is no file</returns>
    static public string Read(string fileName, string folderName = "wave", bool fullpath = false)
    {
        string path = fullpath ? Path.Combine(folderName, fileName) : Path.Combine(Application.persistentDataPath, folderName, fileName);

        if (!File.Exists(path)) // 파일 존제 채크
        {
            return null;
        }

        return File.ReadAllText(path);
    }


    /// <summary>
    /// <b>## THIS OVERRIDES EXISTING FILE ##</b><br/>
    /// Writes json data from path/folderName/fileName<br/>
    /// Automaticly craetes file when there is no such file named fileName
    /// </summary>
    /// <param name="fileName">FileName</param>
    /// <param name="json">The json string that you want to save</param>
    /// <param name="folderName">Folder name<br/>default is wave</param>
    /// <param name-"fullpath">foldername is full path to file</param>
    static public void Write(string fileName, string json, string folderName = "wave", bool fullpath = false)
    {
        string path = fullpath ? Path.Combine(folderName, fileName) : Path.Combine(Application.persistentDataPath, folderName, fileName);

        if (!File.Exists(path)) // 파일 존제 안할 시 파일 생성
        {
            // Directory.CreateDirectory($"{Application.persistentDataPath}/{folderName}/"); // mkdir
            Directory.CreateDirectory(fullpath ? folderName : Path.Combine(Application.persistentDataPath, folderName)); // mkdir
            File.Create(path).Close(); // touch
            Debug.Log("Created folder at: " + (fullpath ? folderName : Path.Combine(Application.persistentDataPath, folderName))); // nvim
        }

        StreamWriter outputFile = new StreamWriter(path); // 파일에 작성
        outputFile.WriteLine(json);
        Debug.Log("Saved: " + path);
        outputFile.Close();
    }
}