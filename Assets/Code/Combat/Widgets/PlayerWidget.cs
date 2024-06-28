using UnityEngine;
using UnityEngine.UI;

namespace TN141.Combat.Widgets
{
    public class PlayerWidget : MonoBehaviour
    {
        private static readonly int attack = Animator.StringToHash("Attack");
        private static readonly int health = Animator.StringToHash("Health");

        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private Slider _healthBar;

        public void Respawn()
        {
            _healthBar.value = 1;
            _animator.SetInteger(health, int.MaxValue);
        }

        public void Attack()
        {
            _animator.SetTrigger(attack);
        }

        public void SetHealth(int value, int maxValue)
        {
            _healthBar.value = Mathf.Clamp01((float) value / maxValue);
            _animator.SetInteger(health, value);
        }
    }
}