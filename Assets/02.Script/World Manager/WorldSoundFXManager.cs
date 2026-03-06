using UnityEngine;

namespace TSG
{
    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager instance;

        [Header("데미지 사운드")]
        public AudioClip[] PhysicalDamageSFX;

        [Header("액션 사운드")]
        public AudioClip rollSFX;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    
        public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
        {
            int index = Random.Range(0, array.Length);

            return array[index];
        }
    }
}
