using UnityEngine;

public class GameStatus : Singleton<GameStatus>, ISingletonObject
{
    /// <summary>
    /// 다이얼로그가 열려 있는지s
    /// </summary>
    public bool IsDialogOpen { get; set; }
    // public bool 
}