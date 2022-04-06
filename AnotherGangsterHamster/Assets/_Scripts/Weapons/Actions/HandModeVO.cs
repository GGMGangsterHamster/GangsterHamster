namespace Weapons.Actions
{
    [System.Serializable]
    public class HandModeVO
    {
        public bool isRightHand;

        public HandModeVO(bool isRightHand)
        {
            this.isRightHand = isRightHand;
        }
    }
}