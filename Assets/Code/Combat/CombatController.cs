using System;
using System.Collections.Generic;
using System.Linq;
using TN141.Combat.Factories;
using TN141.Combat.Models;
using TN141.Combat.Widgets;
using TN141.Configs;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace TN141.Combat
{
    public class CombatController : MonoBehaviour
    {
        [Inject]
        private readonly BuffFactory _buffFactory;
        [Inject]
        private readonly ConfigService _configService;
        [Inject]
        private readonly EventDispatcher _eventDispatcher;
        [Inject]
        private readonly StatFactory _statFactory;

        [SerializeField]
        private PlayerWidget[] _playerWidgets;

        private readonly Dictionary<PlayerModel, PlayerWidget> _players = new();

        private void OnEnable()
        {
            _eventDispatcher.Subscribe<OnConfigsLoaded>(OnConfigsLoadedHandler);
            OnConfigsLoadedHandler();
        }

        private void OnDisable()
        {
            _eventDispatcher.Unsubscribe<OnConfigsLoaded>(OnConfigsLoadedHandler);
        }

        public PlayerModel GetPlayer(int playerId)
        {
            return _players.Keys.FirstOrDefault(p => p.PlayerId == playerId);
        }

        public PlayerModel GetEnemyFor(int playerId)
        {
            return _players.Keys.FirstOrDefault(p => p.PlayerId != playerId);
        }

        public void DealDamage(PlayerModel attacker, PlayerModel victim)
        {
            // Attacker is dead ?
            float attackerHealth = attacker.GetStatFinalValue(StatIds.health);
            if (attackerHealth <= 0)
            {
                Debug.Log("Failed to deal damage: Attacker is dead!");
                return;
            }

            float health = victim.GetStatFinalValue(StatIds.health);
            if (health <= 0)
            {
                Debug.Log("Failed to deal damage: Victim is dead!");
                return;
            }

            float damage = attacker.GetStatFinalValue(StatIds.damage);
            float armor = victim.GetStatFinalValue(StatIds.armor) * 0.01f;
            float lifeSteal = attacker.GetStatFinalValue(StatIds.lifesteal) * 0.01f;

            // Apply armor
            damage *= Mathf.Clamp01(1 - armor);

            // Clamp damage
            if (damage > health)
                damage = health;

            // Apply lifesteal
            float ls = Mathf.Max(0, damage * lifeSteal);
            attacker.SetStatValue(StatIds.health, attacker.GetStatValue(StatIds.health) + ls);

            // Apply damage
            victim.SetStatValue(StatIds.health, victim.GetStatValue(StatIds.health) - damage);

            // Log
            Debug.Log($"Player {attacker.PlayerId} deal {damage} damage to {victim.PlayerId} (lifesteal {ls})");

            // Trigger events
            // Damage
            var attackerWidget = _players[attacker];
            attackerWidget.Attack();

            // Take damage or Die
            var victimWidget = _players[victim];
            victimWidget.SetHealth(
                (int) victim.GetStatFinalValue(StatIds.health),
                (int) victim.GetStatFinalSourceValue(StatIds.health));

            // Trigger event
            _eventDispatcher.Trigger<OnStatsUpdated>();
        }

        public void NewGame(bool allowBuffs = false)
        {
            _players.Clear();

            var buffSettings = _configService.GetBuffSettings();
            var buffs = _configService.GetBuffs();

            var source = _configService.GetPlayerStats();
            for (int i = 0; i < _playerWidgets.Length; ++i)
            {
                Buff[] randomBuffs;
                if (allowBuffs)
                {
                    int n = Random.Range(buffSettings.buffCountMin, buffSettings.buffCountMax + 1);
                    if (buffSettings.allowDuplicateBuffs)
                    {
                        randomBuffs = new Buff[n];
                        for (int j = 0; j < randomBuffs.Length; ++j)
                            randomBuffs[j] = buffs[Random.Range(0, buffs.Length)];
                    }
                    else
                    {
                        randomBuffs = buffs.OrderBy(_ => Random.value)
                            .Take(n)
                            .ToArray();
                    }
                }
                else
                    randomBuffs = Array.Empty<Buff>();

                var widget = _playerWidgets[i];
                var stats = _statFactory.Create(source);
                var buffModels = _buffFactory.Create(randomBuffs);
                _players.Add(new PlayerModel(i, stats, buffModels), widget);

                widget.Respawn();
            }

            _eventDispatcher.Trigger<OnCombatStart>();
        }

        private void OnConfigsLoadedHandler()
        {
            if (!_configService.IsLoaded)
                return;

            NewGame();
        }
    }
}