namespace Setting.VO
{
    [System.Serializable]
    public class GraphicVO
    {
        public int graphicQuality;
        public int shadow;
        public float gamma;
        public int bloom;
        public int lighting;
        public int motionBlur;

        public GraphicVO(int graphicQuality, int shadow, float gamma, int bloom, int lighting, int motionBlur)
        {
            this.graphicQuality = graphicQuality;
            this.shadow = shadow;
            this.gamma = gamma;
            this.bloom = bloom;
            this.lighting = lighting;
            this.motionBlur = motionBlur;
        }
    }
}
