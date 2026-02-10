using UnityEngine;

namespace TSG
{
    public class Item : ScriptableObject
    {
        [Header("아이템 정보")]
        public string itemName;
        public Sprite itemIcon;
        [TextArea] public string itemDescription;
        public int itemID;
    }
}
