using UnityEngine;

namespace Characters.Player
{

    public class Player : CharacterBase
    {
        public float regenerationDelay;
        public int regenerationValue;

        private float _currentRegenerationTime;

        public override void Damage(int damage)
        {
            _hp -= damage;
            _currentRegenerationTime = regenerationDelay;

            if (_hp <= 0)
            {
                Dead();
            }
        }

        protected override void Dead()
        {
            Debug.Log("Deaddddddddd");
            // 죽는다면 무엇을 해야 할까?
        }

        private void Update()
        {
            if(_currentRegenerationTime <= 0)
            {
                if(_hp < _maxHp)
                {
                    _hp += regenerationValue;
                }
            }
            else
            {
                _currentRegenerationTime -= Time.deltaTime;
            }
        }
    }
}