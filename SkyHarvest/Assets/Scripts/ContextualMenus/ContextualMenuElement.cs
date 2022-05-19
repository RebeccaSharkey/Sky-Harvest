using UnityEngine;

/// <summary>
/// Contextual menu element base class
/// All new contextual menu elements should inherit from this
/// </summary>
public class ContextualMenuElement
{
    public string buttontext;

    /// <summary>
    /// Function is required by the contextual menu interface and remains constant, so is defined in the base class
    /// </summary>
    /// <returns>The text to be shown on the contextual menu button</returns>
    public string GetButtonText()
    {
        return buttontext;
    }
}

/// <summary>
/// Contextual menu element for planting a new crop
/// </summary>
public class PlantMenuElement : ContextualMenuElement, iContextualMenuable
{
    PlotBehaviour plot;

    public PlantMenuElement(PlotBehaviour _plot)
    {
        buttontext = "Plant";
        plot = _plot;
    }

    public void OnSelected()
    {
        /*iExecutable plantTask = new PlantTask(plot);
        CustomEvents.TaskSystem.OnAddNewTask?.Invoke(plantTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();*/

        //Brings up the inventory to pick which seed to plant
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
        CustomEvents.TimeCycle.OnPause?.Invoke();
        CustomEvents.InventorySystem.PlayerInventory.OnPlant?.Invoke(plot);
        CustomEvents.Tutorial.OnPlotClicked?.Invoke(true);

    }
}

/// <summary>
/// Contextual menu element for watering a crop
/// </summary>
public class WaterMenuElement: ContextualMenuElement, iContextualMenuable
{
    PlotBehaviour plot;

    public WaterMenuElement(PlotBehaviour _plot)
    {
        buttontext = "Water";
        plot = _plot;
    }

    public void OnSelected()
    {
        iExecutable waterTask = new WaterTask(plot);
        CustomEvents.TaskSystem.OnAddNewTask(waterTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

/// <summary>
/// Contextual menu element for fertilising a crop
/// </summary>
public class FertiliseMenuElement : ContextualMenuElement, iContextualMenuable
{
    PlotBehaviour plot;

    public FertiliseMenuElement(PlotBehaviour _plot)
    {
        buttontext = "Fertilise";
        plot = _plot;
    }

    public void OnSelected()
    {
        /*iExecutable fertiliseTask = new FertiliseTask(plot, "seedYield"); //temp, should bring up menu to select fertiliser
        CustomEvents.TaskSystem.OnAddNewTask?.Invoke(fertiliseTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();*/

        //Brings up the inventory to pick which fertiliser to use
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
        CustomEvents.TimeCycle.OnPause?.Invoke();
        CustomEvents.InventorySystem.PlayerInventory.OnFertilize?.Invoke(plot);
    }
}

/// <summary>
/// Contextual menu element for the cancel button
/// </summary>
public class CancelMenuElement : ContextualMenuElement, iContextualMenuable
{
    public CancelMenuElement()
    {
        buttontext = "Cancel";
    }

    public void OnSelected()
    {
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

/// <summary>
/// Contextual menu element for harvesting a crop
/// </summary>
public class HarvestMenuElement : ContextualMenuElement, iContextualMenuable
{
    PlotBehaviour plot;

    public HarvestMenuElement(PlotBehaviour _plot)
    {
        buttontext = "Harvest";
        plot = _plot;
    }

    public void OnSelected()
    {
        iExecutable harvestTask = new HarvestTask(plot);
        CustomEvents.TaskSystem.OnAddNewTask?.Invoke(harvestTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

/// <summary>
/// Contextual menu element for clearing a blocked plot
/// </summary>
public class ClearPlotMenuElement : ContextualMenuElement, iContextualMenuable
{
    PlotBehaviour plot;

    public ClearPlotMenuElement(PlotBehaviour _plot)
    {
        buttontext = "Clear";
        plot = _plot;
    }

    public void OnSelected()
    {
        //plot.ClearPlot();
        iExecutable clearPlotTask = new ClearPlotTask(plot);
        CustomEvents.TaskSystem.OnAddNewTask?.Invoke(clearPlotTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

/// <summary>
/// Contextual menu element for the sleep option from a bed
/// </summary>
public class SleepMenuElement : ContextualMenuElement, iContextualMenuable
{
    Vector3 bedPos;

    public SleepMenuElement(Vector3 _bedPos)
    {
        buttontext = "Sleep";
        bedPos = _bedPos;
    }

    public void OnSelected()
    {
        iExecutable sleepTask = new SleepTask(bedPos);
        CustomEvents.TaskSystem.OnAddNewTask?.Invoke(sleepTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

public class BuyIslandElement : ContextualMenuElement, iContextualMenuable
{
    GameObject islandPostPos;

    public BuyIslandElement(GameObject _obj)
    {
        buttontext = "Buy Island";
        islandPostPos = _obj;
    }

    public void OnSelected()
    {
        iExecutable buyIslandTask = new BuyIslandTask(islandPostPos);
        CustomEvents.TaskSystem.OnAddNewTask?.Invoke(buyIslandTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

/// <summary>
/// Contextual menu element for entering or exiting the house
/// </summary>
public class EnterExitMenuElement : ContextualMenuElement, iContextualMenuable
{
    Vector3 doorPos;

    public EnterExitMenuElement(Vector3 _doorPos)
    {
        buttontext = "Go through";
        doorPos = _doorPos;
    }

    public void OnSelected()
    {
        iExecutable enterExitTask = new EnterExitTask(doorPos);
        CustomEvents.TaskSystem.OnAddNewTask?.Invoke(enterExitTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

/// <summary>
/// Contextual menu element for buying an animal pen
/// </summary>
public class BuyAnimalPenElement : ContextualMenuElement, iContextualMenuable
{
    GameObject pen;

    public BuyAnimalPenElement(GameObject _obj)
    {
        buttontext = "Buy Pen";
        pen = _obj;
    }

    public void OnSelected()
    {
        iExecutable buyAnimalPenTask = new BuyAnimalPen(pen);
        CustomEvents.TaskSystem.OnAddNewTask?.Invoke(buyAnimalPenTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

/// <summary>
/// Contextual menu element for feeding an animal
/// </summary>
public class FeedAnimalElement : ContextualMenuElement, iContextualMenuable
{
    Vector3 animalPos;
    AnimalBehaviour animal;

    public FeedAnimalElement(AnimalBehaviour _animal, Vector3 _animalPos, string _buttonText)
    {
        buttontext = _buttonText;
        animalPos = _animalPos;
        animal = _animal;
    }

    public void OnSelected()
    {
        iExecutable feedAnimalTask = new FeedAnimal(animal, animalPos);
        CustomEvents.TaskSystem.OnAddNewTask?.Invoke(feedAnimalTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

/// <summary>
/// Contextual menu element for harvesting an animal
/// </summary>
public class HarvestAnimalElement : ContextualMenuElement, iContextualMenuable
{
    Vector3 animalPos;
    AnimalBehaviour animal;

    public HarvestAnimalElement(AnimalBehaviour _animal, Vector3 _animalPos)
    {
        buttontext = "Harvest";
        animalPos = _animalPos;
        animal = _animal;
    }

    public void OnSelected()
    {
        iExecutable harvestAnimalTask = new HarvestAnimal(animal, animalPos);
        CustomEvents.TaskSystem.OnAddNewTask?.Invoke(harvestAnimalTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

public class RenameAnimalElement : ContextualMenuElement, iContextualMenuable
{
    Vector3 animalPos;
    AnimalBehaviour animal;

    public RenameAnimalElement(AnimalBehaviour _animal, Vector3 _animalPos)
    {
        buttontext = "Rename";
        animalPos = _animalPos;
        animal = _animal;
    }

    public void OnSelected()
    {
        iExecutable renameAnimalTask = new RenameAnimal(animal, animalPos);
        CustomEvents.TaskSystem.OnAddNewTask?.Invoke(renameAnimalTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

public class PetAnimalElement : ContextualMenuElement, iContextualMenuable
{
    private Vector3 animalPos;
    private AnimalBehaviour animal;

    public PetAnimalElement(AnimalBehaviour _animal, Vector3 _pos)
    {
        buttontext = "Pet";
        animalPos = _pos;
        animal = _animal;
    }

    public void OnSelected()
    {
        iExecutable petAnimalTask = new PetAnimal(animal, animalPos);
        CustomEvents.TaskSystem.OnAddNewTask?.Invoke(petAnimalTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

/// <summary>
/// Contextual menu element for buying an animal
/// </summary>
public class BuyAnimalElement : ContextualMenuElement, iContextualMenuable
{
    Vector3 penPos;
    AnimalPen pen;
    private AnimalBehaviour animalScript;

    public BuyAnimalElement(AnimalPen _pen, Vector3 _penPos, string _buttonText)
    {
        buttontext = _buttonText;
        penPos = _penPos;
        pen = _pen;
    }

    public void OnSelected()
    {
        iExecutable BuyAnimalTask = new BuyAnimal(pen, penPos);
        CustomEvents.TaskSystem.OnAddNewTask?.Invoke(BuyAnimalTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

/// <summary>
/// Contextual menu element for buying a tent
/// </summary>
public class BuyTentElement : ContextualMenuElement, iContextualMenuable
{
    GameObject tent;
    Vector3 pos;
    private TentBuying tentScript;

    public BuyTentElement(Vector3 _pos, TentBuying _script)
    {
        buttontext = "Buy";
        tentScript = _script;
        pos = _pos;
    }

    public void OnSelected()
    {
        iExecutable BuyTentTask = new BuyTent(pos, tentScript);
        CustomEvents.TaskSystem.OnAddNewTask?.Invoke(BuyTentTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

/// <summary>
/// Contextual menu element for using the teleporter
/// </summary>
public class UseTeleportPoleElement : ContextualMenuElement, iContextualMenuable
{
    Vector3 pos;
    public UseTeleportPoleElement(Vector3 _pos)
    {
        buttontext = "Use";
        pos = _pos;
    }

    public void OnSelected()
    {
        iExecutable TeleportTask = new TeleportToIsland(pos);
        CustomEvents.TaskSystem.OnAddNewTask?.Invoke(TeleportTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

/// <summary>
/// Contextual menu element for using... things
/// (Will maybe separate these out)
/// </summary>
public class UseSiloBagShopElement: ContextualMenuElement, iContextualMenuable
{
    GameObject gObject;

    public UseSiloBagShopElement(GameObject _gObject)
    {
        buttontext = "Use";
        gObject = _gObject;
    }

    public void OnSelected()
    {
        switch(gObject.tag)
        {
            case "Silo":
                iExecutable openSilo = new OpenSiloTask(gObject);
                CustomEvents.TaskSystem.OnAddNewTask?.Invoke(openSilo);
                break;
            case "SpecialShop":
                iExecutable openSpecialShop = new OpenSpecialShopTask(gObject);
                CustomEvents.TaskSystem.OnAddNewTask?.Invoke(openSpecialShop);
                break;
            case "Bag":
                iExecutable openBag = new OpenBagTask(gObject);
                CustomEvents.TaskSystem.OnAddNewTask?.Invoke(openBag);
                break;
            case "Weekly Task Board":
                iExecutable openTaskBoard = new OpenWeeklyTaskBoardTask(gObject);
                CustomEvents.TaskSystem.OnAddNewTask?.Invoke(openTaskBoard);
                break;
            case "CraftingMachine":
                iExecutable openCraftMachine = new OpenCraftMachine(gObject);
                CustomEvents.TaskSystem.OnAddNewTask?.Invoke(openCraftMachine);
                break;
            case "Shop":
                iExecutable openShop = new OnOpenShopTask(gObject);
                CustomEvents.TaskSystem.OnAddNewTask?.Invoke(openShop);
                break;
        }
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

/// <summary>
/// Contextual menu element for fishing
/// </summary>
public class FishElement : ContextualMenuElement, iContextualMenuable
{
    GameObject gObject;

    public FishElement(GameObject _gObject)
    {
        buttontext = "Fish";
        gObject = _gObject;
    }

    public void OnSelected()
    {
        iExecutable fishTask = new FishTask(gObject);
        CustomEvents.TaskSystem.OnAddNewTask?.Invoke(fishTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

public class OpenAquariumContext : ContextualMenuElement, iContextualMenuable
{
    GameObject gObject;

    public OpenAquariumContext(GameObject _gObject)
    {
        buttontext = "Open Aquarium";
        gObject = _gObject;
    }

    public void OnSelected()
    {
        iExecutable aquariumTask = new OpenAquarium(gObject);
        CustomEvents.TaskSystem.OnAddNewTask?.Invoke(aquariumTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

public class PlantTreeElement : ContextualMenuElement, iContextualMenuable
{
    private FruitTreePlotBehaviour fruitPlotScript;
    private GameObject plotObj;

    public PlantTreeElement(FruitTreePlotBehaviour _plotScript, GameObject _plot)
    {
        buttontext = "Plant";
        fruitPlotScript = _plotScript;
        plotObj = _plot;
    }
    
    public void OnSelected()
    {
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
        CustomEvents.TimeCycle.OnPause?.Invoke();
        CustomEvents.InventorySystem.PlayerInventory.OnPlantTree?.Invoke(fruitPlotScript);
        CustomEvents.Tutorial.OnPlotClicked?.Invoke(true);
    }
}

public class WaterTreeElement: ContextualMenuElement, iContextualMenuable
{
    FruitTreePlotBehaviour tree;
    private GameObject treeObj;

    public WaterTreeElement(FruitTreePlotBehaviour _tree, GameObject _treeObj)
    {
        buttontext = "Water";
        tree = _tree;
        treeObj = _treeObj;
    }

    public void OnSelected()
    {
        iExecutable waterTreeTask = new WaterTreeTask(tree, treeObj);
        CustomEvents.TaskSystem.OnAddNewTask(waterTreeTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

public class HarvestTreeElement: ContextualMenuElement, iContextualMenuable
{
    FruitTreePlotBehaviour tree;
    private GameObject treeObj;

    public HarvestTreeElement(FruitTreePlotBehaviour _tree, GameObject _treeObj)
    {
        buttontext = "Harvest";
        tree = _tree;
        treeObj = _treeObj;
    }

    public void OnSelected()
    {
        iExecutable harvestTreeTask = new HarvestTreeTask(tree, treeObj);
        CustomEvents.TaskSystem.OnAddNewTask(harvestTreeTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

public class CutTreeElement : ContextualMenuElement, iContextualMenuable
{
    FruitTreePlotBehaviour tree;
    private GameObject treeObj;

    public CutTreeElement(FruitTreePlotBehaviour _tree, GameObject _treeObj)
    {
        buttontext = "Cut Down";
        tree = _tree;
        treeObj = _treeObj;
    }
    
    public void OnSelected()
    {
        iExecutable cutTreeTask = new CutTreeTask(tree, treeObj);
        CustomEvents.TaskSystem.OnAddNewTask(cutTreeTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}

public class ChangeClothesElement : ContextualMenuElement, iContextualMenuable
{
    private GameObject wardrobe;

    public ChangeClothesElement(GameObject _obj)
    {
        buttontext = "Change Appearance";
        wardrobe = _obj;
    }

    public void OnSelected()
    {
        iExecutable changeClothesTask = new ChangeClothesTask(wardrobe);
        CustomEvents.TaskSystem.OnAddNewTask(changeClothesTask);
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();
    }
}
