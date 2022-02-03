///<summary>
///데미지 받는 오브젝트가 구현해야 하는 인터페이스
///</summary>

namespace Obejcts.Damageable
{

    interface IDamageable<T> // HP 가 꼭 int 가 아닐수도 있을 거 같아서
    {
        public T HP { get; }
        public void OnDamage(T damage);
        public void OnDead(System.Action callback = null);
    }

}