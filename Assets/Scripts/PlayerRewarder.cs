using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRewarder : MonoBehaviour
{
    [SerializeField] Collider rewardArea;
    [SerializeField] List<PowerUp> powerUpPrefabs;


    public void SpawnPowerUpForPlayer(Players playerToReward)
    {
        Vector3 randomSpawnPosition = GetRandomSpawnPosition();
        PowerUp powerUpToSpawn = GetRandomPowerUp();
        PowerUp spawnedPowerUp = Instantiate(powerUpToSpawn, randomSpawnPosition, powerUpToSpawn.transform.rotation, transform.parent);
        randomSpawnPosition.y += spawnedPowerUp.PowerUpBoundExtents.y;
        spawnedPowerUp.transform.position = randomSpawnPosition;

    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomizedExtents = rewardArea.bounds.extents;
        randomizedExtents.x *= Random.Range(-0.75f, 0.75f);
        randomizedExtents.z *= Random.Range(-0.75f, 0.75f);
        return rewardArea.bounds.center + randomizedExtents;
    }

    private PowerUp GetRandomPowerUp()
    {
        return powerUpPrefabs[Random.Range(0, powerUpPrefabs.Count)];
    }
}
