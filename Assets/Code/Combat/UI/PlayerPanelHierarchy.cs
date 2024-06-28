using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TN141.Combat.UI
{
    public class PlayerPanelHierarchy : MonoBehaviour
    {
        [Inject]
        private readonly CombatController _combatController;
        [Inject]
        private readonly EventDispatcher _eventDispatcher;

        [SerializeField]
        private int _playerId = -1;
        [SerializeField]
        private Transform _statsPanel;
        [SerializeField]
        private Button _attackButton;
        [SerializeField]
        private StatWidget _itemPrefab;

        private readonly List<StatWidget> _statWidgets = new();
        
        private void Awake()
        {
            _statWidgets.Add(_itemPrefab);
        }

        private void OnEnable()
        {
            _attackButton.onClick.AddListener(OnClickHandler);
            
            _eventDispatcher.Subscribe<OnCombatStart>(OnCombatStartHandler);
            _eventDispatcher.Subscribe<OnStatsUpdated>(OnStatsUpdatedHandler);
            OnCombatStartHandler();
        }
        
        private void OnDisable()
        {
            _attackButton.onClick.RemoveListener(OnClickHandler);
            
            _eventDispatcher.Unsubscribe<OnCombatStart>(OnCombatStartHandler);
            _eventDispatcher.Unsubscribe<OnStatsUpdated>(OnStatsUpdatedHandler);
        }

        private void OnClickHandler()
        {
            var player = _combatController.GetPlayer(_playerId);
            var enemy = _combatController.GetEnemyFor(_playerId);
            if (player == null || enemy == null)
                return;

            _combatController.DealDamage(player, enemy);
        }

        private void OnCombatStartHandler()
        {
            var player = _combatController.GetPlayer(_playerId);
            if (player == null)
            {
                _statWidgets.ForEach(w => w.gameObject.SetActive(false));
                return;
            }

            // TODO Move to pool
            var stats = player.GetStats().ToArray();
            var parent = _itemPrefab.transform.parent;
            for (int i = 0; i < stats.Length; ++i)
            {
                var widget = GetOrCreateWidget(i, parent);
                widget.ApplyStat(player, stats[i]);
            }

            var buffs = player.GetBuffs().ToArray();
            for (int i = 0; i < buffs.Length; ++i)
            {
                var widget = GetOrCreateWidget(stats.Length + i, parent);
                widget.ApplyStat(player, buffs[i]);
            }

            for (int i = stats.Length + buffs.Length; i < _statWidgets.Count; ++i)
                _statWidgets[i].gameObject.SetActive(false);
        }
        
        private void OnStatsUpdatedHandler()
        {
            var player = _combatController.GetPlayer(_playerId);
            if (player == null)
                return;

            var stats = player.GetStats().ToArray();
            for (int i = 0; i < stats.Length; ++i)
                _statWidgets[i].ApplyStat(player, stats[i]);
        }

        private StatWidget GetOrCreateWidget(int index, Transform parent)
        {
            StatWidget widget;
            if (index < _statWidgets.Count)
                widget = _statWidgets[index];
            else
            {
                widget = Instantiate(_itemPrefab, parent);
                _statWidgets.Add(widget);
            }

            widget.gameObject.SetActive(true);
            return widget;
        }
    }
}
