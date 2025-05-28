using UnityEngine;
using System.Collections;
using System.Linq;
using Meta.XR.MRUtilityKit;

public class BottlePlacementController : MonoBehaviour
{
    public GameObject potionPrefab;
    public GameObject placementMarker;
    public TMPro.TextMeshProUGUI debugText;

    private GameObject potionInstance;
    private GameObject markerInstance;
    private MRUKAnchor anchor;

    private bool anchorPlaced = false;

    void Start()
    {
        if (!MRUK.Instance)
        {
            Log("MRUK instance not found.");
            return;
        }

        StartCoroutine(WaitForRoomAndPlace());
    }

    IEnumerator WaitForRoomAndPlace()
    {
        Log("Scanning environment...");

        // Wait for MRUK to be ready
        while (!MRUK.Instance.IsInitialized)
            yield return null;

        Log("Room scan complete. Looking for a table...");

        // Find a horizontal plane classified as table
        var anchors = MRUK.Instance.GetCurrentRoom().Anchors;
        var tableAnchor = anchors
            .Where(a => a.Label.HasFlag(MRUKAnchor.SceneLabels.TABLE))
            .OrderByDescending(a => a.PlaneRect?.size.magnitude ?? 0f)
            .FirstOrDefault();

        if (tableAnchor == null)
        {
            Log("No table found.");
            yield break;
        }

        Log($"Found table: {tableAnchor.name}");

        // Place anchor just above the detected surface
        Vector3 pos = tableAnchor.transform.position + tableAnchor.transform.up * 0.01f;
        Quaternion rot = Quaternion.identity; // keep prefab's original rotation

        PlaceAnchor(pos, rot);
    }

    void PlaceAnchor(Vector3 pos, Quaternion rot)
    {
        if (anchorPlaced) return;

        GameObject anchorGO = new GameObject("PotionAnchor");
        anchorGO.transform.SetPositionAndRotation(pos, rot);
        anchor = anchorGO.AddComponent<MRUKAnchor>();

        anchorPlaced = true;
        Log("Anchor placed. Showing marker...");

        if (placementMarker)
        {
            markerInstance = Instantiate(placementMarker, anchor.transform);
            markerInstance.transform.localPosition = Vector3.zero;
        }

        Invoke(nameof(SpawnPotionOverlay), 10f); // potion delay
    }

    void SpawnPotionOverlay()
    {
        if (!anchor) return;

        if (markerInstance) Destroy(markerInstance);

        if (potionPrefab)
        {
            potionInstance = Instantiate(potionPrefab, anchor.transform);
            potionInstance.transform.localPosition = Vector3.zero;
            potionInstance.transform.localRotation = Quaternion.identity;

            var ps = potionInstance.GetComponentInChildren<ParticleSystem>();
            if (ps) ps.Play();

            Log("Potion placed.");
        }
        else
        {
            Log("No potion prefab assigned.");
        }
    }

    void Log(string msg)
    {
        Debug.Log("[MRUKPotionPlacement] " + msg);
        if (debugText) debugText.text = msg;
    }
}
