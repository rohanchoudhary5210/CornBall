using UnityEngine;
using System.Collections;

/// <summary>
/// Manages the sandbag's physics, state (thrown, holding), and input handling.
/// Renamed from 'cornrag.cs' for clarity.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SandbagController : MonoBehaviour
{
    // --- Configuration ---
    [Header("Throwing Power")]
    [SerializeField] private float throwForceMultiplier = 0.1f;
    [SerializeField] private float verticalSensitivity = 0.01f;
    [SerializeField] private float horizontalSensitivity = 0.05f;
    [SerializeField] private float minSwipeDistance = 30f;
    [SerializeField] private float maxBallSpeed = 50f;

    [Header("Stability Check")]
    [SerializeField] private float stabilityThreshold = 0.001f;
    [SerializeField] private float stableDuration = 0.5f;

    // --- State ---
    private Rigidbody _rb;
    private Vector3 _resetPosition;
    private Quaternion _resetRotation;
    private bool _isThrown = false;
    private bool _isHolding = false;
    
    // Flags to track scoring conditions
    public bool HasLandedOnBoard { get; set; } = false;
    public bool HasHitGround { get; set; } = false;
    public bool HasScoredInHole { get; set; } = false;

    // --- Swipe Detection ---
    private Vector2 _startPos, _endPos;
    private float _startTime, _endTime;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Store initial position and rotation to reset to
        _resetPosition = transform.position;
        _resetRotation = transform.rotation;
        ResetSandbag();
    }

    void Update()
    {
        // Don't process input if the bag has already been thrown
        if (_isThrown) return;

        // Handle input based on the platform
        #if UNITY_EDITOR || UNITY_STANDALONE
            HandleMouseInput();
        #elif UNITY_ANDROID || UNITY_IOS
            HandleTouchInput();
        #endif
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isHolding = true;
            _startTime = Time.time;
            _startPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) && _isHolding)
        {
            _isHolding = false;
            _endTime = Time.time;
            _endPos = Input.mousePosition;
            ProcessThrow();
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _isHolding = true;
                    _startTime = Time.time;
                    _startPos = touch.position;
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (_isHolding)
                    {
                        _isHolding = false;
                        _endTime = Time.time;
                        _endPos = touch.position;
                        ProcessThrow();
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Processes the swipe input to calculate and apply the throw force.
    /// </summary>
    private void ProcessThrow()
    {
        float swipeDistance = (_endPos - _startPos).magnitude;
        float swipeTime = _endTime - _startTime;

        if (swipeTime > 0 && swipeDistance >= minSwipeDistance)
        {
            // Calculate direction and speed from the swipe
            Vector3 throwDirection = CalculateThrowDirection();
            float ballSpeed = CalculateThrowSpeed(swipeDistance, swipeTime);
            
            // Apply the force
            Vector3 force = throwDirection * ballSpeed;
            _rb.useGravity = true;
            _rb.AddForce(force, ForceMode.Impulse);
            _isThrown = true;
            
            // Start checking when the bag has come to a rest
            StartCoroutine(CheckIfStable());
        }
    }

    private float CalculateThrowSpeed(float distance, float time)
    {
        float swipeVelocity = distance / time;
        float speed = swipeVelocity * throwForceMultiplier;
        return Mathf.Clamp(speed, 5f, maxBallSpeed);
    }

    private Vector3 CalculateThrowDirection()
    {
        Vector3 swipeDirectionScreen = (_endPos - _startPos);
        
        // Horizontal aim based on swipe X
        Quaternion horizontalRotation = Quaternion.AngleAxis(swipeDirectionScreen.x * horizontalSensitivity, Vector3.up);
        Vector3 forwardDirection = Camera.main.transform.forward;
        forwardDirection.y = 0;
        Vector3 finalDirection = horizontalRotation * forwardDirection.normalized;

        // Vertical aim based on swipe Y
        float upwardAngle = swipeDirectionScreen.y * verticalSensitivity;
        upwardAngle = Mathf.Clamp(upwardAngle, 10f, 30f);

        return Quaternion.AngleAxis(-upwardAngle, Camera.main.transform.right) * finalDirection;
    }

    /// <summary>
    /// Resets the sandbag to its initial state and position.
    /// </summary>
    public void ResetSandbag()
    {
        _isThrown = false;
        _isHolding = false;
        HasLandedOnBoard = false;
        HasHitGround = false;
        HasScoredInHole = false;

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.useGravity = false;
        transform.SetPositionAndRotation(_resetPosition, _resetRotation);
    }
    
    /// <summary>
    /// Coroutine to check when the Rigidbody has stopped moving.
    /// </summary>
    private IEnumerator CheckIfStable()
    {
        yield return new WaitForSeconds(1f); // Initial delay to let it fly
        
        float timer = 0f;
        Vector3 lastPos = transform.position;

        while (timer < stableDuration)
        {
            yield return new WaitForSeconds(0.1f);
            float distance = Vector3.Distance(transform.position, lastPos);

            if (distance < stabilityThreshold)
            {
                timer += 0.1f;
            }
            else
            {
                timer = 0f; // Reset timer if it moved
            }
            lastPos = transform.position;
        }

        Debug.Log("Sandbag is stable. Requesting a new one.");
        // Tell the GameManager that the turn is over
        GameManager.Instance.RequestNewSandbag();
        // The current sandbag will just stay where it is.
        // The SpawnManager will create a new one.
    }
}
