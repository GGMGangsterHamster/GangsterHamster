using System;
using System.IO;
using System.Threading.Tasks;


// FIXME: 파일 생성 후 로그 작성 시 Sharing violation

public enum LogLevel : byte
{
   Normal = 0,
   Warning,
   Error,
   Fatal
}

static public class Logger
{
   static private string _dir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", ".Logs"); // 로그 폴더 위치
   static private string _date = DateTime.Today.ToString().Split(' ')[0].Replace('/', '_'); // 로그 파일 생성일
   static private string _filePath = Path.Combine(_dir, $"{_date}.log"); // 로그 파일 위치

   /// <summary>
   /// 로그를 작성합니다.
   /// </summary>
   /// <param name="log">남길 로그</param>
   /// <param name="logLevel">심각도</param>
   static public void Log(string log, LogLevel logLevel = LogLevel.Normal)
   {

      if (!Directory.Exists(_dir))
      { // 폴더가 없다면 만듬
         Directory.CreateDirectory(_dir);
      }

      if (!File.Exists(_filePath))
      { // 파일이 없다면 만듬
         File.Create(_filePath).Close();
      }



      Write(TemplateGenerator(log, logLevel), logLevel);
   }

   #region 내부 함수
   static private void Write(string log, LogLevel logLevel)
   {
      using (StreamWriter logFile = new StreamWriter(_filePath, append: true))
      {
         logFile.Write(log);
      }

      switch (logLevel)
      {
         case LogLevel.Normal:
            UnityEngine.Debug.Log(log);
            break;
         case LogLevel.Warning:
            UnityEngine.Debug.LogWarning(log);
            break;
         default:
            UnityEngine.Debug.LogError(log);
            break;
      }
   }

   static private string TemplateGenerator(string log, LogLevel logLevel)
   {
      return $"[{DateTime.Now.ToString().Replace('/', '.')}] [{logLevel}]\t {log}\r\n";
   }

   #endregion // 내부 함수

}