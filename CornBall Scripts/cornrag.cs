using UnityEngine;
using System.Collections;
using TMPro;

public class cornrag : MonoBehaviour
{
    // Swipe detection
    private float startTime, endTime, swipeDistance, swipeTime;
    private Vector2 startPos, endPos;

    // Throwing
    public float MinSwipeDist = 30f;
    public float MaxBallSpeed = 50f;
    private float BallSpeed = 0f;
    private Vector3 throwDirection;

    // Ball state
    private bool thrown = false;
    private bool holding = false;
    private bool isResetting = false;
    private Vector3 newPosition;
    private Vector3 resetPos;
    private Quaternion resetRot;

    public Rigidbody rb;
    public TextMeshProUGUI text;

    private float count = 0;
    public bool touchedCornHole = false;
    public bool touchedPointer = false;
    public bool hasScored = false;

    void Start()
    {
        resetPos = transform.position;
        resetRot = transform.rotation;
        ResetBall();
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (!thrown) HandleMouseInput();
#elif UNITY_ANDROID || UNITY_IOS
        if (!thrown) HandleTouchInput();
#endif
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTime = Time.time;
            startPos = Input.mousePosition;
            holding = true;
        }

        if (Input.GetMouseButton(0) && holding)
        {
            PickupBall(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0) && holding)
        {
            endTime = Time.time;
            endPos = Input.mousePosition;
            holding = false;
            HandleRelease();
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTime = Time.time;
                    startPos = touch.position;
                    holding = true;
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (holding)
                        PickupBall(touch.position);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (holding)
                    {
                        endTime = Time.time;
                        endPos = touch.position;
                        holding = false;
                        HandleRelease();
                    }
                    break;
            }
        }
    }

    void PickupBall(Vector2 inputPos)
    {
        if (thrown) return;

        Vector3 screenPos = new Vector3(inputPos.x, inputPos.y, Camera.main.nearClipPlane + 5f);
        newPosition = Camera.main.ScreenToWorldPoint(screenPos);
        transform.position = Vector3.Lerp(transform.position, newPosition, 15f * Time.deltaTime);
    }

    void HandleRelease()
    {
        swipeDistance = (endPos - startPos).magnitude;
        swipeTime = endTime - startTime;

        if (swipeTime > 0 && swipeDistance >= MinSwipeDist)
        {
            StartCoroutine(DelayedThrow());
        }
    }

    IEnumerator DelayedThrow()
    {
        yield return new WaitForEndOfFrame();

        CalculateSpeed();
        CalculateDirection();

        BallSpeed = Mathf.Clamp(BallSpeed, 5f, MaxBallSpeed);
        Vector3 force = throwDirection * BallSpeed;
        rb.AddForce(force, ForceMode.Impulse);

        rb.useGravity = true;
        thrown = true;
    }

    void CalculateDirection()
    {
        Vector2 swipe = endPos - startPos;

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0f;
        forward.Normalize();

        float swipeX = swipe.x / Screen.width;
        float swipeY = swipe.y / Screen.height;

        throwDirection = (forward + Vector3.up * swipeY + right * swipeX).normalized;

        Vector3 horizontal = new Vector3(throwDirection.x, 0f, throwDirection.z);
        float angle = Vector3.Angle(throwDirection, horizontal);

        float maxAngle = Random.Range(21f, 22f);
        if (angle > maxAngle)
        {
            float clampedYRatio = Mathf.Tan(maxAngle * Mathf.Deg2Rad);
            float horizontalMagnitude = horizontal.magnitude;
            float maxY = horizontalMagnitude * clampedYRatio;

            throwDirection = new Vector3(throwDirection.x, maxY, throwDirection.z).normalized;
        }

        Debug.DrawRay(transform.position, throwDirection * 5f, Color.red, 2f);
    }

    void CalculateSpeed()
    {
        float swipeRatio = Mathf.Clamp01(swipeDistance / Screen.height);
        BallSpeed = swipeRatio * MaxBallSpeed;
    }

    public void ResetBall()
    {
        startTime = endTime = swipeTime = swipeDistance = 0f;
        throwDirection = Vector3.zero;
        startPos = endPos = Vector2.zero;
        BallSpeed = 0f;
        holding = false;
        thrown = false;
        isResetting = false;
        hasScored = false;
        touchedCornHole = false;
        touchedPointer = false;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        transform.position = resetPos;
        transform.rotation = resetRot;
        collisions.instance.hasCollided = false;
        groundhit.instance.onGroundHit = false;
        Pointer.instance.hasCornHole = false;
    }
}
