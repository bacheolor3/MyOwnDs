using UnityEngine;

namespace TSG
{    
    public class Enums : MonoBehaviour
    {
        
    }

    public enum CharacterSlot
    {
        CharacterSlot_01,
        CharacterSlot_02,
        CharacterSlot_03,
        CharacterSlot_04,
        CharacterSlot_05,
        CharacterSlot_06,
        CharacterSlot_07,
        CharacterSlot_08,
        CharacterSlot_09,
        CharacterSlot_10,
        
        NO_SLOT
    }

    public enum WeaponModelSlot
    {
        RightHand,
        LeftHand,
        // Right Hips
        // Left Hips
        // Back
    }

    // 공격 타입에 따른 데미지 계산을 위한 것
    public enum AttackType
    {
        LightAttack01,
        LightAttack02,
        HeavyAttack01,
        HeavyAttack02,
        ChargedAttack01,
        ChargedAttack02
    }
}
