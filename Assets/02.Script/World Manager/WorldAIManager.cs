using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace TSG
{
    public class WorldAIManager : MonoBehaviour
    {
        public static WorldAIManager instance;

        [Header("디버그")]
        [SerializeField] bool despawnCharacters = false;
        [SerializeField] bool respawnCharacters = false;

        [Header("캐릭터들")]
        [SerializeField] GameObject[] aiCharacters;
        [SerializeField] List<GameObject> spawnedInCharacters;

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
            if (NetworkManager.Singleton.IsServer)
            {
                // 씬에 있는 모든 AI 다 소환하기
                StartCoroutine(WaitForSceneToLoadThenSpawnCharacters());
            }
        }

        private void Update()
        {
            if (respawnCharacters)
            {
                respawnCharacters = false;
                SpawnAllCharacters();
            }

            if (despawnCharacters)
            {
                despawnCharacters = false;
                DespawnAllCharacters();
            }
        }

        private IEnumerator WaitForSceneToLoadThenSpawnCharacters()
        {
            while (!SceneManager.GetActiveScene().isLoaded)
            {
                yield return null;
            }

            SpawnAllCharacters();
        }

        private void SpawnAllCharacters()
        {
            foreach(var character in aiCharacters)
            {
                GameObject instantiatiedCharacter = Instantiate(character);
                instantiatiedCharacter.GetComponent<NetworkObject>().Spawn();
                spawnedInCharacters.Add(instantiatiedCharacter);
            }
        }
    
        private void DespawnAllCharacters()
        {
            foreach(var character in spawnedInCharacters)
            {
                character.GetComponent<NetworkObject>().Despawn();
            }
        }
    
        private void DisableAllCharacters()
        {
            // 캐릭터 오브젝트 비활성화, 네트워크 싱크 또한 비활성화 됨
            // 비활성화 상태라면 연결된 클라이언트에게 게임 오브젝트 비활성화
            // 플레이어에게서 멀리 떨어진 캐릭터를 비활성화해서 메모리 최적화에 사용됨
            // 캐릭터는 지역들에 따라 분리될 수 있음(AREA_00_, AREA_01, AREA_02 등)
        }
    }
}