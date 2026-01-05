using UnityEngine;
using UnityEngine.UI;

namespace TSG
{
    public class UI_StatBar : MonoBehaviour
    {
        private Slider slider;
        // 스탯을 나타내는 바 사이즈에 따라 표현도 달라지게 하기(큰 스탯 = 더 길어지는 바)
        // 스탯 뒤에 나오는 바는 현상태를 나타낸다 (얼마나 움직일 수 있나/얼마나 데미지를 받을 수 있나를 표현)
        
        protected virtual void Awake()
        {
            if(slider == null)
            {
                slider = GetComponent<Slider>();
            }

        }
        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }
    }
    
}
