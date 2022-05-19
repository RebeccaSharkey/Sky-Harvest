using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script which handles all camera movement and input
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private float normalSpeed;
    [SerializeField] private float fasterSpeed;

    [SerializeField] private float lerpTime;

    private Quaternion newRotation;
    [SerializeField] private float rotationAmount;

    private Vector3 newPos;

    [SerializeField] private Transform mainCam;
    private Vector3 dragStartPos;
    private Vector3 dragCurrentPos;

    [SerializeField] private Vector3 zoomAmount;
    private Vector3 newZoom;

    [SerializeField] private int maxZoomAmount;
    private int currentZoomAmount = 0;

    [SerializeField] private float playerZOffset;

    [SerializeField] private Slider panSpeedSlider;

    private float moveTime = 0f;
    [SerializeField] private float timeToMove;

    [Header("House Boundaries")] 
    [SerializeField] private float minHouseBoundsX;
    [SerializeField] private float maxHouseBoundsX;
    [SerializeField] private float minHouseBoundsZ;
    [SerializeField] private float maxHouseBoundsZ;

    [Header("First Islands Boundaries")]
    [SerializeField] private float minFirstIslandsBoundsX;
    [SerializeField] private float maxFirstIslandsBoundsX;
    [SerializeField] private float minFirstIslandsBoundsZ;
    [SerializeField] private float maxFirstIslandsBoundsZ;

    
    [Header("Snowy Islands Boundaries")]
    [SerializeField] private float minSnowyIslandsBoundsX;
    [SerializeField] private float maxSnowyIslandsBoundsX;
    [SerializeField] private float minSnowyIslandsBoundsZ;
    [SerializeField] private float maxSnowyIslandsBoundsZ;

    private bool bIsLerping = false;
    private bool bMovingCam = false;
    private bool camActive = true;

    void Start()
    {
        newPos = transform.position;
        normalSpeed = moveSpeed;
        newRotation = transform.rotation;
        newZoom = mainCam.localPosition;
    }

    private void LateUpdate()
    {
        if(camActive)
        {
            MovementInput();
            MouseInput();
        }
    }

    /// <summary>
    /// Checks for player input and moves the camera accordingly depending on the WSAD keys
    /// Rotates the camera around a parent object depending on the Q and E keys
    /// Destroys any contextual menu if the camera is moving and a menu is open/spawned in
    /// Speeds up camera panning if Left Shift is held down
    /// </summary>
    private void MovementInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = fasterSpeed;
        }
        else
        {
            moveSpeed = normalSpeed;
        }

        //if (CustomEvents.PlayerControls.OnGetInHouse?.Invoke() == false)
        //{
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                CustomEvents.UI.OnDestroyContextualMenu?.Invoke();

                newPos += (transform.forward * moveSpeed);
                CustomEvents.Tutorial.OnCameraMovement?.Invoke(true);
                bMovingCam = true;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                CustomEvents.UI.OnDestroyContextualMenu?.Invoke();

                newPos += (transform.forward * -moveSpeed);
                CustomEvents.Tutorial.OnCameraMovement?.Invoke(true);
                bMovingCam = true;
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                CustomEvents.UI.OnDestroyContextualMenu?.Invoke();

                newPos += (transform.right * -moveSpeed);
                CustomEvents.Tutorial.OnCameraMovement?.Invoke(true);
                bMovingCam = true;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                CustomEvents.UI.OnDestroyContextualMenu?.Invoke();

                newPos += (transform.right * moveSpeed);
                CustomEvents.Tutorial.OnCameraMovement?.Invoke(true);
                bMovingCam = true;
            }
        //}

        if (Input.GetKey(KeyCode.Q))
        {
            CustomEvents.UI.OnDestroyContextualMenu?.Invoke();

            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
            CustomEvents.Tutorial.OnCameraMovement?.Invoke(true);
            bMovingCam = true;
        }

        if (Input.GetKey(KeyCode.E))
        {
            CustomEvents.UI.OnDestroyContextualMenu?.Invoke();

            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
            CustomEvents.Tutorial.OnCameraMovement?.Invoke(true);
            bMovingCam = true;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            CustomEvents.UI.OnDestroyContextualMenu?.Invoke();

            Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            CentralisePlayer(player.position);
        }

        if (newPos != transform.position)
        {
            transform.position = Vector3.Lerp(transform.position, newPos, lerpTime * Time.deltaTime);
        }

        if (!Input.anyKey && bMovingCam)
        {
            CheckPosition();
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, lerpTime * Time.deltaTime);
        mainCam.localPosition = Vector3.Lerp(mainCam.localPosition, newZoom, lerpTime * Time.deltaTime);
    }

    /// <summary>
    /// Checks the mouse scrollwheel input and zooms the camera in and out depending on the direction
    /// Constrains how many times the camera can zoom in
    /// Destroys any contextual menu if the camera is zooming and a menu is open/spawned in
    /// Fires a ray at the location of the mouse click which intersects an invisible plane
    /// Calculates the distance between initial click and final pos and drags the camera accordingly
    /// </summary>
    private void MouseInput()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f && currentZoomAmount < maxZoomAmount)
        {
            CustomEvents.UI.OnDestroyContextualMenu?.Invoke();

            newZoom += Input.mouseScrollDelta.y * zoomAmount;
            currentZoomAmount++;
            CustomEvents.Tutorial.OnCameraMovement?.Invoke(true);
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f && currentZoomAmount > 0)
        {
            CustomEvents.UI.OnDestroyContextualMenu?.Invoke();

            newZoom += Input.mouseScrollDelta.y * zoomAmount;
            currentZoomAmount--;
            CustomEvents.Tutorial.OnCameraMovement?.Invoke(true);
        }

        if (Input.GetMouseButtonDown(1))
        {
            CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
            bMovingCam = true;
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragStartPos = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPos = ray.GetPoint(entry);
                newPos = transform.position + dragStartPos - dragCurrentPos;
                CustomEvents.Tutorial.OnCameraMovement?.Invoke(true);
            }
        }

        if (!Input.GetMouseButton(1) && bMovingCam)
        {  
            CheckPosition();
        }
    }

    private void CheckPosition()
    {
        PlayerLocation tempLocation = (PlayerLocation) CustomEvents.Camera.OnGetPlayerLocation?.Invoke();

        switch (tempLocation)
        {
            case PlayerLocation.House:
                if (transform.position.x <= minHouseBoundsX)
                {
                    newPos = new Vector3(minHouseBoundsX, transform.position.y, transform.position.z);
                    transform.position = newPos;
                }

                if (transform.position.x >= maxHouseBoundsX)
                {
                    newPos = new Vector3(maxHouseBoundsX, transform.position.y, transform.position.z);
                    transform.position = newPos;
                }

                if (transform.position.z >= minHouseBoundsZ)
                {
                    newPos = new Vector3(transform.position.x, transform.position.y, minHouseBoundsZ);
                    transform.position = newPos;
                }

                if (transform.position.z <= maxHouseBoundsZ)
                {
                    newPos = new Vector3(transform.position.x, transform.position.y, maxHouseBoundsZ);
                    transform.position = newPos;
                }
                break;
            case PlayerLocation.FirstIslands:
                if (transform.position.x <= minFirstIslandsBoundsX)
                {
                    newPos = new Vector3(minFirstIslandsBoundsX, transform.position.y, transform.position.z);
                    transform.position = newPos;
                }
                if (transform.position.x >= maxFirstIslandsBoundsX)
                {
                    newPos = new Vector3(maxFirstIslandsBoundsX, transform.position.y, transform.position.z);
                    transform.position = newPos;
                }

                if (transform.position.z >= minFirstIslandsBoundsZ)
                {
                    newPos = new Vector3(transform.position.x, transform.position.y, minFirstIslandsBoundsZ);
                    transform.position = newPos;
                }
                if (transform.position.z <= maxFirstIslandsBoundsZ)
                {
                    newPos = new Vector3(transform.position.x, transform.position.y, maxFirstIslandsBoundsZ);
                    transform.position = newPos;
                }

                break;
            case PlayerLocation.SnowyIslands:
                if (transform.position.x >= minSnowyIslandsBoundsX)
                {
                    newPos = new Vector3(minSnowyIslandsBoundsX, transform.position.y, transform.position.z);
                    transform.position = newPos;
                }
                if (transform.position.x <= maxSnowyIslandsBoundsX)
                {
                    newPos = new Vector3(maxSnowyIslandsBoundsX, transform.position.y, transform.position.z);
                    transform.position = newPos;
                }
                if (transform.position.z <= minSnowyIslandsBoundsZ)
                {
                    newPos = new Vector3(transform.position.x, transform.position.y, minSnowyIslandsBoundsZ);
                    transform.position = newPos;
                }
                if (transform.position.z >= maxSnowyIslandsBoundsZ)
                {
                    newPos = new Vector3(transform.position.x, transform.position.y, maxSnowyIslandsBoundsZ);
                    transform.position = newPos;
                }
                break;
        }

        bMovingCam = false;
    }

    /// <summary>
    /// Centralises the camera to the centre of the player position
    /// Does not alter the Y position (Height)
    /// Disables the camera controls and re-enables them once position has been changed
    /// </summary>
    /// <param name="_playerPos">Position passed in to centre on</param>
    private void CentralisePlayer(Vector3 _playerPos)
    {
        CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(false);
        newPos = new Vector3(_playerPos.x, transform.position.y, (_playerPos.z + playerZOffset));
        transform.position = new Vector3(_playerPos.x, transform.position.y, (_playerPos.z + playerZOffset));
        CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(true);
    }

    private void StartLerpToPos(Vector3 newPos)
    {
        StartCoroutine(LerpToNewPos(newPos));
    }

    private IEnumerator LerpToNewPos(Vector3 _newPos)
    {
        OnDisableScript(false);
        bIsLerping = true;
        Vector3 initialPos = transform.position;
        while(bIsLerping)
        {
            moveTime += (Time.deltaTime) * (1 / timeToMove);
            transform.position = Vector3.Lerp(initialPos, _newPos, moveTime);
            yield return null;
            if(moveTime >= 1)
            {
                transform.position = _newPos;
                bIsLerping = false;
                moveTime = 0f;
                newPos = transform.position;
                break;
            }
        }
    }

    private bool FinishedLerping()
    {
        if(!bIsLerping)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Increases pan speed
    /// </summary>
    /// <param name="speed">Float passed in that becomes the new pan speed</param>
    public void SetPanSpeed(float speed)
    {
        moveSpeed = speed;
    }

    /// <summary>
    /// Toggles the camera script on or off depending on the bool passed in
    /// </summary>
    /// <param name="state">Bool passed in which determines the state of the script</param>
    private void OnDisableScript(bool state)
    {
        camActive = state;
    }
    private void OnEnable()
    {
        CustomEvents.PlayerControls.OnSnapCamToPlayer += CentralisePlayer;
        CustomEvents.Scripts.OnDisableCameraMovement += OnDisableScript;
        CustomEvents.Camera.OnLerpToNewPos += StartLerpToPos;
        CustomEvents.Camera.OnFinishedLerping += FinishedLerping;
    }

    private void OnDisable()
    {
        CustomEvents.PlayerControls.OnSnapCamToPlayer -= CentralisePlayer;
        CustomEvents.Scripts.OnDisableCameraMovement -= OnDisableScript;
        CustomEvents.Camera.OnLerpToNewPos -= StartLerpToPos;
        CustomEvents.Camera.OnFinishedLerping -= FinishedLerping;
    }
}
