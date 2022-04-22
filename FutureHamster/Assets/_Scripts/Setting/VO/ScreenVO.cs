namespace Setting.VO
{
    [System.Serializable]
    public class ScreenVO
    {
        public bool isFullScreen;
        public int width;
        public int height;

        public ScreenVO(bool isFullScreen, int width, int height)
        {
            this.isFullScreen = isFullScreen;
            this.width = width;
            this.height = height;
        }
    }

}