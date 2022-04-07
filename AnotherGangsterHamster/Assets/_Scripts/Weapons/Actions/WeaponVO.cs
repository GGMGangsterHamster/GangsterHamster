namespace Weapons.Actions
{
    [System.Serializable]
    public class WeaponVO
    {
        public int Shot;
        public int Activate;
        public int Reset;
        public int ChangeToInercio;
        public int ChangeToGrand;
        public int ChangeToGravito;

        public WeaponVO(int Shot, int Activate, int Reset,
                        int ChangeToInercio, int ChangeToGrand, int ChangeToGravito)
        {
            this.Shot = Shot;
            this.Activate = Activate;
            this.Reset = Reset;
            this.ChangeToInercio = ChangeToInercio;
            this.ChangeToGrand = ChangeToGrand;
            this.ChangeToGravito = ChangeToGravito;
        }
    }
}
