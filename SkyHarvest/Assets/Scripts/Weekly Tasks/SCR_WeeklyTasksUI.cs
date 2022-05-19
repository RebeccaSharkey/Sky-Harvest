using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SCR_WeeklyTasksUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private List<GameObject> objectsToHide;

    [SerializeField] private GameObject weeklyTasksMenu;
    [SerializeField] private GameObject weeklyTaskUISlot;
    [SerializeField] private Transform weeklyTasksContainer;
    [SerializeField] private TextMeshProUGUI taskCompletedText;

    private List<PlayerTask> playerTasks;
    private int _taskBaordID;

    private void Start()
    {
        //_taskBaordID = GetComponentInParent<SCR_WeeklyTasks>().TaskBoardID;
        playerTasks = new List<PlayerTask>();
    }

    private void ToggleUI(bool state, int taskbaordID)
    {
        if (taskbaordID == _taskBaordID)
        {
            weeklyTasksMenu.SetActive(state);
            if(state)
            {
                foreach (GameObject objecty in objectsToHide)
                {
                    objecty.SetActive(false);
                }
            }
            else
            {
                foreach (GameObject objecty in objectsToHide)
                {
                    objecty.SetActive(true);
                }
            }
        }
    }

    public void OnExitClick()
    {
        ToggleUI(false, _taskBaordID);
        CustomEvents.TimeCycle.OnUnpause?.Invoke();
    }

    private void SetTaskList(List<PlayerTask> newList, int taskboardID)
    {
        playerTasks = newList;
        _taskBaordID = taskboardID;
        UpdateTaskboard(_taskBaordID);
    }

    private void UpdateTaskboard(int taskboardID)
    {
        if (taskboardID == _taskBaordID)
        {
            foreach (Transform item in weeklyTasksContainer)
            {
                Destroy(item.gameObject);
            }

            foreach(PlayerTask task in playerTasks)
            {
                GameObject currentSlot = Instantiate(weeklyTaskUISlot, transform.position, Quaternion.identity, gameObject.transform);
                currentSlot.GetComponent<RectTransform>().SetParent(weeklyTasksContainer.gameObject.GetComponent<RectTransform>());
                currentSlot.GetComponent<SCR_WeeklyTaskSlotUI>().SetPlayerTask(task, _taskBaordID);
            }
        }
    }

    private void UpdateTaskCompleted(int currentAmount)
    {
        taskCompletedText.text = "Tasks Completed: " + currentAmount + "/7";
    }

    private void OnEnable()
    {
        CustomEvents.WeeklyTasks.OnGetTaskList += SetTaskList;
        CustomEvents.WeeklyTasks.OnUpdateUI += UpdateTaskboard;
        CustomEvents.WeeklyTasks.ToggleUI += ToggleUI;
        CustomEvents.WeeklyTasks.OnUpdateTasksCompletedText += UpdateTaskCompleted;

    }

    private void OnDisable()
    {
        CustomEvents.WeeklyTasks.OnGetTaskList -= SetTaskList;
        CustomEvents.WeeklyTasks.OnUpdateUI -= UpdateTaskboard;
        CustomEvents.WeeklyTasks.ToggleUI -= ToggleUI;
        CustomEvents.WeeklyTasks.OnUpdateTasksCompletedText -= UpdateTaskCompleted;
    }
}
