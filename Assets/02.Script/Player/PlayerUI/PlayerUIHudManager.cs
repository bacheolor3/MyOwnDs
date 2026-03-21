using UnityEngine;
using UnityEngine.UI;

namespace TSG
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [Header("스탯 바")]
        [SerializeField] UI_StatBar healthBar;
        [SerializeField] UI_StatBar staminaBar;
        
        [Header("퀵슬롯들")]
        [SerializeField] Image rightWeaponQuickSlotIcon;
        [SerializeField] Image leftWeaponQuickSlotIcon;
        public void RefreshHUD()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }

        public void SetNewHealthValue(int oldValue, int newValue)
        {
            healthBar.SetStat(newValue);
        }

        public void SetMaxHealthValue(int maxhealth)
        {
            healthBar.SetMaxStat(maxhealth);
        }

        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }
    
        public void SetRightWeaponQuickSlotIcon(int weaponID)
        {
            // 방식 1. 플레이어 손에 들린 무기를 '직접 참조'하는 방식
            // 장점: 매우 직관적이고 구조가 단순함.
            // 단점: 무기가 먼저 로드된 '후'에 이 함수를 호출해야 함. 순서가 틀리면 에러 발생.
            // 예: 세이브 파일을 불러올 때, UI가 무기 정보를 참조하려 하지만 정작 무기 객체는 아직 생성(Instantiate)되지 않았을 수 있음.
            // 결론: 실행 순서(Order of operations) 제어에 자신 있다면 이 방법도 충분히 좋음.
            
            // 이 방법을 쓸 거임
            // 방식 2. 무기의 '아이템 ID'를 이용해 데이터베이스에서 정보를 가져오는 방식
            // 장점: 항상 무기 ID를 저장하고 있으므로, 무기 객체가 생성될 때까지 기다릴 필요 없이 즉시 정보를 가져올 수 있음.
            // 단점: 방식 1에 비해 참조 과정이 한 단계 더 필요함 (비직관적).
            // 결론: 로딩 순서나 실행 타이밍을 신경 쓰고 싶지 않다면 이 방식이 더 안정적이고 합리적임.

            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);

            if(weapon == null)
            {
                Debug.Log("아이템이 없음");
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }

            if(weapon.itemIcon == null)
            {
                Debug.Log("아이템에 아이콘 없음");
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }

            // 아이템의 요구치를 체크하고 쓸 수 없으면 경고를 UI에 띄울지 말지 체크하는 곳

            rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            rightWeaponQuickSlotIcon.enabled = true;
        }

        public void SetLeftWeaponQuickSlotIcon(int weaponID)
        {
            // 방식 1. 플레이어 손에 들린 무기를 '직접 참조'하는 방식
            // 장점: 매우 직관적이고 구조가 단순함.
            // 단점: 무기가 먼저 로드된 '후'에 이 함수를 호출해야 함. 순서가 틀리면 에러 발생.
            // 예: 세이브 파일을 불러올 때, UI가 무기 정보를 참조하려 하지만 정작 무기 객체는 아직 생성(Instantiate)되지 않았을 수 있음.
            // 결론: 실행 순서(Order of operations) 제어에 자신 있다면 이 방법도 충분히 좋음.
            
            // 이 방법을 쓸 거임
            // 방식 2. 무기의 '아이템 ID'를 이용해 데이터베이스에서 정보를 가져오는 방식
            // 장점: 항상 무기 ID를 저장하고 있으므로, 무기 객체가 생성될 때까지 기다릴 필요 없이 즉시 정보를 가져올 수 있음.
            // 단점: 방식 1에 비해 참조 과정이 한 단계 더 필요함 (비직관적).
            // 결론: 로딩 순서나 실행 타이밍을 신경 쓰고 싶지 않다면 이 방식이 더 안정적이고 합리적임.

            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);

            if(weapon == null)
            {
                Debug.Log("아이템이 없음");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }

            if(weapon.itemIcon == null)
            {
                Debug.Log("아이템에 아이콘 없음");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }

            // 아이템의 요구치를 체크하고 쓸 수 없으면 경고를 UI에 띄울지 말지 체크하는 곳

            leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            leftWeaponQuickSlotIcon.enabled = true;
        }
    }    
}
