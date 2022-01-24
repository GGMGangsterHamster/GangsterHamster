namespace OpenRC
{
    /// <summary>
    /// 실행될 지점
    /// </summary>
    public enum RunLevel : int
    {
        // Game
        OnGameStart = 0,
        OnGameExit,

        // Scene
        OnSceneLoad,
        OnSceneUnLoad,

        END_OF_ENUM
    }
}