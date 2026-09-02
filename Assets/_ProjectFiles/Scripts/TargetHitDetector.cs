using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Attach to the interactive target object (must have a Trigger Collider + Rigidbody set to Kinematic).
/// Detects a hit from either a hand-tracking fingertip collider or a controller-tip collider,
/// both of which should be tagged "Interactor" in the scene.
///
/// Works with Meta XR SDK: put a small trigger collider on the index fingertip bone under
/// OVRSkeleton (for hand tracking) and/or on the controller model's tip transform (for controllers),
/// tag both "Interactor".
/// </summary>
[RequireComponent(typeof(Collider))]
public class TargetHitDetector : MonoBehaviour
{
    [Header("Feedback")]
    [SerializeField] private Color hitColor = Color.green;
    [SerializeField] private float respawnDelay = 0.6f;
    [SerializeField] private AudioClip hitSound;

    [Header("Events")]
    public UnityEvent OnHit; // hook up score counter, UI, etc. later

    private Renderer _renderer;
    private Color _originalColor;
    private AudioSource _audioSource;
    private bool _isOnCooldown;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer != null)
        {
            _originalColor = _renderer.material.color;
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null && hitSound != null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isOnCooldown) return;
        if (!other.CompareTag("Interactor")) return;

        RegisterHit();
    }

    private void RegisterHit()
    {
        _isOnCooldown = true;

        if (_renderer != null)
        {
            _renderer.material.color = hitColor;
        }

        if (_audioSource != null && hitSound != null)
        {
            _audioSource.PlayOneShot(hitSound);
        }

        OnHit?.Invoke();

        Invoke(nameof(ResetTarget), respawnDelay);
    }

    private void ResetTarget()
    {
        if (_renderer != null)
        {
            _renderer.material.color = _originalColor;
        }
        _isOnCooldown = false;
    }
}