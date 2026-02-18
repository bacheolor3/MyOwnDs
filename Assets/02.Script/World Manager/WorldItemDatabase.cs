using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace TSG
{
    public class WorldItemDatabase : MonoBehaviour
    {
        public static WorldItemDatabase Instance;

        public WeaponItem unarmedWeapon;

        [Header("무기들")]
        [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

        // 게임 내 존재하는 모든 아이템의 리스트
        [Header("아이템들")]
        private List<Item> items = new List<Item>();

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            // 아이템 리스트에 내가 가진 모든 무기 다 포함
            foreach(var weapon in weapons)
            {
                items.Add(weapon);
            }

            // 모든 아이템에 각각 고유한 ID 부여
            for(int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }
        }

        public WeaponItem GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
        }
    }    
}
