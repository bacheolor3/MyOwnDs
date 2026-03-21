using System.Collections.Generic;
using UnityEngine;

namespace TSG
{
    public class WorldGameSessionManager : MonoBehaviour
    {
        public static WorldGameSessionManager instance;

        [Header("현시점 세션 안에 있는 플레이어들")]
        public List<PlayerManager> players = new List<PlayerManager>();

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

        public void AddPlayerToActivePlayersList(PlayerManager player)
        {
            // 리스트를 확인하고 만약 플레이어가 없다면, 더하기
            if (!players.Contains(player))
            {
                players.Add(player);
                Debug.Log(player + "Added");
            }

            // 빈 슬롯들 확인하고, 비어있는 슬롯 제거하기
            for(int i = players.Count - 1; i > -1; i--)
            {
                if(players[i] == null)
                {
                    players.RemoveAt(i);
                }
            }
        }

        public void RemovePlayerFromActivePlayersList(PlayerManager player)
        {
            // 리스트를 확인하고 만약 리스트에 있는 플레이어면 제거하기
            if (players.Contains(player))
            {
                players.Remove(player);
            }

            // 빈 슬롯들 확인하고, 비어있는 슬롯 제거하기
            for(int i = players.Count - 1; i > -1; i--)
            {
                if(players[i] == null)
                {
                    players.RemoveAt(i);
                }
            }
        }
    }    
}
