
using UnityEngine;

public class SpawnManagerScript : MonoBehaviour
{
    public GameObject portalPrefab;
    public Transform playerCameraTransform; 
    public Vector3 offsetFromPlayer = new Vector3(0f, -0.5f, 2f); // x, y, z offset

    void Start()
    {
        SpawnPortal();
    }
    
    private void SpawnPortal()
    {
        // simple spawn not using MRUK

        // Calculate position based on player's forward direction and offset
        Vector3 spawnPosition =
            playerCameraTransform.position + playerCameraTransform.TransformDirection(offsetFromPlayer);
        Quaternion
            spawnRotation =
                Quaternion.LookRotation(playerCameraTransform.forward); // Makes portal face away from player initially
        // Or: Quaternion spawnRotation = portalPrefab.transform.rotation; // Keep prefab's default rotation

        Instantiate(portalPrefab, spawnPosition, spawnRotation);

    }

}