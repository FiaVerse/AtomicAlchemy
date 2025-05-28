using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [Header("Reward Settings")]
        public GameObject rewardPrefab;
        public Transform rewardSpawnPoint;
       
public void SpawnReward()
        {
            Instantiate(rewardPrefab,
                rewardSpawnPoint ? rewardSpawnPoint.position : Vector3.zero,
                Quaternion.identity);
        }
    
}
