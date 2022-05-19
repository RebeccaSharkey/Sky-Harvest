using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Handles time passing related events
/// </summary>
public class TimeController : MonoBehaviour, iSaveable
{
    [SerializeField] private int _day = 1;
    private int day
    {
        get
        {
            return _day;
        }
        set
        {
            _day = value;
            CustomEvents.UI.OnDayChanged?.Invoke(_day);
        }
    }
    [SerializeField] private int _hour = 0;
    private int hour
    {
        get
        {
            return _hour;
        }
        set
        {
            _hour = value;
            CustomEvents.UI.OnTimeChangedHours?.Invoke(_hour);
        }
    }
    [SerializeField] private int _minutes = 0;
    private int minutes
    {
        get
        {
            return _minutes;
        }
        set
        {
            _minutes = value;
            CustomEvents.UI.OnTimeChangedMinutes?.Invoke(_minutes);
        }
    }
    [SerializeField] private float fiveMinuteInterval = 10f;
    [SerializeField] private int startHour = 7, endHour = 24;

    private Quaternion initialRotation;
    [SerializeField] private float timeRotationSpeed;

    private bool timePassing = true;
    private bool waitingForTransition = false;

    private WaitForSeconds fiveMinuteWait;
    private Coroutine timePassingRoutine;

    [SerializeField] private Light directionalLight;
    

    private void OnEnable()
    {
        CustomEvents.TimeCycle.OnDayStart += StartNewDay;
        CustomEvents.TimeCycle.OnSleep += EndDay;
        CustomEvents.TimeCycle.OnNewDaySetup += NewDaySetup;
        CustomEvents.TimeCycle.OnPauseTimeOnly += PauseTime;
        CustomEvents.TimeCycle.OnPause += PauseGame;
        CustomEvents.TimeCycle.OnUnpause += UnpauseGame;
        CustomEvents.TimeCycle.OnGetPaused += IsPaused;
        CustomEvents.UI.OnTransitionFinish += TransitionFinish;
    }

    private void OnDisable()
    {
        CustomEvents.TimeCycle.OnDayStart -= StartNewDay;
        CustomEvents.TimeCycle.OnSleep -= EndDay;
        CustomEvents.TimeCycle.OnNewDaySetup -= NewDaySetup;
        CustomEvents.TimeCycle.OnPauseTimeOnly += PauseTime;
        CustomEvents.TimeCycle.OnPause -= PauseGame;
        CustomEvents.TimeCycle.OnUnpause -= UnpauseGame;
        CustomEvents.TimeCycle.OnGetPaused -= IsPaused;
        CustomEvents.UI.OnTransitionFinish -= TransitionFinish;
    }
 
    private void Awake()
    {
        fiveMinuteWait = new WaitForSeconds(fiveMinuteInterval);
        initialRotation = directionalLight.transform.localRotation;
        //if (SaveLoadManager.isNewGameStarted) day = 1;
    }

    private void Start()
    {
        CustomEvents.UI.OnDayChanged?.Invoke(_day);
    }

    private void Update()
    {
        directionalLight.transform.Rotate(Vector3.forward * timeRotationSpeed * Time.deltaTime);
    }

    private void NewDaySetup()
    {
        day++;
    }

    /// <summary>
    /// Resets variables, ticks day counter, and starts the next day time coroutine
    /// </summary>
    private void StartNewDay()
    {
        hour = startHour;
        minutes = 0;
        if(timePassingRoutine != null) StopCoroutine(timePassingRoutine);
        timePassing = true;
        timePassingRoutine = StartCoroutine(PassTime());
        CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Waiting);
    }

    private void EndDay()
    {
        StartCoroutine(EndDayRoutine());
    }

    /// <summary>
    /// Transitions between gameplay to end of day screen
    /// </summary>
    /// <returns>Ienumerator</returns>
    private IEnumerator EndDayRoutine()
    {
        CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(false);
        CustomEvents.PlayerControls.OnToggleSliderUI?.Invoke(false);
        waitingForTransition = true;
        CustomEvents.UI.OnRandomFadeOut?.Invoke();

        if(timePassingRoutine != null) StopCoroutine(timePassingRoutine);
        timePassing = false;
        CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Stopped);
        directionalLight.transform.localRotation = initialRotation;

        while (waitingForTransition) yield return null;

        CustomEvents.TimeCycle.OnDayEnd?.Invoke();
        CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(true);
    }

    private void TransitionFinish()
    {
        waitingForTransition = false;
    }

    #region PAUSE METHODS
    private bool isPaused = true;
    private bool IsPaused() 
    { 
        return isPaused; 
    }

    /// <summary>
    /// Sets timescale to 0 to pause the game
    /// </summary>
    private void PauseGame() 
    { 
        Time.timeScale = 0; 
        isPaused = true; 
        CustomEvents.UI.OnTogglePauseButton?.Invoke(false); 
    }

    /// <summary>
    /// Restores timescale to 1 to unpause the game
    /// </summary>
    private void UnpauseGame() 
    { 
        Time.timeScale = 1; 
        isPaused = false; 
        CustomEvents.UI.OnTogglePauseButton?.Invoke(true); 
    }

    /// <summary>
    /// Changes the bool used within time passing
    /// </summary>
    private void PauseTime(bool state)
    {
        timePassing = state;
    }
    #endregion

    /// <summary>
    /// Handles the main time tick
    /// </summary>
    /// <returns>Ienumerator</returns>
    private IEnumerator PassTime()
    {
        while(true)
        {
            if (timePassing)
            {
                yield return fiveMinuteWait;
                minutes += 5;
                if (minutes >= 60)
                {
                    minutes = 0;
                    hour++;
                    if (hour % 3 == 0)
                    {
                        CustomEvents.WeatherSystem.OnChangeWeather?.Invoke();
                    }

                    if (hour >= endHour)
                    {
                        //force day end
                        EndDay();
                        CustomEvents.WeatherSystem.OnChangeWeather?.Invoke();
                    }
                }
            }
            yield return null;
        }
    }

    /// <summary>
    /// Returns local data for saving
    /// </summary>
    /// <returns>Current day</returns>
    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();
        data.Add(day.ToString());

        return data;
    }

    /// <summary>
    /// Restores current day value upon loading
    /// </summary>
    public void LoadData(SerializableList _data)
    {
        day = int.Parse(_data[0]);
    }
}
