using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SCR_Tutorial : MonoBehaviour, iSaveable
{
    [SerializeField] private GameObject ui;
    [SerializeField] private TextMeshProUGUI dialogue;
    [SerializeField] private GameObject forwardButton;
    [SerializeField] private GameObject backwardsButton;
    [SerializeField] private GameObject tutorialButton;

    //Progression Bools
    private bool usingTutorials = true;
    private bool floorPressed = false;
    private bool cameraMoved = false;
    private bool doorPressed = false;
    private bool plotPressed = false;
    private bool planted = false;
    private bool newDay = false;
    private bool harvested = false;
    private int step = 1;

    private void Start()
    {
        ui.SetActive(false);
        tutorialButton.SetActive(true);
    }

    private void StartTutorial()
    {
        CustomEvents.SceneManagement.OnSetNewGame?.Invoke(false);
        usingTutorials = true;
        ui.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(WriteText("Hello! Welcome to Sky Harvest, I'm Farmer Tractor and I will be helping you get started!\n\nFirst off, lets get moving shall we?\nClick anywhere on the floor to move to that position."));
        CustomEvents.Tutorial.OnStartTutorial -= StartTutorial;
        step = 1;
        backwardsButton.SetActive(false);
        if (floorPressed)
        {
            forwardButton.SetActive(true);
        }
        else
        {
            forwardButton.SetActive(false);
        }
    }

    private void FloorPressed(bool state)
    {
        if ((!floorPressed && usingTutorials && state) || (floorPressed && usingTutorials && !state))
        {
            floorPressed = true;
            StopAllCoroutines();
            ui.SetActive(true);
            StartCoroutine(WriteText("That's the basics of movement, well done!\n\nNow lets move the camera! To rotate the camera use Q and E. Give it a try and look around your lovely new house!\n When not in your house press and hold the right mouse button while dragging the mouse," +
                " or use the WASD keys to move the camera. Don't worry if you ever lose yourself you can use F to return the camera back to the player."));
            CustomEvents.Tutorial.OnFloorPressed -= FloorPressed;
            step = 2;
            backwardsButton.SetActive(true);
            if (cameraMoved)
            {
                forwardButton.SetActive(true);
            }
            else
            {
                forwardButton.SetActive(false);
            }
        }
        else
        {
            step--;
            forwardButton.SetActive(false);
        }
    }

    private void CameraMovement(bool state)
    {
        if ((!cameraMoved && floorPressed && usingTutorials && state) || (cameraMoved && floorPressed && usingTutorials && !state))
        {
            cameraMoved = true;
            StopAllCoroutines();
            ui.SetActive(true);
            StartCoroutine(WriteText("Yey that's camera movement sorted. Now you know how to move and look around lets head to the farm!\n\nTo interact with objects in the world click on them with the left mouse button.\n\n" +
                "Try clicking on your house door to leave!"));
            CustomEvents.Tutorial.OnCameraMovement -= CameraMovement;
            step = 3;
            backwardsButton.SetActive(true);
            if (doorPressed)
            {
                forwardButton.SetActive(true);
            }
            else
            {
                forwardButton.SetActive(false);
            }
        }
        else
        {
            step--;
            forwardButton.SetActive(false);
        }
    }

    private void DoorPressed(bool state)
    {
        if ((!doorPressed && usingTutorials && state) || (doorPressed && usingTutorials && !state))
        {
            doorPressed = true;
            StopAllCoroutines();
            ui.SetActive(true);
            StartCoroutine(WriteText("Welcome to your new farm! This is where the majority of your work will take place!\n\nFirst things first lets plant some crops... have a look around for some plots and interact with them to get started."));
            CustomEvents.Tutorial.OnDoorPressed -= DoorPressed;
            step = 4;
            backwardsButton.SetActive(true);
            if (plotPressed)
            {
                forwardButton.SetActive(true);
            }
            else
            {
                forwardButton.SetActive(false);
            }
        }
        else
        {
            step--;
            forwardButton.SetActive(false);
        }
    }

    private void PlotClicked(bool state)
    {
        if ((!plotPressed && usingTutorials && state) || (plotPressed && usingTutorials && !state))
        {
            plotPressed = true;
            StopAllCoroutines();
            ui.SetActive(true);
            StartCoroutine(WriteText("Here you can see what seeds the player has in their inventory to be planted.\n\nWe only started with wheat, so lets plant one of those. Right click the item and choose to plant it!"));
            CustomEvents.Tutorial.OnPlotClicked -= PlotClicked;
            step = 5;
            backwardsButton.SetActive(true);
            if (planted)
            {
                forwardButton.SetActive(true);
            }
            else
            {
                forwardButton.SetActive(false);
            }
        }
        else
        {
            step--;
            forwardButton.SetActive(false);
        }
    }

    private void SeedPlanted(bool state)
    {
        if ((!planted && usingTutorials && state) || (planted && usingTutorials && !state))
        {
            planted = true;
            StopAllCoroutines();
            ui.SetActive(true);
            StartCoroutine(WriteText("Well done, you've planted your first crop!\nRemember to come back and interact with the plot each day to water it, once you have access to fertilizer you" +
                " can also fertilize it too for better outcomes!\n\nHave a look around your farm and click on other things to interact with them until the end of that day, maybe even plant some more crops!"));
            CustomEvents.Tutorial.OnSeedPlanted -= SeedPlanted;
            step = 6;
            backwardsButton.SetActive(true);
            if (newDay)
            {
                forwardButton.SetActive(true);
            }
            else
            {
                forwardButton.SetActive(false);
            }
        }
        else
        {
            step--;
            forwardButton.SetActive(false);
        }
    }

    private void NewDay()
    {
        NewDay(true);
    }

    private void NewDay(bool state)
    {
        if ((!newDay && usingTutorials && planted && state) || (newDay && usingTutorials && planted && !state))
        {
            newDay = true;
            StopAllCoroutines();
            ui.SetActive(true);
            StartCoroutine(WriteText("Good Morning Farmer! After a busy day planting and exploring yesterday looks like you've made it to a new day! At the end of each day you will be brought back to your bed to sleep so you're not too tired for the next day.\nIf you ever want to" +
                " sleep before the day is up, come to your home and click on your bed to sleep sooner.\n\nNow it's time to go and water your plants again and explore some more!"));
            CustomEvents.TimeCycle.OnDayStart -= NewDay;
            step = 7;
            backwardsButton.SetActive(true);
            if (harvested)
            {
                forwardButton.SetActive(true);
            }
            else
            {
                forwardButton.SetActive(false);
            }
        }
        else
        {
            step--;
            forwardButton.SetActive(false);
        }
    }

    private void Harvested(bool state)
    {
        if ((!harvested && usingTutorials && state) || (harvested && usingTutorials && !state))
        {
            harvested = true;
            StopAllCoroutines();
            ui.SetActive(true);
            StartCoroutine(WriteText("Wowza! Looks like you just harvested your first crop! You can press 'I' to open your inventory and check it out, or go to the shop to sell it to buy more seeds!\n\n" +
                "Now that you've planted, watered and harvested you've got the basics down! Remember to complete yoour quest you must harvest all possible crops and catch all fish! Good luck Farmer!"));
            CustomEvents.Tutorial.OnHarvested -= Harvested;
            step = 8;
            backwardsButton.SetActive(true);
            forwardButton.SetActive(false);
        }
        else
        {
            step--;
            forwardButton.SetActive(false);
        }
    }

    public void Close()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");
        StopAllCoroutines();
        ui.SetActive(false);
        tutorialButton.SetActive(true);
    }

    private IEnumerator WriteText(string newText)
    {
        dialogue.text = "";
        foreach (char c in newText)
        {
            dialogue.text += c;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    private void ChangeVisableIntructions()
    {
        switch (step)
        {
            case 1:
                StartTutorial();
                return;
            case 2:
                FloorPressed(false);
                return;
            case 3:
                CameraMovement(false);
                return;
            case 4:
                DoorPressed(false);
                return;
            case 5:
                PlotClicked(false);
                return;
            case 6:
                SeedPlanted(false);
                return;
            case 7:
                NewDay(false);
                return;
            case 8:
                Harvested(false);
                return;

        }
    }

    public void OnForward()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");
        if (step < 8)
        {
            step++;
            if (step == 8)
            {
                forwardButton.SetActive(false);
            }
            ChangeVisableIntructions();
        }
    }

    public void OnBackwards()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");
        if (step > 1)
        {
            step--;
            if (step == 1)
            {
                forwardButton.SetActive(true);
            }
            ChangeVisableIntructions();
        }
    }

    public void OnOpenTutorial()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");
        tutorialButton.SetActive(false);
        ChangeVisableIntructions();
    }

    private void OnEnable()
    {
        CustomEvents.Tutorial.OnStartTutorial += StartTutorial;
        CustomEvents.Tutorial.OnFloorPressed += FloorPressed;
        CustomEvents.Tutorial.OnCameraMovement += CameraMovement;
        CustomEvents.Tutorial.OnDoorPressed += DoorPressed;
        CustomEvents.Tutorial.OnPlotClicked += PlotClicked;
        CustomEvents.Tutorial.OnSeedPlanted += SeedPlanted;
        CustomEvents.TimeCycle.OnDayStart += NewDay;
        CustomEvents.Tutorial.OnHarvested += Harvested;
    }
    private void OnDisable()
    {
        CustomEvents.Tutorial.OnStartTutorial -= StartTutorial;
        CustomEvents.Tutorial.OnFloorPressed -= FloorPressed;
        CustomEvents.Tutorial.OnCameraMovement -= CameraMovement;
        CustomEvents.Tutorial.OnDoorPressed -= DoorPressed;
        CustomEvents.Tutorial.OnPlotClicked -= PlotClicked;
        CustomEvents.Tutorial.OnSeedPlanted -= SeedPlanted;
        CustomEvents.TimeCycle.OnDayStart -= NewDay;
        CustomEvents.Tutorial.OnHarvested -= Harvested;
    }

    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();

        data.Add(usingTutorials.ToString());
        data.Add(floorPressed.ToString());
        data.Add(cameraMoved.ToString());
        data.Add(doorPressed.ToString());
        data.Add(plotPressed.ToString());
        data.Add(planted.ToString());
        data.Add(newDay.ToString());
        data.Add(harvested.ToString());
        data.Add(step.ToString());

        return data;
    }

    public void LoadData(SerializableList _data)
    {
        if (_data.Count <= 0) return;

        usingTutorials = bool.Parse(_data[0]);
        floorPressed = bool.Parse(_data[1]);
        cameraMoved = bool.Parse(_data[2]);
        doorPressed = bool.Parse(_data[3]);
        plotPressed = bool.Parse(_data[4]);
        planted = bool.Parse(_data[5]);
        newDay = bool.Parse(_data[6]);
        harvested = bool.Parse(_data[7]);
        step = int.Parse(_data[8]);

    }
}
