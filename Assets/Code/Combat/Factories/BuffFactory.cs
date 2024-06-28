using System.Collections.Generic;
using System.Linq;
using TN141.Combat.Models;

namespace TN141.Combat.Factories
{
    public class BuffFactory
    {
        private const string iconPath = "Icons/";
        
        public BuffModel Create(Buff data)
        {
            return new BuffModel(
                data.id,
                data.title,
                iconPath + data.icon,
                data.stats.ToDictionary(k => k.statId, v => v.value));
        }

        public BuffModel[] Create(Buff[] data)
        {
            var models = new BuffModel[data.Length];
            for (int i = 0; i < models.Length; ++i)
                models[i] = Create(data[i]);

            return models;
        }
    }
}