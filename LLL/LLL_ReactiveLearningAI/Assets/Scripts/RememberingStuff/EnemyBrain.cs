using System.Collections.Generic;

namespace RememberingStuff
{
    public class EnemyBrain
    {
        Dictionary<ERememberanceType, int> simpleSmallBrain = new();

        public void AddMemoryCategory(ERememberanceType _type, int _value)
        {
            simpleSmallBrain.Add(_type, _value);
        }

        public void RemoveMemoryCategory(ERememberanceType _type)
        {
            simpleSmallBrain.Remove(_type);
        }

        public void UpdateMemoryCategory(ERememberanceType _type, int _value)
        {
            simpleSmallBrain[_type]  = _value;
    }

        public bool GetMemoryCategory(ERememberanceType _type, out int _value)
        {
            return  simpleSmallBrain.TryGetValue(_type, out _value);
        }
    }
}