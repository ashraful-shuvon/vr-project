using UnityEngine;

/// <summary>
/// Spawns a single interactive target relative to the OVRCameraRig's head position,
/// so it appears at a consistent, reachable spot regardless of where the user is standing.
/// Attach to an empty GameObject in the scene and assign the target prefab + camera rig transform.
/// </summary>
public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private Transform headTransform; // assign OVRCameraRig -> CenterEyeAnchor
    [SerializeField] private Vector3 localOffset = new Vector3(0f, -0.1f, 0.6f); // slightly below eye level, ~0.6m forward

    private GameObject _currentTarget;

    private void Start()
    {
        SpawnTarget();
    }

    public void SpawnTarget()
    {
        if (targetPrefab == null || headTransform == null)
        {
            Debug.LogWarning("TargetSpawner: assign targetPrefab and headTransform in the Inspector.");
            return;
        }

        if (_currentTarget != null)
        {
            Destroy(_currentTarget);
        }

        Vector3 spawnPos = headTransform.position
                            + headTransform.forward * localOffset.z
                            + headTransform.up * localOffset.y
                            + headTransform.right * localOffset.x;

        _currentTarget = Instantiate(targetPrefab, spawnPos, Quaternion.identity);
    }
}