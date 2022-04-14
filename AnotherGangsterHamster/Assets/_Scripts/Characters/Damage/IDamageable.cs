namespace Characters.Damage
{
   public interface IDamageable
   {

      /// <summary>
      /// 데미지 처리
      /// </summary>
      /// <param name="damage">입힐 데미지</param>
      public void Damage(int damage);
   }
}