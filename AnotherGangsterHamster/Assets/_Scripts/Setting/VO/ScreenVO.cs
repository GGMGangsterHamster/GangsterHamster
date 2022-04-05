namespace Setting.VO
{
    [System.Serializable]
    public class ScreenVO
    {
        public bool isFullScreen;
        public int width;
        public int heigth;

        public ScreenVO(bool isFullScreen, int width, int heigth)
        {
            this.isFullScreen = isFullScreen;
            this.width = width;
            this.heigth = heigth;
        }
    }

}