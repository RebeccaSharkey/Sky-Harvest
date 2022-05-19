using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Task : iExecutable
{
    public Sprite taskIcon;
    public virtual void ExecuteTask(int _part) 
    {
        Debug.LogError("ExecuteTask not overridden / base.ExecuteTask left in");
    }

    public Sprite GetIcon()
    {
        return taskIcon;
    }

    private SCR_Items item;
    public virtual SCR_Items GetItem()
    {
        return item;
    }

    private ItemQuality quality;
    public virtual ItemQuality GetQuality()
    {
        return quality;
    }

    private float timeToWait;
    public virtual IEnumerator WaitForTaskCompletion()
    {
        yield return new WaitForSeconds(timeToWait);
    }
}

//Moves the player based on the Vector3 passed in
//Sets the NavMeshAgent destination to the Vector3 passed in
public class MoveTask : Task
{
    Vector3 posToMoveTo;
    public MoveTask(Vector3 pos)
    {
        posToMoveTo = pos;
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/MoveTaskLogo");
    }

    public override void ExecuteTask(int _part)
    {
        CustomEvents.TaskSystem.OnPlayerMoved?.Invoke(posToMoveTo);
        CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Moving);

    }
}

public class PlantTask : Task
{
    MoveTask moveTask;
    PlotBehaviour plot;
    string crop;
    SCR_Items item;
    ItemQuality quality;

    public PlantTask(PlotBehaviour _plot, string _crop, InventorySlot _item)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/PlantTaskLogo");
        moveTask = new MoveTask(_plot.gameObject.transform.position);
        plot = _plot;
        crop = _crop;
        item = _item.Item;
        quality = _item.Quality;
    }

    public override void ExecuteTask(int _part)
    {
        switch(_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                moveTask.GetIcon();
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Planting);
                break;
            case 1:

                plot.PlantCrop(crop);

                break;
        }
    }

    public override SCR_Items GetItem()
    {
        return item;
    }

    public override ItemQuality GetQuality()
    {
        return quality;
    }
}

public class WaterTask : Task
{
    MoveTask moveTask;
    PlotBehaviour plot;

    public WaterTask(PlotBehaviour _plot)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/WaterTaskLogo");
        moveTask = new MoveTask(_plot.gameObject.transform.position);
        plot = _plot;
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Watering);
                break;
            case 1:
                //Wait for a certain amount of time

                plot.WaterCrop();
                break;
        }
    }
}

public class FertiliseTask : Task
{
    MoveTask moveTask;
    PlotBehaviour plot;
    string fertiliserType;
    SCR_Items item;
    ItemQuality quality;
    float timeToWait;

    public FertiliseTask(PlotBehaviour _plot, string _fertiliserType, InventorySlot _item/*, float _timer*/)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/FertiliseTaskLogo");
        moveTask = new MoveTask(_plot.gameObject.transform.position);
        plot = _plot;
        fertiliserType = _fertiliserType;
        item = _item.Item;
        quality = _item.Quality;
        //timeToWait = _timer;
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Fertilising);
                break;
            case 1:
                //CustomEvents.TaskSystem.OnWaitForTaskCompletion?.Invoke(timeToWait);
                plot.FertiliseCrop(fertiliserType);
                break;
        }
    }

    public override SCR_Items GetItem()
    {
        return item;
    }

    public override ItemQuality GetQuality()
    {
        return quality;
    }
}

public class HarvestTask : Task
{
    MoveTask moveTask;
    PlotBehaviour plot;

    public HarvestTask(PlotBehaviour _plot)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/HarvestTaskLogo");
        moveTask = new MoveTask(_plot.gameObject.transform.position);
        plot = _plot;
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Harvesting);
                break;
            case 1:
                plot.HarvestCrop();
                CustomEvents.Tutorial.OnHarvested?.Invoke(true);
                break;
        }
    }
}

public class ClearPlotTask : Task
{
    MoveTask moveTask;
    PlotBehaviour plot;

    public ClearPlotTask(PlotBehaviour _plot)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/HarvestTaskLogo"); //CHANGE LATER
        moveTask = new MoveTask(_plot.gameObject.transform.position);
        plot = _plot;
    }

    public override void ExecuteTask(int _part)
    {
        switch(_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.ClearingPlot);
                break;
            case 1:
                plot.ClearPlot();
                break;
        }
    }
}

public class SleepTask : Task
{
    MoveTask moveTask;

    public SleepTask(Vector3 _bedPos)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/SleepTaskLogo");
        moveTask = new MoveTask(_bedPos);
    }

    public override void ExecuteTask(int _part)
    {
        switch(_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Sleeping);
                break;
            case 1:
                CustomEvents.TimeCycle.OnSleep?.Invoke();
                break;
        }
    }
}

public class EnterExitTask : Task
{
    MoveTask moveTask;

    public EnterExitTask(Vector3 _doorPos)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/Enter House UI");
        moveTask = new MoveTask(_doorPos);
    }

    public override void ExecuteTask(int _part)
    {
        moveTask.ExecuteTask(0);
        CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.EnteringHouse);
    }
}

public class BuyIslandTask : Task
{
    MoveTask moveTask;
    GameObject post;

    public BuyIslandTask(GameObject _post)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/MoveTaskLogo");
        post = _post;
        moveTask = new MoveTask(post.transform.position);
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Buying);
                break;
            case 1:
                post.GetComponent<IslandBuy>().OpenUI();
                break;
        }
    }
}

public class OpenBagTask : Task
{
    MoveTask moveTask;
    GameObject bag;

    public OpenBagTask(GameObject _bag)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/Open Storage Task");
        bag = _bag;
        moveTask = new MoveTask(bag.transform.position);
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.UsingInventory);
                break;
            case 1:
                CustomEvents.InventorySystem.BagInventory.OnOpenBag?.Invoke(bag.GetComponent<SCR_Bags>().BagID);
                CustomEvents.TimeCycle.OnPause?.Invoke();
                break;
        }
    }
}

public class OpenSiloTask : Task
{
    MoveTask moveTask;
    GameObject silo;

    public OpenSiloTask(GameObject _silo)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/Open Storage Task");
        silo = _silo;
        moveTask = new MoveTask(silo.transform.position);
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.UsingInventory);
                break;
            case 1:
                CustomEvents.InventorySystem.SiloInventory.OnOpenSilo(silo.GetComponent<SCR_Silos>().SiloID);
                CustomEvents.InventorySystem.PlayerInventory.OnSiloOpened?.Invoke(silo.GetComponent<SCR_Silos>().SiloID);
                CustomEvents.TimeCycle.OnPause?.Invoke();
                break;
        }
    }
}

public class TeleportToIsland : Task
{
    MoveTask moveTask;

    public TeleportToIsland(Vector3 teleportPost)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/MoveTaskLogo");
        moveTask = new MoveTask(teleportPost);
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Buying);
                break;
            case 1:
                CustomEvents.IslandSystem.Teleportation.OnToggleUI?.Invoke(true);
                CustomEvents.IslandSystem.Teleportation.OnSetDropdownUI?.Invoke();
                CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(false);
                CustomEvents.Scripts.OnDisablePlayer?.Invoke(false);
                CustomEvents.TimeCycle.OnPause?.Invoke();
                break;
        }
    }
}

public class OpenSpecialShopTask : Task
{
    MoveTask moveTask;
    GameObject shop;

    public OpenSpecialShopTask(GameObject _shop)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/MoveTaskLogo");
        shop = _shop;
        moveTask = new MoveTask(shop.transform.position);
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.UsingInventory);
                break;
            case 1:
                if(shop.GetComponentInParent<SCR_SpecialShop>().IsShopDocked)
                {
                    CustomEvents.ShopSystem.SpecialShop.OnOpenSpecialShop?.Invoke(shop.GetComponentInParent<SCR_SpecialShopInventory>()._shopID);
                }
                else
                {
                    shop.GetComponentInParent<SCR_SpecialShop>().OnNotDocked();
                }
                CustomEvents.TimeCycle.OnPause?.Invoke();
                break;
        }
    }
}

public class OnOpenShopTask : Task
{
    MoveTask moveTask;
    GameObject shop;

    public OnOpenShopTask(GameObject _shop)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/Open Storage Task");
        shop = _shop;
        moveTask = new MoveTask(shop.transform.position);
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.UsingInventory);
                break;
            case 1:
                shop.GetComponent<SCR_ShopInventory>().OpenShop(shop.GetComponent<SCR_ShopInventory>()._shopID);
                CustomEvents.TimeCycle.OnPause?.Invoke();
                break;
        }
    }
}

public class OpenWeeklyTaskBoardTask : Task
{
    MoveTask moveTask;
    GameObject taskBoard;

    public OpenWeeklyTaskBoardTask(GameObject _taskBoard)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/MoveTaskLogo");
        taskBoard = _taskBoard;
        moveTask = new MoveTask(taskBoard.transform.position);
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.UsingInventory);
                break;
            case 1:
                CustomEvents.WeeklyTasks.OnOpenTaskboard?.Invoke(taskBoard.GetComponentInParent<SCR_WeeklyTasks>().TaskBoardID);
                CustomEvents.TimeCycle.OnPause?.Invoke();
                break;
        }
    }
}

public class BuyAnimalPen : Task
{
    MoveTask moveTask;
    GameObject pen;

    public BuyAnimalPen(GameObject _pen)
    {
        pen = _pen;
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/MoveTaskLogo");
        moveTask = new MoveTask(pen.transform.position);
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Buying);
                break;
            case 1:
                pen.GetComponent<AnimalPenBuying>().OpenPenUI();
                CustomEvents.TimeCycle.OnPause?.Invoke();
                break;
        }
    }
}

public class FeedAnimal : Task
{
    MoveTask moveTask;
    AnimalBehaviour animal;

    public FeedAnimal(AnimalBehaviour _animal, Vector3 _animalPos)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/Feed Animals task");
        moveTask = new MoveTask(_animalPos);
        animal = _animal;
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Feeding);
                break;
            case 1:
                animal.Feed();
                break;
        }
    }
}

public class HarvestAnimal : Task
{
    MoveTask moveTask;
    AnimalBehaviour animal;

    public HarvestAnimal(AnimalBehaviour _animal, Vector3 _animalPos)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/HarvestTaskLogo");
        moveTask = new MoveTask(_animalPos);
        animal = _animal;
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Harvesting);
                break;
            case 1:
                animal.Harvest();
                break;
        }
    }
}

public class RenameAnimal : Task
{
    MoveTask moveTask;
    AnimalBehaviour animal;

    public RenameAnimal(AnimalBehaviour _animal, Vector3 _animalPos)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/MoveTaskLogo");
        moveTask = new MoveTask(_animalPos);
        animal = _animal;
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Renaming);
                break;
            case 1:
                animal.Rename("");
                break;
        }
    }
}

public class PetAnimal : Task
{
    private MoveTask moveTask;
    private AnimalBehaviour animal;

    public PetAnimal(AnimalBehaviour _animal, Vector3 _pos)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/Petting animal task UI");
        moveTask = new MoveTask(_pos);
        animal = _animal;
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Petting);
                break;
            case 1:
                animal.Pet();
                break;
        }
    }
}

public class BuyAnimal : Task
{
    MoveTask moveTask;
    AnimalPen pen;
    Vector3 penPos;

    public BuyAnimal(AnimalPen _pen, Vector3 _penPos)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/MoveTaskLogo");
        penPos = _penPos;
        moveTask = new MoveTask(penPos);
        pen = _pen;
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.AnimalBuying);
                break;
            case 1:
                switch (pen.animalType)
                {
                    case AnimalType.Chicken:
                    case AnimalType.Cow:
                    case AnimalType.Pig:
                    case AnimalType.Horse:
                    case AnimalType.Sheep:
                        pen.SpawnAnimal();
                        break;
                }
                break;
        }
    }
}

public class BuyTent : Task
{
    MoveTask moveTask;
    private TentBuying tentScript;
    Vector3 movePos;

    public BuyTent(Vector3 _movePos, TentBuying _script)
    {
        movePos = _movePos;
        tentScript = _script;
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/MoveTaskLogo");
        moveTask = new MoveTask(movePos);
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.TentBuying);
                break;
            case 1:
                tentScript.SetUpUI();
                break;
        }
    }
}

public class OpenCraftMachine : Task
{
    MoveTask moveTask;
    GameObject _craftMachine;

    public OpenCraftMachine(GameObject craftMachine)
    {
        _craftMachine = craftMachine;
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/MoveTaskLogo");
        moveTask = new MoveTask(craftMachine.transform.position);
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.UsingInventory);
                break;
            case 1:
                CustomEvents.CraftMachine.OnOpenCraftingMachine?.Invoke(_craftMachine.GetComponent<SCR_CraftMachine>()._machineID);
                CustomEvents.TimeCycle.OnPause?.Invoke();
                break;
        }
    }
}

public class FishTask : Task
{
    MoveTask moveTask;
    GameObject _water;

    public FishTask(GameObject water)
    {
        _water = water;
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/Fishing Task UI");
        moveTask = new MoveTask(_water.transform.position);
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Fishing);
                break;
            case 1:
                CustomEvents.Fishing.OnOpenFishingUI?.Invoke(_water.GetComponent<SCR_Fishing>().WaterID);
                break;
        }
    }
}

public class PlantTreeTask : Task
{
    private MoveTask moveTask;
    private FruitTreePlotBehaviour fruitScript;
    private SCR_Items item;
    private string treeName;
    private ItemQuality quality;

    public PlantTreeTask(FruitTreePlotBehaviour _fruitScript, string _name, InventorySlot _itemSlot)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/PlantTaskLogo");
        fruitScript = _fruitScript;
        treeName = _name;
        item = _itemSlot.Item;
        quality = _itemSlot.Quality;
        
        moveTask = new MoveTask(fruitScript.gameObject.transform.position);
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Planting);
                break;
            case 1:
                fruitScript.Plant(treeName);
                break;
        }
    }
}

public class WaterTreeTask : Task
{
    private MoveTask moveTask;
    private FruitTreePlotBehaviour fruitScript;
    private GameObject tree;

    public WaterTreeTask(FruitTreePlotBehaviour _fruitScript, GameObject _tree)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/WaterTaskLogo");
        fruitScript = _fruitScript;
        tree = _tree;
        moveTask = new MoveTask(tree.transform.position);
    }

    public override void ExecuteTask(int _part)
    {
        switch(_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Watering);
                break;
            case 1:
                fruitScript.Water();
                break;
        }
    }
}

public class HarvestTreeTask : Task
{
    private MoveTask moveTask;
    private FruitTreePlotBehaviour fruitScript;
    private GameObject tree;

    public HarvestTreeTask(FruitTreePlotBehaviour _fruitScript, GameObject _tree)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/HarvestTaskLogo");
        fruitScript = _fruitScript;
        tree = _tree;
        moveTask = new MoveTask(tree.transform.position);
    }

    public override void ExecuteTask(int _part)
    {
        switch(_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Harvesting);
                break;
            case 1:
                fruitScript.Harvest();
                break;
        }
    }
}

public class CutTreeTask : Task
{
    private MoveTask moveTask;
    private FruitTreePlotBehaviour fruitScript;
    private GameObject tree;
    
    public CutTreeTask(FruitTreePlotBehaviour _fruitScript, GameObject _tree)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/HarvestTaskLogo");
        fruitScript = _fruitScript;
        tree = _tree;
        moveTask = new MoveTask(tree.transform.position);
    }
    
    public override void ExecuteTask(int _part)
    {
        switch(_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Harvesting);
                break;
            case 1:
                fruitScript.CutDown();
                break;
        }
    }
}

public class ChangeClothesTask : Task
{
    private MoveTask moveTask;
    private GameObject wardrobe;

    public ChangeClothesTask(GameObject _obj)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/MoveTaskLogo");
        wardrobe = _obj;
        moveTask = new MoveTask(wardrobe.transform.position);
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.ChangingAppearance);
                break;
            case 1:
                CustomEvents.PlayerCustomisation.OnToggleUI?.Invoke(true);
                break;
        }
    }
}

public class OpenAquarium : Task
{
    private MoveTask moveTask;
    private GameObject tank;

    public OpenAquarium(GameObject _obj)
    {
        taskIcon = Resources.Load<Sprite>("Graphics/TaskQueueUI/MoveTaskLogo");
        tank = _obj;
        moveTask = new MoveTask(tank.transform.position);
    }

    public override void ExecuteTask(int _part)
    {
        switch (_part)
        {
            case 0:
                moveTask.ExecuteTask(0);
                CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.UsingInventory);
                break;
            case 1:
                CustomEvents.Fishing.OnToggleAquarium?.Invoke(true);
                CustomEvents.TimeCycle.OnPause?.Invoke();
                break;
        }
    }
}
