using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TN141.Combat.UI
{
    [RequireComponent(typeof(Button))]
    public class NewGameButtonWidget : MonoBehaviour
    {
        [Inject]
        private readonly CombatController _combatController;
        
        [SerializeField]
        private bool _allowBuffs;

        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClickHandler);
        }
        
        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClickHandler);
        }
        
        private void OnClickHandler()
        {
            _combatController.NewGame(_allowBuffs);
        }
    }
}