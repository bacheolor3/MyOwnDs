using UnityEngine;

namespace TSG
{
    public class WeaponModelInstantiationSlot : MonoBehaviour
    {
        // 어디 쪽 슬롯에 있는지?(왼손인지 오른손인지, 아니면 엉덩이 혹은 뒤인지)
        public WeaponModelSlot weaponSlot;
        public GameObject currentWeaponModel;

        public void UnloadWeapon()
        {
            if(currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        public void LoadWeapon(GameObject weaponModel)
        {
            UnloadWeapon();

            if(weaponModel == null)
            {
                return;
            }

            currentWeaponModel = Instantiate(weaponModel);
            // currentWeaponModel = weaponModel;
            if(currentWeaponModel != null)
            {
                currentWeaponModel.transform.parent = transform;

                currentWeaponModel.transform.localPosition = Vector3.zero;
                currentWeaponModel.transform.localRotation = Quaternion.identity;
                currentWeaponModel.transform.localScale = Vector3.one;                
            }
        }
    }    
}
