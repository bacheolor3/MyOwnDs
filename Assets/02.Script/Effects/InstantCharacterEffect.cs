using UnityEngine;

namespace TSG
{
    public class InstantCharacterEffect : ScriptableObject
    {
        [Header("이펙트 ID")]
        public int instantEffectID;

        public virtual void ProcessEffect(CharacterManager character)
        {
            
        }
    }
    
}
