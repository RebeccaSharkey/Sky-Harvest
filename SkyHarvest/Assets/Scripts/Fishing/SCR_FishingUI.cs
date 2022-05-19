using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCR_FishingUI : MonoBehaviour
{
    [Header("UI Variables")]
    [SerializeField] private GameObject fishingUI;
    [SerializeField] private GameObject fishButton;
    [SerializeField] private GameObject fishPullBar;
    [SerializeField] private GameObject fishCompletionBar;
    [SerializeField] private GameObject fishIndicator;
    [SerializeField] private GameObject fishGreen;
    [SerializeField] private GameObject[] fishRed = new GameObject[2];
    [SerializeField] private GameObject timerObject;
    [SerializeField] private GameObject timerText;
    [SerializeField] private TextMeshProUGUI endText;
    [SerializeField] private List<GameObject> uiToHide;


    [Header("Implementation Variables")]
    private int waterID;
    private SO_FishObject currentFish = null;
    private bool isFishing = false;
    private bool isPlaying = false;
    [SerializeField] private Vector3[] bounds = new Vector3[2];
    [SerializeField] private float playerPullSpeed = 1f;
    private float fishPullSpeed;
    private Coroutine coroutine;
    private float timer = 0f;
    private bool fishCaught = false;

    private bool randomFishEvent = false;
    private float fishEventElapsedTime = 0f;
    private float fishEventDuration = 3f;
    private float fishSpeedMultiplier = 0f;


    public void Update()
    {
        if(isPlaying)
        {
            timer -= Time.deltaTime;
            timerText.GetComponent<TextMeshProUGUI>().text = timer.ToString("F2");

            Vector3 newPos = fishIndicator.transform.localPosition;

            //Moves Indicator if player presses space
            if (Input.GetKey(KeyCode.Space))
            {
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    newPos += (-transform.right * playerPullSpeed) * Time.deltaTime;
                }
                else
                {
                    newPos += (transform.right * playerPullSpeed) * Time.deltaTime;
                }
            }

            //Fish push back action
            if(!randomFishEvent)
            {
                fishEventElapsedTime = 0f;
                fishEventDuration = Random.Range(0.5f, 3f);
                float probability = Random.Range(0f, 1f);
                if(probability >= 0.8f)
                {
                    fishSpeedMultiplier = Random.Range(-200f, -50f);
                }
                else
                {
                    fishSpeedMultiplier = Random.Range(50f, 250f);
                }
                randomFishEvent = true;
            }
            else
            {
                if(fishEventElapsedTime < fishEventDuration)
                {
                    fishEventElapsedTime += Time.deltaTime;
                }
                else
                {
                    randomFishEvent = false;
                }
            }
            newPos += (-transform.right * (fishPullSpeed * fishSpeedMultiplier)) * Time.deltaTime;

            //Clamps the indicator within the box and moves indicator
            newPos.x = Mathf.Clamp(newPos.x, bounds[0].x, bounds[1].x);
            fishIndicator.transform.localPosition = newPos;

            //Checks if fish is within the right distance
            if (fishIndicator.transform.localPosition.x > (fishGreen.transform.localPosition.x - (fishGreen.GetComponent<RectTransform>().sizeDelta.x / 2)) && fishIndicator.transform.localPosition.x < (fishGreen.transform.localPosition.x + (fishGreen.GetComponent<RectTransform>().sizeDelta.x / 2)))
            {
                fishCompletionBar.GetComponent<Slider>().value += (fishPullSpeed / 2) * Time.deltaTime;

                if(fishCompletionBar.GetComponent<Slider>().value == 1)
                {
                    StopCoroutine(coroutine);
                    fishCaught = true;
                    StartCoroutine(CaughtFish());
                }
            }

            if(fishIndicator.transform.localPosition.x > (fishRed[0].transform.localPosition.x - (fishRed[0].GetComponent<RectTransform>().sizeDelta.x / 2)) && fishIndicator.transform.localPosition.x < (fishRed[0].transform.localPosition.x + (fishRed[0].GetComponent<RectTransform>().sizeDelta.x / 2)))
            {
                fishCompletionBar.GetComponent<Slider>().value -= (fishPullSpeed / 2) * Time.deltaTime;
                if (fishCompletionBar.GetComponent<Slider>().value == 0)
                {
                    StopCoroutine(coroutine);
                    fishCaught = false;
                    StartCoroutine(CaughtFish());
                }
            }
            else if(fishIndicator.transform.localPosition.x > (fishRed[1].transform.localPosition.x - (fishRed[1].GetComponent<RectTransform>().sizeDelta.x / 2)) && fishIndicator.transform.localPosition.x < (fishRed[1].transform.localPosition.x + (fishRed[1].GetComponent<RectTransform>().sizeDelta.x / 2)))
            {
                fishCompletionBar.GetComponent<Slider>().value -= (fishPullSpeed / 2) * Time.deltaTime;
                if (fishCompletionBar.GetComponent<Slider>().value == 0)
                {
                    StopCoroutine(coroutine);
                    fishCaught = false;
                    StartCoroutine(CaughtFish());
                }
            }
        }
    }

    private void ToggleUI(bool newState)
    {
        fishingUI.SetActive(newState);
        if(newState)
        {
            foreach(GameObject gObject in uiToHide)
            {
                gObject.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject gObject in uiToHide)
            {
                gObject.SetActive(true);
            }
        }
    }

    private void SetUpFishing(int thisID)
    {
        waterID = thisID;
        isFishing = true;
        ToggleUI(true);
    }

    public void OnStopFishing()
    {
        if(isFishing)
        {
            StopAllCoroutines();
            fishPullBar.SetActive(false);
            fishCompletionBar.SetActive(false);
            timerObject.SetActive(false);
            isPlaying = false; 
            fishButton.SetActive(true);
            endText.gameObject.SetActive(false);

            isFishing = false;
            CustomEvents.Scripts.OnDisablePlayer?.Invoke(true);
            CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(true);
            //CustomEvents.TaskSystem.OnTaskComplete?.Invoke();
            CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Waiting);
            ToggleUI(false);
        }
    }

    public void OnFish()
    {
        fishButton.SetActive(false);
        //call fishing event
        CustomEvents.Fishing.OnFish?.Invoke(waterID);
    }

    private void SetFish(SO_FishObject newFish)
    {
        currentFish = newFish;
        fishPullSpeed = currentFish.PullSpeed;
    }

    private void PlayMiniGame()
    {
        //Run the UI mini game to catch a fish.
        fishButton.SetActive(false);
        fishPullBar.SetActive(true);
        fishCompletionBar.SetActive(true);
        timerObject.SetActive(true);
        fishIndicator.transform.localPosition = bounds[0] + new Vector3(fishRed[0].GetComponent<RectTransform>().sizeDelta.x + 50f, 0f, 0f);
        fishCompletionBar.GetComponent<Slider>().value = 0.1f;
        timer = currentFish.FishingTime;
        timerText.GetComponent<TextMeshProUGUI>().text = timer.ToString();

        isPlaying = true;
        coroutine = StartCoroutine(fishMiniGameTime(currentFish.FishingTime));
    }

    private void OnEnable()
    {
        CustomEvents.Fishing.OnSetupFishingUI += SetUpFishing;
        CustomEvents.Fishing.OnSetFish += SetFish;
        CustomEvents.Fishing.OnPlayMiniGame += PlayMiniGame;
        CustomEvents.TimeCycle.OnDayEnd += OnStopFishing;
    }

    private void OnDisable()
    {
        CustomEvents.Fishing.OnSetupFishingUI -= SetUpFishing;
        CustomEvents.Fishing.OnSetFish -= SetFish;
        CustomEvents.Fishing.OnPlayMiniGame -= PlayMiniGame;
        CustomEvents.TimeCycle.OnDayEnd -= OnStopFishing;
    }

    IEnumerator CaughtFish()
    {
        fishPullBar.SetActive(false);
        fishCompletionBar.SetActive(false);
        timerObject.SetActive(false);
        isPlaying = false;

        if(fishCaught)
        {
            endText.text = currentFish.Item.ItemName + " Caught!";
            endText.gameObject.SetActive(true);
            CustomEvents.Fishing.OnFishCaught?.Invoke(waterID);
            CustomEvents.Achievements.OnAddToTotalFishCaught?.Invoke(1);
        }
        else
        {
            endText.text = "Fish Escaped!";
            endText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(1f);
        fishButton.SetActive(true);
        endText.gameObject.SetActive(false);
    }

    IEnumerator fishMiniGameTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Set the fish button back active so player can fish again
        fishPullBar.SetActive(false);
        fishCompletionBar.SetActive(false);
        timerObject.SetActive(false);
        isPlaying = false;

        endText.text = "Fish Escaped!";
        endText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        fishButton.SetActive(true);
        endText.gameObject.SetActive(false);
    }
}
