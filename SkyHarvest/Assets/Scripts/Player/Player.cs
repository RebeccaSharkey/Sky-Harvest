using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum PlayerState { Waiting, Moving, Planting, Watering, Fertilising, Harvesting, ClearingPlot, Sleeping, EnteringHouse, Stopped, Buying, UsingInventory, Feeding, AnimalBuying, Fishing, TentBuying, Renaming, Petting, ChangingAppearance };
public enum PlayerLocation { House, FirstIslands, SnowyIslands };

/// <summary>
/// Player monobehaviour, handles player input and movement
/// </summary>
public class Player : MonoBehaviour, iSaveable
{
    [SerializeField] private PlayerState state;
    [SerializeField] private PlayerLocation location;

    private bool isInControl = true;

    [SerializeField] private NavMeshAgent agent;

    private Vector3 hitPoint;

    private GameObject objectHit;

    [SerializeField] private Vector3 teleportOffset;

    [SerializeField] private Camera mainCam;

    private float waitingTimer;
    [SerializeField] private float waitingTimerMax = 0.2f;
    [SerializeField] private Transform insideHouseTeleportPoint, outsideHouseTeleportPoint, spawnPoint;
    [SerializeField] private bool isInHouse = true;
    private bool isWaitingForTransition = false;
    
    [SerializeField] private float completionTimer;
    private float currentTimer;

    [SerializeField] private LayerMask interactableLayer;

    [SerializeField] private GameObject sliderUI;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI secondsDisplay;
    
    private Animator anim;
    [SerializeField] private string animationName;

    private bool bIsSleepingInTent = false;


    private void OnEnable()
    {
        CustomEvents.TaskSystem.OnPlayerMoved += MovePlayer;
        CustomEvents.TaskSystem.OnSetPlayerState += SetState;
        CustomEvents.Camera.OnSetPlayerLocation += SetLocation;
        CustomEvents.Camera.OnGetPlayerLocation += GetLocationState;
        CustomEvents.IslandSystem.Teleportation.OnTeleportPlayer += TeleportPlayer;
        CustomEvents.TimeCycle.OnNewDaySetup += NewDaySetup;
        CustomEvents.UI.OnTransitionFinish += TransitionFinished;
        CustomEvents.TaskSystem.OnPlayerStopMovement += StopPlayer;
        CustomEvents.Scripts.OnDisablePlayer += OnDisablePlayer;
        CustomEvents.PlayerControls.OnChangePlayerRotation += ChangeRotation;
        CustomEvents.PlayerControls.OnSetNewSpawnPos += SetSpawnPos;
        CustomEvents.PlayerControls.OnToggleSliderUI += ToggleStatusBar;
    }

    private void OnDisable()
    {
        CustomEvents.TaskSystem.OnPlayerMoved -= MovePlayer;
        CustomEvents.TaskSystem.OnSetPlayerState -= SetState;
        CustomEvents.Camera.OnSetPlayerLocation -= SetLocation;
        CustomEvents.Camera.OnGetPlayerLocation -= GetLocationState;
        CustomEvents.IslandSystem.Teleportation.OnTeleportPlayer -= TeleportPlayer;
        CustomEvents.TimeCycle.OnNewDaySetup -= NewDaySetup;
        CustomEvents.UI.OnTransitionFinish -= TransitionFinished;
        CustomEvents.TaskSystem.OnPlayerStopMovement -= StopPlayer;
        CustomEvents.PlayerControls.OnChangePlayerRotation -= ChangeRotation;
        CustomEvents.PlayerControls.OnSetNewSpawnPos -= SetSpawnPos;
        CustomEvents.PlayerControls.OnToggleSliderUI -= ToggleStatusBar;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        state = PlayerState.Waiting;
        currentTimer = completionTimer;
        CustomEvents.PlayerControls.OnSnapCamToPlayer?.Invoke(transform.position);
        CustomEvents.Audio.OnPlayThemeSong?.Invoke("First Islands Theme");
    }

    void Update()
    {
        //If the player isn't performing a task then request a new one
        //Checks each state and requests the task with the numbered part of it
        //After completion, changes the player state back to waiting for a task
        switch (state)
        {
            case PlayerState.Waiting:
                anim.SetBool(animationName, false);
                waitingTimer -= Time.deltaTime;
                if (waitingTimer <= 0)
                {
                    waitingTimer = waitingTimerMax;
                    CustomEvents.TaskSystem.OnRequestNewTask?.Invoke(0);
                }
                break;
            case PlayerState.Moving:
                if (!agent.pathPending)
                {
                    anim.SetBool(animationName, true);
                    if (agent.remainingDistance <= 0.1f)
                    {
                        agent.isStopped = true;
                        anim.SetBool(animationName, false);
                        CustomEvents.TaskSystem.OnTaskComplete?.Invoke();
                        state = PlayerState.Waiting;
                        agent.ResetPath();
                    }
                }
                break;
            case PlayerState.Sleeping:
                if (!agent.pathPending)
                {
                    anim.SetBool(animationName, true);
                    if (agent.remainingDistance <= 0.05f)
                    {
                        SetSpawnPos(transform.position);
                        agent.isStopped = true;
                        anim.SetBool(animationName, false);
                        CustomEvents.TaskSystem.OnRequestNewTask?.Invoke(1);
                        CustomEvents.TaskSystem.OnTaskComplete?.Invoke();
                        //state = PlayerState.Stopped;
                        agent.ResetPath();
                    }
                }
                break;
            case PlayerState.Feeding:
            case PlayerState.Petting:
            case PlayerState.Renaming:
                if (!agent.pathPending)
                {
                    anim.SetBool(animationName, true);
                    if (agent.remainingDistance <= 0.6f)
                    {
                        agent.isStopped = true;
                        anim.SetBool(animationName, false);
                        CustomEvents.TaskSystem.OnRequestNewTask?.Invoke(1);
                        CustomEvents.TaskSystem.OnTaskComplete?.Invoke();
                        state = PlayerState.Waiting;
                        agent.ResetPath();
                    }
                }
                break;
            case PlayerState.EnteringHouse:
                if (!agent.pathPending)
                {
                    anim.SetBool(animationName, true);
                    if (agent.remainingDistance <= 0.05f)
                    {
                        agent.isStopped = true;
                        anim.SetBool(animationName, false);
                        EnterExitHouse();
                        //CustomEvents.TaskSystem.OnTaskComplete?.Invoke(); unneeded as entering/exiting house clears task list anyway
                        state = PlayerState.Waiting;
                        agent.ResetPath();
                        CustomEvents.Tutorial.OnDoorPressed?.Invoke(true);
                    }
                }
                break;
            //you can collapse duplicate cases like this
            case PlayerState.Planting:
            case PlayerState.Watering:
            case PlayerState.Fertilising:
            case PlayerState.Harvesting:
            case PlayerState.ClearingPlot:
                if (!agent.pathPending)
                {
                    anim.SetBool(animationName, true);
                    if (agent.remainingDistance <= 0.05f)
                    {
                        anim.SetBool(animationName, false);
                        WaitForTask();
                    }
                }
                break;
            case PlayerState.Buying:
            case PlayerState.UsingInventory:
            case PlayerState.AnimalBuying:
            case PlayerState.Fishing:
            case PlayerState.TentBuying:
            case PlayerState.ChangingAppearance:
                if (!agent.pathPending)
                {
                    anim.SetBool(animationName, true);
                    if (agent.remainingDistance <= 0.5f)
                    {
                        anim.SetBool(animationName, false);
                        ExecuteTaskInstantly();
                    }
                }
                break;
            case PlayerState.Stopped:
                return;
            default:
                Debug.LogError($"Unhandled player state {state}");
                break;
        }

        if (isInControl && Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    private void FixedUpdate()
    {
        HandleOutlineHover();
    }

    private void HandleOutlineHover()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                hit.transform.gameObject.GetComponent<OutlineShaderSwitcher>().SwitchToOutlineShader();
            }
            else
            {
                //Debug.Log("Hit uninteractable");
                CustomEvents.VFX.OnOutlineRemove?.Invoke();
            }
        }
    }

    /// <summary>
    /// Marks rask as finished and requests next task
    /// </summary>
    private void ExecuteTaskInstantly()
    {
        agent.isStopped = true;
        CustomEvents.TaskSystem.OnRequestNewTask?.Invoke(1);
        CustomEvents.TaskSystem.OnTaskComplete?.Invoke();
        state = PlayerState.Waiting;
        agent.ResetPath();
    }

    /// <summary>
    /// Time delay on completing a task to allow for animation
    /// </summary>
    private void WaitForTask()
    {
        if(currentTimer == completionTimer)
        {
            agent.isStopped = true;
            CustomEvents.TaskSystem.OnRequestNewTask?.Invoke(1);
            sliderUI.SetActive(true);
            progressSlider.maxValue = completionTimer;
            progressSlider.value = progressSlider.maxValue;
            secondsDisplay.text = progressSlider.maxValue.ToString("0") + "s";
        }
        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            progressSlider.value = currentTimer;
            secondsDisplay.text = progressSlider.value.ToString("0") + "s";
        }
        else if (currentTimer <= 0)
        {
            CustomEvents.TaskSystem.OnTaskComplete?.Invoke();
            currentTimer = completionTimer;
            state = PlayerState.Waiting;
            agent.ResetPath();
            sliderUI.SetActive(false);
            CustomEvents.Tutorial.OnSeedPlanted?.Invoke(true);
        }
    }

    /// <summary>
    /// Handles clicks, determines what type of object has been clicked on and performs relevant action
    /// </summary>
    private void HandleClick()
    {
        bool paused = false;
        if (CustomEvents.TimeCycle.OnGetPaused?.Invoke() != null)
        {
            paused = (bool)CustomEvents.TimeCycle.OnGetPaused?.Invoke();
        }
        if (!paused)
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (!EventSystem.current.IsPointerOverGameObject())
            {
                CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject hitTarget = hit.transform.gameObject;
                    if (hitTarget)
                    {
                        string hitTag = hitTarget.tag;
                        hitPoint = hit.point;
                        objectHit = hit.transform.gameObject;

                        switch (hitTag)
                        {
                            case "Ground":
                                iExecutable moveToPosTask = new MoveTask(hitPoint);
                                CustomEvents.TaskSystem.OnAddNewTask(moveToPosTask);
                                CustomEvents.Tutorial.OnFloorPressed?.Invoke(true);
                                break;
                            case "Plot":
                                PlotBehaviour selectedPlot = hitTarget.GetComponent<PlotBehaviour>();

                                List<iContextualMenuable> plotMenuElements = new List<iContextualMenuable>();

                                switch (selectedPlot.GetPlotState())
                                {
                                    case PlotState.blocked:
                                        plotMenuElements.Add(new ClearPlotMenuElement(selectedPlot));
                                        break;
                                    case PlotState.empty:
                                        plotMenuElements.Add(new PlantMenuElement(selectedPlot));
                                        break;
                                    case PlotState.unwatered:
                                        plotMenuElements.Add(new WaterMenuElement(selectedPlot));
                                        if (!selectedPlot.GetIsFertilised())
                                        {
                                            plotMenuElements.Add(new FertiliseMenuElement(selectedPlot));
                                        }
                                        break;
                                    case PlotState.watered:
                                        if (!selectedPlot.GetIsFertilised())
                                        {
                                            plotMenuElements.Add(new FertiliseMenuElement(selectedPlot));
                                        }
                                        break;
                                    case PlotState.harvestable:
                                        plotMenuElements.Add(new HarvestMenuElement(selectedPlot));
                                        break;
                                    default:
                                        Debug.LogError("Unhandled plotstate in plot click");
                                        break;
                                }

                                plotMenuElements.Add(new CancelMenuElement());
                                new ContextualMenu(plotMenuElements, Input.mousePosition);
                                break;
                            case "Silo":
                            case "SpecialShop":
                            case "Bag":
                            case "Weekly Task Board":
                            case "CraftingMachine":
                            case "Shop":
                                List<iContextualMenuable> invMenuElements = new List<iContextualMenuable>();
                                invMenuElements.Add(new UseSiloBagShopElement(hitTarget));
                                invMenuElements.Add(new CancelMenuElement());
                                new ContextualMenu(invMenuElements, Input.mousePosition);
                                break;
                            case "Bed":
                                SetIsInTent(false);
                                List<iContextualMenuable> sleepMenuElements = new List<iContextualMenuable>();
                                sleepMenuElements.Add(new SleepMenuElement(hitPoint));
                                sleepMenuElements.Add(new CancelMenuElement());
                                new ContextualMenu(sleepMenuElements, Input.mousePosition);
                                break;
                            case "TentBed":
                                SetIsInTent(true);
                                List<iContextualMenuable> tentMenuElements = new List<iContextualMenuable>();
                                tentMenuElements.Add(new SleepMenuElement(hitPoint));
                                tentMenuElements.Add(new CancelMenuElement());
                                new ContextualMenu(tentMenuElements, Input.mousePosition);
                                break;
                            case "Door":
                                List<iContextualMenuable> enterExitMenuElements = new List<iContextualMenuable>();
                                enterExitMenuElements.Add(new EnterExitMenuElement(hitPoint));
                                enterExitMenuElements.Add(new CancelMenuElement());
                                new ContextualMenu(enterExitMenuElements, Input.mousePosition);
                                break;
                            case "IslandPost":
                                List<iContextualMenuable> buyIslandMenuElements = new List<iContextualMenuable>();
                                buyIslandMenuElements.Add(new BuyIslandElement(hitTarget));
                                buyIslandMenuElements.Add(new CancelMenuElement());
                                new ContextualMenu(buyIslandMenuElements, Input.mousePosition);
                                break;

                            case "AnimalPenPost":
                                List<iContextualMenuable> buyAnimalPenMenuElements = new List<iContextualMenuable>();
                                buyAnimalPenMenuElements.Add(new BuyAnimalPenElement(hitTarget));
                                buyAnimalPenMenuElements.Add(new CancelMenuElement());
                                new ContextualMenu(buyAnimalPenMenuElements, Input.mousePosition);
                                break;
                            case "Animal":
                                List<iContextualMenuable> manageAnimal = new List<iContextualMenuable>();
                                AnimalBehaviour animal;
                                animal = objectHit.GetComponentInChildren<AnimalBehaviour>();
                                manageAnimal.Add(new FeedAnimalElement(animal, hitPoint, "Feed " + animal.AnimalData.itemToFeed.ItemName));
                                manageAnimal.Add(new HarvestAnimalElement(animal, hitPoint));
                                manageAnimal.Add(new PetAnimalElement(animal, hitPoint));
                                manageAnimal.Add(new RenameAnimalElement(animal, hitPoint));
                                manageAnimal.Add(new CancelMenuElement());
                                new ContextualMenu(manageAnimal, Input.mousePosition);
                                break;
                            case "Animal Pen":
                                List<iContextualMenuable> buyAnimal = new List<iContextualMenuable>();
                                AnimalPen pen = objectHit.GetComponent<AnimalPen>();
                                buyAnimal.Add(new BuyAnimalElement(pen, hitPoint, "Buy " + pen.animalType.ToString() +  "(" + pen.buyValue.ToString() + "FR)"));
                                buyAnimal.Add(new CancelMenuElement());
                                new ContextualMenu(buyAnimal, Input.mousePosition);
                                break;
                            case "Water":
                                List<iContextualMenuable> waterMenuElements = new List<iContextualMenuable>();
                                waterMenuElements.Add(new FishElement(hitTarget));
                                waterMenuElements.Add(new CancelMenuElement());
                                new ContextualMenu(waterMenuElements, Input.mousePosition);
                                break;
                            case "IslandTeleporter":
                                List<iContextualMenuable> teleporterElements = new List<iContextualMenuable>();
                                teleporterElements.Add(new UseTeleportPoleElement(hitPoint));
                                teleporterElements.Add(new CancelMenuElement());
                                new ContextualMenu(teleporterElements, Input.mousePosition);
                                break;
                            case "Tent":
                                List<iContextualMenuable> buyTent = new List<iContextualMenuable>();
                                buyTent.Add(new BuyTentElement(hitPoint, hitTarget.GetComponent<TentBuying>()));
                                buyTent.Add(new CancelMenuElement());
                                new ContextualMenu(buyTent, Input.mousePosition);
                                break;
                            case "Wardrobe":
                                List<iContextualMenuable> changeClothes = new List<iContextualMenuable>();
                                changeClothes.Add(new ChangeClothesElement(objectHit));
                                changeClothes.Add(new CancelMenuElement());
                                new ContextualMenu(changeClothes, Input.mousePosition);
                                break;
                            case "FruitTree":
                                FruitTreePlotBehaviour selectedFruitPlot = hitTarget.GetComponent<FruitTreePlotBehaviour>();

                                List<iContextualMenuable> fruitMenuElements = new List<iContextualMenuable>();

                                switch (selectedFruitPlot.GetFruitPlotState())
                                {
                                    case FruitPlotState.Empty:
                                        fruitMenuElements.Add(new PlantTreeElement(selectedFruitPlot, hitTarget));
                                        fruitMenuElements.Add(new CancelMenuElement());
                                        break;
                                    case FruitPlotState.Unwatered:
                                        fruitMenuElements.Add(new WaterTreeElement(selectedFruitPlot, hitTarget));
                                        fruitMenuElements.Add(new CancelMenuElement());
                                        break;
                                    case FruitPlotState.Watered:
                                        fruitMenuElements.Add(new CancelMenuElement());
                                        break;
                                    case FruitPlotState.Harvestable:
                                        fruitMenuElements.Add(new HarvestTreeElement(selectedFruitPlot, hitTarget));
                                        if(!selectedFruitPlot.CheckTreeWithered()) fruitMenuElements.Add(new CutTreeElement(selectedFruitPlot, hitTarget));
                                        fruitMenuElements.Add(new CancelMenuElement());
                                        break;
                                    default:
                                        Debug.LogError("Unhandled plotstate in plot click");
                                        break;
                                }
                                new ContextualMenu(fruitMenuElements, Input.mousePosition);

                                break;
                            case "Aquarium":
                                List<iContextualMenuable> openAquarium = new List<iContextualMenuable>();
                                openAquarium.Add(new OpenAquariumContext(objectHit));
                                openAquarium.Add(new CancelMenuElement());
                                new ContextualMenu(openAquarium, Input.mousePosition);
                                break;
                            default:
                                Debug.LogWarning($"Unhandled tag on click: {hitTag}");
                                break;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Sets new navmesh destination
    /// </summary>
    /// <param name="pos">Target position</param>
    private void MovePlayer(Vector3 pos)
    {
        agent.isStopped = false;
        agent.SetDestination(pos);
    }

    /// <summary>
    /// Stops navmesh from moving
    /// </summary>
    private void StopPlayer()
    {
        agent.isStopped = true;
        transform.position = this.transform.position;
    }

    /// <summary>
    /// Sets player state
    /// </summary>
    /// <param name="_state">New state</param>
    private void SetState(PlayerState _state)
    {
        state = _state;
    }

    private void SetLocation(PlayerLocation _location)
    {
        location = _location;
    }

    private PlayerLocation GetLocationState()
    {
        return location;
    }

    /// <summary>
    /// Handles logic for teleporting the player in/out of the house
    /// </summary>
    private void EnterExitHouse()
    {
        if (isInHouse)
        {
            TeleportPlayer(outsideHouseTeleportPoint.position);
            location = PlayerLocation.FirstIslands;
            CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Door Sound");
        }
        else
        {
            TeleportPlayer(insideHouseTeleportPoint.position);
            location = PlayerLocation.House;
        }

        CustomEvents.TaskSystem.OnClearAllTasks?.Invoke();
        isInHouse ^= true; //XOR shorthand, flips bool
    }

    private void TeleportPlayer(Vector3 _teleportPos)
    {
        StartCoroutine(TeleportPlayerRoutine(_teleportPos));
    }

    /// <summary>
    /// Handles player teleportation with screen fades and timing
    /// </summary>
    /// <param name="_teleportPos">Destination position</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator TeleportPlayerRoutine(Vector3 _teleportPos)
    {
        CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(false);
        isWaitingForTransition = true;
        CustomEvents.UI.OnRandomFadeOut?.Invoke();

        while (isWaitingForTransition) yield return null;

        agent.Warp(_teleportPos + teleportOffset);
        CustomEvents.PlayerControls.OnSnapCamToPlayer?.Invoke(_teleportPos + teleportOffset);

        CustomEvents.UI.OnRandomFadeIn?.Invoke();
        CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(true);
        OnDisablePlayer(true);
    }

    private void ChangeRotation(float angle)
    {
        transform.parent.rotation = Quaternion.Euler(transform.parent.rotation.x, angle, transform.parent.rotation.z);
    }

    private void TransitionFinished()
    {
        isWaitingForTransition = false;
    }

    private void NewDaySetup()
    {
        CustomEvents.TimeCycle.OnUpdating?.Invoke();
        ResetPlayer();
        CustomEvents.TimeCycle.OnUpdated?.Invoke();
    }

    /// <summary>
    /// Resets player values and UI
    /// </summary>
    private void ResetPlayer()
    {
        TeleportPlayer(spawnPoint.position);
        if (bIsSleepingInTent)
        {
            isInHouse = false;
            CustomEvents.Camera.OnSetPlayerLocation?.Invoke(PlayerLocation.FirstIslands);
        }
        else
        {
            isInHouse = true;
            CustomEvents.Camera.OnSetPlayerLocation?.Invoke(PlayerLocation.House);
        }

        CustomEvents.TaskSystem.OnClearAllTasks?.Invoke();
        CustomEvents.InventorySystem.PlayerInventory.OnResetUIInventory?.Invoke();
        CustomEvents.InventorySystem.PlayerInventory.ToggleUI?.Invoke(false);
    }

    private void SetSpawnPos(Vector3 _pos)
    {
        spawnPoint.position = _pos;
    }

    private void ToggleStatusBar(bool state)
    {
        if (state)
        {
            sliderUI.SetActive(true);
        }
        else
        {
            sliderUI.SetActive(false);
        }
    }

    private void SetIsInTent(bool state)
    {
        bIsSleepingInTent = state;
    }

    /// <summary>
    /// Disables control
    /// </summary>
    /// <param name="_state">State</param>
    private void OnDisablePlayer(bool _state)
    {
        isInControl = _state;
    }

    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();
        
        data.Add(location.ToString());
        data.Add(agent.speed.ToString());
        data.Add(agent.angularSpeed.ToString());

        return data;
    }

    public void LoadData(SerializableList _data)
    {
        location = (PlayerLocation)Enum.Parse( typeof(PlayerLocation), _data[0]);
        agent.speed = float.Parse(_data[1]);
        agent.angularSpeed = float.Parse(_data[2]);
    }
}
