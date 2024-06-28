using System.Collections.Generic;
using System.Linq;

namespace TN141.Combat.Models
{
    public class PlayerModel
    {
        public int PlayerId { get; }

        private readonly Dictionary<int, StatModel> _stats;
        private readonly Dictionary<int, BuffModel> _buffs;

        public PlayerModel(int playerId, StatModel[] stats, BuffModel[] buffs)
        {
            PlayerId = playerId;

            _stats = new Dictionary<int, StatModel>(stats.Length);
            foreach (var stat in stats)
                _stats.Add(stat.Id, stat);

            _buffs = new Dictionary<int, BuffModel>(buffs.Length);
            foreach (var buff in buffs)
                _buffs.Add(buff.Id, buff);
        }

        public float GetStatFinalValue(int statId)
        {
            if (!_stats.TryGetValue(statId, out var stat))
                return 0;

            return stat.Value +
                _buffs.Values.SelectMany(b => b.StatValues)
                    .Where(kv => kv.Key == statId)
                    .Sum(kv => kv.Value);
        }
        
        public float GetStatFinalSourceValue(int statId)
        {
            if (!_stats.TryGetValue(statId, out var stat))
                return 0;

            return stat.SourceValue +
                _buffs.Values.SelectMany(b => b.StatValues)
                    .Where(kv => kv.Key == statId)
                    .Sum(kv => kv.Value);
        }
        
        public float GetStatValue(int statId)
        {
            if (!_stats.TryGetValue(statId, out var stat))
                return 0;

            return stat.Value;
        }

        public void SetStatValue(int statId, float value)
        {
            if (!_stats.TryGetValue(statId, out var stat))
                return;

            stat.Value = value;
        }

        public IEnumerable<StatModel> GetStats()
        {
            return _stats.Values;
        }
        
        public IEnumerable<BuffModel> GetBuffs()
        {
            return _buffs.Values;
        }
    }
}