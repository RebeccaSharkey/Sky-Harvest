using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SCR_CheatSystem : MonoBehaviour
{
    [SerializeField] private GameObject cheatUI;
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private TextMeshProUGUI pastCommands;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
        {
            cheatUI.SetActive(!cheatUI.activeSelf);
            if (cheatUI.activeSelf)
            {
                CustomEvents.TimeCycle.OnPause?.Invoke();
                CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(false);
            }
            else
            {
                CustomEvents.TimeCycle.OnUnpause?.Invoke();
                CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(true);
            }
        }
    }

    public void OnTextChange()
    {
        if (userInput.text.EndsWith("\n"))
        {
            CheckCommand(userInput.text);
            userInput.text = "";
        }
    }

    private void CheckCommand(string command)
    {
        if (command == "")
        {
            pastCommands.text += "No Command Entered \n\n";
            return;
        }

        pastCommands.text += "Command Written: " + userInput.text;
        string[] words = command.Split('_');

        words[words.Length - 1] = words[words.Length - 1].Replace("\n", "");

        switch (words[0])
        {
            case "Add":
                {
                    switch (words[1])
                    {
                        case "Fertilizer":
                        case "Fish":
                        case "Produce":
                        case "SeedPackets":
                        case "Seeds":
                            if(words.Length >= 3)
                            {
                                SCR_Items newItem = Resources.Load<SCR_Items>("Items/" + words[1] + "/" + words[2]);
                                if (newItem != null)
                                {
                                    if (words.Length == 4)
                                    {
                                        try
                                        {
                                            int result = Int32.Parse(words[3]);
                                            if (result >= 1 && result <= 999)
                                            {
                                                pastCommands.text += words[3] + " " + words[2] + " added to player inventory.\n\n";
                                                CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(newItem, result, ItemQuality.Perfect);
                                                return;
                                            }
                                            else
                                            {
                                                pastCommands.text += "Invalid Number: " + words[3] + "\n\n";
                                                return;
                                            }
                                        }
                                        catch (FormatException)
                                        {
                                            pastCommands.text += "Invalid Number: " + words[3] + "\n\n";
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        pastCommands.text += words[3] + " " + words[2] + " added to player inventory.\n\n";
                                        CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(newItem, 1, ItemQuality.Perfect);
                                    }
                                }
                                return;
                            }
                            else
                            {
                                pastCommands.text += "Invalid Item: " + words[2] + "\n\n";
                                return;
                            }
                        case "ItemsToComplete":
                            {
                                ItemManager itemManager = Resources.Load<ItemManager>("Items/Item Manager");
                                foreach(SCR_Items item in itemManager.allCropItems)
                                {
                                    CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(item, 1, ItemQuality.Perfect);
                                }
                                foreach(SCR_Items item in itemManager.allFishItems)
                                {
                                    CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(item, 1, ItemQuality.Perfect);
                                }
                            }
                            break;
                        case "AllSeeds":
                            {
                                ItemManager itemManager = Resources.Load<ItemManager>("Items/Item Manager");
                                foreach (SCR_Items item in itemManager.allSeedItems)
                                {
                                    CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(item, 1, ItemQuality.Perfect);
                                }
                            }
                            break;
                        default:
                            pastCommands.text += "Invalid Item Type: " + words[1] + "\n\n";
                            return;
                    }
                    break;
                }
            case "Money":
                {
                    switch(words[1])
                    {
                        case "Add":
                            {
                                try
                                {
                                    int result = Int32.Parse(words[2]);
                                    if (result >= 1 && result <= 9999)
                                    {
                                        pastCommands.text += words[2] + " Frozen Raindrops added.\n\n";
                                        CustomEvents.Currency.OnAddCurrency?.Invoke(result);
                                        return;
                                    }
                                    else
                                    {
                                        pastCommands.text += "Invalid Amount: " + words[2] + "\n\n";
                                        return;
                                    }
                                }
                                catch(FormatException)
                                {
                                    pastCommands.text += "Invalid Number Format: " + words[2] + "\n\n";
                                    return;
                                }
                            }
                        case "Remove":
                            {
                                try
                                {
                                    int result = Int32.Parse(words[2]);
                                    if (result >= 1 && result <= 9999)
                                    {
                                        pastCommands.text += words[2] + " Frozen Raindrops removed.\n\n";
                                        CustomEvents.Currency.OnRemoveCurrency?.Invoke(result);
                                        return;
                                    }
                                    else
                                    {
                                        pastCommands.text += "Invalid Amount: " + words[2] + "\n\n";
                                        return;
                                    }
                                }
                                catch (FormatException)
                                {
                                    pastCommands.text += "Invalid Number Format: " + words[2] + "\n\n";
                                    return;
                                }
                            }
                        default:
                            pastCommands.text += "Invalid Currency Command: " + words[1] + "\n\n";
                            return;
                    }
                }
            case "Time":
                {
                    switch (words[1])
                    {
                        case "Pause":
                            pastCommands.text += "Time Paused.\n\n";
                            CustomEvents.TimeCycle.OnPauseTimeOnly?.Invoke(false);
                            return;
                        case "Play":
                            pastCommands.text += "Time Played.\n\n";
                            CustomEvents.TimeCycle.OnPauseTimeOnly?.Invoke(true);
                            return;
                        case "SkipDay":
                            //Unpause Everything
                            CustomEvents.TimeCycle.OnUnpause?.Invoke();
                            CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(true);
                            CustomEvents.TimeCycle.OnPauseTimeOnly?.Invoke(true);
                            //Turn Off UI
                            cheatUI.SetActive(!cheatUI.activeSelf);
                            //Next Day
                            CustomEvents.TimeCycle.OnDayEnd?.Invoke();
                            break;
                        default:
                            pastCommands.text += "Invalid Time Command: " + words[1] + "\n\n";
                            return;
                    }
                }
                break;
        default:
                pastCommands.text += "Command Unknown \n\n";
                return;
        }
    }
}
