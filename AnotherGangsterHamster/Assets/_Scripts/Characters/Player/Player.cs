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
            Debug.Log("�׾����!");
            // �״´ٸ� ������ �ؾ� �ұ�?
        }

        // regenreationValue��ŭ �������� ���� �ʰ� �ִٸ� ü�� ����� ������
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