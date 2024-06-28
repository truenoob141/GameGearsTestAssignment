using TN141.Combat.Models;

namespace TN141.Combat.Factories
{
    public class StatFactory
    {
        private const string iconPath = "Icons/";
        
        public StatModel Create(Stat data)
        {
            return new StatModel(
                data.id,
                data.title,
                data.value,
                iconPath + data.icon);
        }

        public StatModel[] Create(Stat[] data)
        {
            var models = new StatModel[data.Length];
            for (int i = 0; i < models.Length; ++i)
                models[i] = Create(data[i]);

            return models;
        }
    }
}