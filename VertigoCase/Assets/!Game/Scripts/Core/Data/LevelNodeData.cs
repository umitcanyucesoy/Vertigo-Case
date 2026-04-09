using System;
using _Game.Scripts.Core.Enums;

namespace _Game.Scripts.Core.Data
{
    [Serializable]
    public struct LevelNodeData
    {
        public int levelNumber;
        public LevelType levelType;
        public LevelState levelState;
    }
}
