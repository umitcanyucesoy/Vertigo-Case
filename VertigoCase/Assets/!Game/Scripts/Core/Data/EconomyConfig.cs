using UnityEngine;

namespace _Game.Scripts.Core.Economy
{
    [CreateAssetMenu(fileName = "EconomyConfig", menuName = "GameData/EconomyConfig")]
    public class EconomyConfig : ScriptableObject
    {
        [SerializeField] private int startingGold = 500;
        [SerializeField] private int reviveCost = 150;

        public int StartingGold => startingGold;
        public int ReviveCost => reviveCost;
    }
}
