using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Updates the Task Slot UI by removing or adding the slots to the UI panel
/// </summary>
public class TaskSlotUI : MonoBehaviour
{
    private TaskQueueSlot[] slots;

    private void Awake()
    {
        //Stores an array of task slots from the children of the queue parent
        slots = transform.GetComponentsInChildren<TaskQueueSlot>();
    }

    /// <summary>
    /// Checks the length of the slots
    /// Updates the UI by grabbing the task list from the TaskSystem.cs
    /// </summary>
    void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            List<iExecutable> taskList = CustomEvents.TaskSystem.OnGetTaskList?.Invoke();
            if(i < taskList.Count)
            {
                slots[i].AddTaskToSlot(taskList[i]);
            }
            else
            {
                slots[i].DestroySlot();
            }
        }
    }

    private void OnEnable()
    {
        CustomEvents.TaskSystem.OnUpdateUI += UpdateUI;
    }

    private void OnDisable()
    {
        CustomEvents.TaskSystem.OnUpdateUI -= UpdateUI;
    }
}
