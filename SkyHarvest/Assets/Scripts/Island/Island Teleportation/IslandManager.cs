using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manager that keeps track of all teleportation locations the player can move to
/// </summary>
public class IslandManager : MonoBehaviour
{
    public List<Vector3> teleportPositions;
    private List<string> teleportNames;
    [SerializeField] private TMP_Dropdown islandDropdownList;

    private int index;

    private void Awake()
    {
        teleportPositions = new List<Vector3>();
        teleportNames = new List<string>();
        islandDropdownList.ClearOptions();
    }

    /// <summary>
    /// Adds a position to the list of locations when an island is bought
    /// Adds the name of the location added into the list
    /// </summary>
    /// <param name="_pos">Teleport location passed in to add to the list</param>
    /// <param name="_name">Name of the location passed in to show the player</param>
    private void AddPosition(Vector3 _pos, string _name)
    {
        teleportPositions.Add(_pos);
        teleportNames.Add(_name);
        
    }

    /// <summary>
    /// Adds the locations into a Dropdown menu for the player to choose from
    /// </summary>
    private void SetDropdownUI()
    {
        islandDropdownList.ClearOptions();
        islandDropdownList.AddOptions(teleportNames);
        index = 0;
        //islandDropdownList.onValueChanged.AddListener(delegate { LocationSelected(islandDropdownList); });
    }

    /// <summary>
    /// Index used to keep track of the option selected within in the position list
    /// </summary>
    /// <param name="_index">Index of the positions list to select individual elements</param>
    public void LocationSelected(int _index)
    {
        index = _index;
    }

    /// <summary>
    /// Teleports the player to the location selected from the dropdown menu
    /// Uses the index from Location Selected to grab the element from the position list
    /// Toggles the UI off after selecting an option
    /// </summary>
    public void TeleportPlayer()
    {
        CustomEvents.IslandSystem.Teleportation.OnTeleportPlayer?.Invoke(teleportPositions[index]);
        if (teleportNames[index] == "Snowy Islands")
        {
            CustomEvents.Camera.OnSetPlayerLocation?.Invoke(PlayerLocation.SnowyIslands);
            CustomEvents.Audio.OnStopAllSound?.Invoke();
            CustomEvents.Audio.OnPlayThemeSong?.Invoke("Snowy Islands Theme");
        }
        else
        {
            CustomEvents.Camera.OnSetPlayerLocation?.Invoke(PlayerLocation.FirstIslands);
            CustomEvents.Audio.OnStopAllSound?.Invoke();
            CustomEvents.Audio.OnPlayThemeSong?.Invoke("First Islands Theme");
        }
        CustomEvents.TimeCycle.OnUnpause?.Invoke();
        CustomEvents.IslandSystem.Teleportation.OnToggleUI?.Invoke(false);
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Teleporting");
    }

    /// <summary>
    /// Same functionality as ResetPositions
    /// Allows UI buttons to call this function instead
    /// </summary>
    public void CloseMenu()
    {
        CustomEvents.IslandSystem.Teleportation.OnResetTeleportPos?.Invoke();
        CustomEvents.IslandSystem.Teleportation.OnToggleUI?.Invoke(false);
        CustomEvents.Scripts.OnDisablePlayer?.Invoke(true);
        CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(true);
        CustomEvents.TimeCycle.OnUnpause?.Invoke();
    }

    /// <summary>
    /// Removes all options from the dropdown menu
    /// </summary>
    private void ResetPositions()
    {
        islandDropdownList.ClearOptions();
    }

    private void OnEnable()
    {
        CustomEvents.IslandSystem.Teleportation.OnAddTeleportPos += AddPosition;
        CustomEvents.IslandSystem.Teleportation.OnResetTeleportPos += ResetPositions;
        CustomEvents.IslandSystem.Teleportation.OnSetDropdownUI += SetDropdownUI;

    }

    private void OnDisable()
    {
        CustomEvents.IslandSystem.Teleportation.OnAddTeleportPos -= AddPosition;
        CustomEvents.IslandSystem.Teleportation.OnResetTeleportPos -= ResetPositions;
        CustomEvents.IslandSystem.Teleportation.OnSetDropdownUI -= SetDropdownUI;
    }
}
