using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script that all Task Slots (UI) use
/// Stores the data that the Task Slot utilises to display data and execute functions for cancelling and removing tasks
/// </summary>
public class TaskQueueSlot : MonoBehaviour
{
    private Image taskImage;
    private iExecutable task;

    [SerializeField] private Button removeTaskButton;

    private void Awake()
    {
        taskImage = GetComponent<Image>();
    }

    /// <summary>
    /// Function to add a task to the Task Slot of the UI
    /// Sets the correct task logo and displays it
    /// Enables the cancel actions button on the slot
    /// </summary>
    /// <param name="_task"></param>
    public void AddTaskToSlot(iExecutable _task)
    {
        task = _task;
        taskImage.sprite = task.GetIcon();
        taskImage.enabled = true;
        removeTaskButton.interactable = true;
    }

    /// <summary>
    /// Sets the task to nothing and disbales all UI components
    /// </summary>
    public void DestroySlot()
    {
        task = null;
        taskImage.sprite = null;
        taskImage.enabled = false;
        removeTaskButton.interactable = false;
    }

    /// <summary>
    /// Removes the task from the slot
    /// Checks to make sure certain tasks don't need adjustments like planting
    /// Changes the state of the player to allow them to move again normally
    /// </summary>
    public void RemoveTaskButton()
    {
        if(task != null)
        {
            CustomEvents.TaskSystem.OnCheckClearTask?.Invoke(task);
            CustomEvents.TaskSystem.OnRemoveTask?.Invoke(0);
            CustomEvents.TaskSystem.OnUpdateUI?.Invoke();
            CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Waiting);
        }
        else
        {
            Debug.Log("No Tasks To Remove");
        }
    }
}
