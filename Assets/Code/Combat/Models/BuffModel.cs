using System.Collections.Generic;

namespace TN141.Combat.Models
{
    public class BuffModel
    {
        public int Id { get; }
        public string Title { get; }
        public string Icon { get; }
        public Dictionary<int, float> StatValues { get; }

        public BuffModel(int id, string title, string icon, Dictionary<int, float> statValues)
        {
            Id = id;
            Title = title;
            Icon = icon;
            StatValues = statValues;
        }
    }
}