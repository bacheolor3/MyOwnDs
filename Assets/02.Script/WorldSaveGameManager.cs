using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TSG
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;
        [SerializeField] int worldSceneIndex = 1;

        private void Awake()
        {
            // 하나의 Instance만 가질 수 있음, 만약 다른게 있다면 파괴할것
            if (instance == null)
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

        }
        
        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            yield return null;
        }
    }
    
}
