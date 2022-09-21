namespace Setting.VO
{
    [System.Serializable]
    public class MouseVO
    {
        public float sensitivity;

        public MouseVO(float sensitivity)
        {
            this.sensitivity = sensitivity;
        }
    }
}