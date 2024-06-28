using TMPro;
using TN141.Combat.Models;
using UnityEngine;
using UnityEngine.UI;

namespace TN141.Combat.UI
{
    public class StatWidget : MonoBehaviour
    {
        [SerializeField]
        private Image _statIcon;
        [SerializeField]
        private TMP_Text _valueLabel;
        
        public void ApplyStat(PlayerModel player, StatModel stat)
        {
            _statIcon.sprite = Resources.Load<Sprite>(stat.Icon);
            _valueLabel.text = player.GetStatFinalValue(stat.Id).ToString("0.");
        }
        
        public void ApplyStat(PlayerModel player, BuffModel buff)
        {
            _statIcon.sprite = Resources.Load<Sprite>(buff.Icon);
            _valueLabel.text = buff.Title;
        }
    }
}