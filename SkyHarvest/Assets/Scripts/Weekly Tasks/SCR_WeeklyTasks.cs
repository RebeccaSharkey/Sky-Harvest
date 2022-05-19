using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_WeeklyTasks : MonoBehaviour, iSaveable
{
    private List<PlayerTask> playerTasks = new List<PlayerTask>();
    private int _taskBoardID = -1;
    public int TaskBoardID { get => _taskBoardID; }
    private int dayCount = 0;
    private int tasksCompleted = 0;
    [SerializeField] private List<SCR_Items> rewards;

    private void Awake()
    {
        //Creates a new list of tasks and passes the same list to the UI to use.
        playerTasks = new List<PlayerTask>();
        CustomEvents.WeeklyTasks.OnGetTaskList?.Invoke(playerTasks, _taskBoardID);

        //Sets the ID of the weekly task board by seeing how many task boards there are in scene
        GameObject[] weeklyTaskBoards;
        weeklyTaskBoards = GameObject.FindGameObjectsWithTag("Weekly Task Board");
        if (weeklyTaskBoards.Length == 0)
        {
            _taskBoardID = 1;
        }
        else
        {
            //Loops through the array of Weekly task boards and sets the Task boards ID to the gameObjects place in the array
            for (int i = 0; i < weeklyTaskBoards.Length; i++)
            {
                if (this.gameObject == weeklyTaskBoards[i].transform.parent.gameObject)
                {
                    _taskBoardID = i;
                }
            }
        }
    }

    private void Start()
    {
        CustomEvents.WeeklyTasks.OnUpdateTasksCompletedText?.Invoke(tasksCompleted);
    }

    private void NewGame()
    {
        dayCount = 0;
        tasksCompleted = 0;
        UpdateTasks();
    }

    private void OnOpenTaskBoard(int id)
    {
        if(id == _taskBoardID)
        {
            CustomEvents.WeeklyTasks.OnGetTaskList?.Invoke(playerTasks, _taskBoardID);
            CustomEvents.WeeklyTasks.ToggleUI?.Invoke(true, _taskBoardID);
        }
    }

    private void UpdateTasks()
    {
        //Deletes all old tasks before creating new ones.
        playerTasks.Clear();

        //Creates 7 new tasks and adds them to the list
        for (int i = 0; i < 7; i++)
        {
            PlayerTask task = new PlayerTask();
            List<SCR_Items> availableItems = new List<SCR_Items>();
            availableItems = (List<SCR_Items>)CustomEvents.AvailableItems.OnGetWeeklyTasks?.Invoke();
            task.LoadItems(availableItems);
            playerTasks.Add(task);
        }

        //Gives the new task list to the UI
        CustomEvents.WeeklyTasks.OnGetTaskList?.Invoke(playerTasks, _taskBoardID);
    }

    private void DeleteCompletedTask(PlayerTask task, int taskBoardID)
    {
        //Called when a task gets deleted by the player (Player completes task)
        if (taskBoardID == _taskBoardID)
        {
            //Removes task from list
            playerTasks.Remove(task);

            //Gives the player the reward
            CustomEvents.Currency.OnAddCurrency?.Invoke(task.Reward);

            //Gives the updated task list to the UI
            CustomEvents.WeeklyTasks.OnGetTaskList?.Invoke(playerTasks, _taskBoardID);
        }
    }

    //Increments day count every new day
    private void UpdateDays()
    {
        dayCount++;

        //If it's been 7 days then creates new tasks and resets day count
        if (dayCount == 7)
        {
            UpdateTasks();
            dayCount = 0;
        }
    }

    private void TaskCompleted()
    {
        tasksCompleted++;
        if(tasksCompleted == 7)
        {
            tasksCompleted = 0;
            foreach(SCR_Items item in rewards)
            {
                CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(item, 1, ItemQuality.Normal);
            }
        }
        CustomEvents.WeeklyTasks.OnUpdateTasksCompletedText?.Invoke(tasksCompleted);
    }

    //Events Used
    private void OnEnable()
    {
        CustomEvents.WeeklyTasks.OnOpenTaskboard += OnOpenTaskBoard;
        CustomEvents.WeeklyTasks.OnDeleteTask += DeleteCompletedTask;
        CustomEvents.TimeCycle.OnDayStart += UpdateDays;
        CustomEvents.WeeklyTasks.OnTaskCompleted += TaskCompleted;
        CustomEvents.Tutorial.OnStartTutorial += NewGame;
    }

    private void OnDisable()
    {
        CustomEvents.WeeklyTasks.OnOpenTaskboard -= OnOpenTaskBoard;
        CustomEvents.WeeklyTasks.OnDeleteTask -= DeleteCompletedTask;
        CustomEvents.TimeCycle.OnDayStart -= UpdateDays;
        CustomEvents.WeeklyTasks.OnTaskCompleted -= TaskCompleted;
        CustomEvents.Tutorial.OnStartTutorial -= NewGame;
    }

    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();

        data.Add(dayCount.ToString());
        data.Add(tasksCompleted.ToString());

        foreach(PlayerTask task in playerTasks)
        {
            data.Add($"{task.Item.name.ToString()}#{task.Amount.ToString()}#{task.Reward.ToString()}#{task.RequireQuality.ToString()}" +
                $"#{task.Quality.ToString()}");
        }

        return data;
    }

    public void LoadData(SerializableList _data)
    {
        if (_data.Count <= 0) return;

        dayCount = int.Parse(_data[0]);
        _data.Remove(_data[0]);
        tasksCompleted = int.Parse(_data[0]);
        _data.Remove(_data[0]);

        playerTasks.Clear();

        foreach (string item in _data)
        {
            if (string.IsNullOrEmpty(item)) return;

            string[] taskInfo = item.Split('#');

            SCR_Items taskItem = Resources.Load<SCR_Items>($"Items/Fertilizer/{taskInfo[0].ToString()}");
            if (taskItem == null) taskItem = Resources.Load<SCR_Items>($"Items/Fish/{taskInfo[0].ToString()}");
            if (taskItem == null) taskItem = Resources.Load<SCR_Items>($"Items/Produce/{taskInfo[0].ToString()}");
            if (taskItem == null) taskItem = Resources.Load<SCR_Items>($"Items/SeedPackets/{taskInfo[0].ToString()}");
            if (taskItem == null) taskItem = Resources.Load<SCR_Items>($"Items/Seeds/{taskInfo[0].ToString()}");
            if (taskItem == null) taskItem = Resources.Load<SCR_Items>($"Items/Unique Items/{item.ToString()}");
            if (taskItem == null) taskItem = Resources.Load<SCR_Items>($"Items/Unique Items/Recipes/{item.ToString()}");
            if (taskItem == null)
            {
                Debug.LogError($"Null SO object for {taskInfo[1]}");
                continue;
            }

            PlayerTask newTask = new PlayerTask(taskItem, int.Parse(taskInfo[1]), int.Parse(taskInfo[2]), bool.Parse(taskInfo[3]), (ItemQuality)Enum.Parse(typeof(ItemQuality), taskInfo[4]));
            playerTasks.Add(newTask);
        }
    }
}
