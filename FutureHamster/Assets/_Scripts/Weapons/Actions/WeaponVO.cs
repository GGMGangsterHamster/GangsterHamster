namespace Weapons.Actions
{
    [System.Serializable]
    public class WeaponVO
    {
        public int Shot;
        public int Use;
        public int Reset;
        public int ChangeToInercio;
        public int ChangeToGrand;
        public int ChangeToGravito;

        public WeaponVO(int Shot, int Use, int Reset,
                        int ChangeToInercio, int ChangeToGrand, int ChangeToGravito)
        {
            this.Shot = Shot;
            this.Use = Use;
            this.Reset = Reset;
            this.ChangeToInercio = ChangeToInercio;
            this.ChangeToGrand = ChangeToGrand;
            this.ChangeToGravito = ChangeToGravito;
        }
    }
}
