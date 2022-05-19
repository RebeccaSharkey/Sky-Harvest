using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TaskSystem : MonoBehaviour
{
    //Stores a list of Tasks the player needs to perform
    public List<iExecutable> taskList;
    [SerializeField] private int maxAmountOfTasks = 5;

    private void Awake()
    {
        //Initialises a new List of tasks
        taskList = new List<iExecutable>();
    }


    public void RequestTask(int _part)
    {
        //if the list isn't empty then get the first task in the list
        if (taskList.Count > 0)
        {
            taskList[0].ExecuteTask(_part);
        }
    }

    //When a task is completed, remove it from the front of the task list
    private void TaskComplete()
    {
        taskList.RemoveAt(0);
        CustomEvents.TaskSystem.OnUpdateUI?.Invoke();
    }

    private void ClearAllTasks()
    {
        foreach (iExecutable task in taskList)
        {
            CheckClearTask(task);
        }
        taskList.Clear();
        CustomEvents.TaskSystem.OnUpdateUI?.Invoke();
    }

    private void CheckClearTask(iExecutable task)
    {
        switch (task.ToString())
        {
            case "FertiliseTask":
                CustomEvents.TaskSystem.OnPlayerStopMovement?.Invoke();
                CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(task.GetItem(), 1, task.GetQuality());
                break;
            case "PlantTask":
                CustomEvents.TaskSystem.OnPlayerStopMovement?.Invoke();
                CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(task.GetItem(), 1, task.GetQuality());
                break;
            case "HarvestTask":
                CustomEvents.TaskSystem.OnPlayerStopMovement?.Invoke();
                CustomEvents.InventorySystem.PlayerInventory.OnRemoveItemStack?.Invoke(task.GetItem(), task.GetQuality());
                break;
            case "MoveTask":
            case "WaterTask":
            case "ClearPlotTask":
            case "SleepTask":
            case "EnterExitTask":
            case "BuyIslandTask":
            case "OpenBagTask":
            case "OpenSiloTask":
            case "TeleportToIslandTask":
            case "OpenSpecialShopTask":
            case "OnOpenShopTask":
            case "OpenWeeklyTaskBoardTask":
            case "BuyAnimalPenTask":
            case "FeedAnimalTask":
            case "HarvestAnimalTask":
            case "RenameAnimalTask":
            case "PetAnimalTask":
            case "BuyAnimalTask":
            case "BuyTentTask":
            case "OpenCraftMachine":
            case "FishTask":
            case "PlantTreeTask":
            case "WaterTreeTask":
            case "HarvestTreeTask":
            case "CutTreeTask":
            case "ChangeClothesTask":
            case "OpenAquarium":
                CustomEvents.TaskSystem.OnPlayerStopMovement?.Invoke();
                break;
        }
    }

    private void RemoveTask(int index)
    {
        taskList.RemoveAt(index);
    }

    private List<iExecutable> GetTaskList()
    {
        return taskList;
    }

    //Adds a task to the task list to be executed
    public void AddTask(iExecutable task)
    {
        if (taskList.Count < maxAmountOfTasks)
        {
            taskList.Add(task);
            CustomEvents.TaskSystem.OnUpdateUI?.Invoke();
        }
        else
        {
            Debug.Log("Task queue is full, please wait for task to finish");
        }
    }

    private void OnEnable()
    {
        CustomEvents.TaskSystem.OnAddNewTask += AddTask;
        CustomEvents.TaskSystem.OnRequestNewTask += RequestTask;
        CustomEvents.TaskSystem.OnTaskComplete += TaskComplete;
        CustomEvents.TaskSystem.OnClearAllTasks += ClearAllTasks;
        CustomEvents.TaskSystem.OnGetTaskList += GetTaskList;
        CustomEvents.TaskSystem.OnCheckClearTask += CheckClearTask;
        CustomEvents.TaskSystem.OnRemoveTask += RemoveTask;
    }

    private void OnDisable()
    {
        CustomEvents.TaskSystem.OnAddNewTask -= AddTask;
        CustomEvents.TaskSystem.OnRequestNewTask -= RequestTask;
        CustomEvents.TaskSystem.OnTaskComplete -= TaskComplete;
        CustomEvents.TaskSystem.OnClearAllTasks -= ClearAllTasks;
        CustomEvents.TaskSystem.OnGetTaskList -= GetTaskList;
        CustomEvents.TaskSystem.OnCheckClearTask -= CheckClearTask;
        CustomEvents.TaskSystem.OnRemoveTask -= RemoveTask;
    }
}
