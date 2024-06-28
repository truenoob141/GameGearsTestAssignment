using Unity.VisualScripting;

namespace TN141.Combat.Models
{
    public class StatModel
    {
        public int Id { get; }
        public string Title { get; }
        public float Value { get; set; }
        public float SourceValue { get; }
        public string Icon { get; }

        public StatModel(int id, string title, float value, string icon)
        {
            Id = id;
            Title = title;
            Value = value;
            SourceValue = value;
            Icon = icon;
        }
    }
}