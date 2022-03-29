using System;
using System.Collections.Generic;

static public class Contains
{

   /// <summary>
   /// 요약: <br/>
   ///    dictionary 에 key 가 존재하는지 확인합니다. <br/>
   /// <br/>
   /// 반환: <br/>
   ///     존재하지 않거나 오류가 발생한 경우 false <br/>
   /// </summary>
   static public bool In<TValue>(string key,
                                 object dictionary, // fuck it
                                 Action onExists = null,
                                 Action onNull = null)
   {
      if (onNull == null)
      {
         onNull += () =>
         {
            Logger.Log($"Contains.In > Key: {key} does not exist",
                     LogLevel.Fatal);
         };
      }

      bool result;

      if (dictionary is Dictionary<string, TValue>) // Dictionary 타입인 경우
      {
         var dict = dictionary as Dictionary<string, TValue>;
         result = dict.ContainsKey(key);
      }
      else // Dictionary 타입이 아닌 경우
      {
         Logger.Log("Contains.In > type mismatch: dictionary",
                  LogLevel.Fatal);
         return false;
      }

      if (result)
         onExists?.Invoke();
      else
         onNull();

      return result;

   }
}