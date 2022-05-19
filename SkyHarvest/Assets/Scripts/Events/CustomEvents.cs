using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all defined events
/// </summary>
public class CustomEvents
{
    public class Achievements
    {
        public static Action<int> OnAddToTotalCropsPlanted;
        public static Action<int> OnAddToTotalCropsFertilised;
        public static Action<int> OnAddToTotalFishCaught;
        public static Action<int> OnAddToTotalAnimalsPet;
        public static Action<int> OnAddToTotalUpgradesBought;
        public static Action<int> OnAddToSnowyIslandsBought;
        public static Action<int> OnAddToTotalMoneyGot;

        public static Func<int> OnGetTotalCropsPlanted;
        public static Func<int> OnGetTotalCropsFertilised;
        public static Func<int> OnGetTotalFishCaught;
        public static Func<int> OnGetTotalAnimalsPet;
        public static Func<int> OnGetTotalUpgradesBought;
        public static Func<int> OnGetSnowyIslandsBought;
    }
    
    public class AI
    {
        public static Func<Vector3, float, Vector3> OnGetNextPosition;
        public static Action SetNextPosition;
    }

    public class AnimalPens
    {
        public static Action<int> OnBuyAnimalPen;
        public static Action<string, string, int, int> OnSetUI;
        public static Action<string, string, string, int, int> OnSetBillboardData;
        public static Action OnSetNewID;
        public static Func<int> OnGetNewID;
        public static Action<bool> OnToggleRenamer;
        public static Action<AnimalBehaviour> OnGetAnimalScript;
        public static Action<bool> OnToggleUI;
    }

    public class AvailableItems
    {
        public static Action<SCR_Items> OnAddToShops;
        public static Action<SCR_Items> OnRemoveFromShops;
        public static Func<List<SCR_Items>> OnGetShopList;

        public static Action<SCR_Items> OnAddToSpecialShop;
        public static Action<SCR_Items> OnRemoveFromSpecialShops;
        public static Func<List<SCR_Items>> OnGetSpecialShopList;

        public static Action<SCR_Items> OnAddToWeeklyTasks;
        public static Action<SCR_Items> OnRemoveFromWeeklyTasks;
        public static Func<List<SCR_Items>> OnGetWeeklyTasks;
    }

    public class Audio
    {
        public static Action<string> OnPlaySoundEffect;
        public static Action<string> OnPlayThemeSong;
        public static Action OnStopAllSound;
    }

    public class Camera
    {
        public static Action<Vector3> OnLerpToNewPos;
        public static Func<bool> OnFinishedLerping;
        public static Action<PlayerLocation> OnSetPlayerLocation;
        public static Func<PlayerLocation> OnGetPlayerLocation;
        public class CameraShake
        {
            public static Action<float, float> OnCameraShake;
        }
    }

    public class CraftMachine
    {
        public static Action<int> OnOpenCraftingMachine;
        public static Func<SCR_Items, int, int, ItemQuality, bool> OnAddNewItemStack;
        public static Action<InventorySlot, int> OnRemoveItemStackWithSlot;

        public static Action<Inventory, int> OnSetInventory;
        public static Action OnUpdateUI;
        public static Action<bool> OnToggleUI;

        public static Action<InventorySlot, bool, int> OnOpenInspection;

        public static Action<bool> OnMovingChanged;

        public static Action<Recipes> OnAddToList;
        public static Action<Recipes> OnRemoveFromList;
        public static Func<List<Recipes>> OnGetListOfRecipes;

        public static Action<Recipes> OnRecipeButtonPress;
    }

    public class Crops
    {
        public static Action OnCropPlanted;
        public static Action OnCropWatered;
        public static Action OnCropFertilised;
        public static Action OnCropHarvested;
    }

    public class Currency
    {
        public static Action<int> OnAddCurrency;
        public static Action<int> OnRemoveCurrency;
        public static Func<int> OnGetCurrency;
        public static Func<int, bool> OnCheckCurrency;
    }

    public class FarmersHandbook
    {
        public static Action<SCR_CropItems> OnOpenCroppedia;
        public static Action OnOpenCropPediaPage;
        public static Action OnOpenRecipeBookPage;
    }

    public class Fishing
    {
        public static Action<int> OnOpenFishingUI;
        public static Action<int> OnFish;
        public static Action<int> OnFishCaught;

        public static Action<int> OnSetupFishingUI;
        public static Action<SO_FishObject> OnSetFish;
        public static Action OnPlayMiniGame;

        public static Action<bool> OnToggleAquarium;
        public static Action<SO_Fish> OnIncrimentAmountCaught;
    }

    public class InventorySystem
    {
        public static Action OnInventoryTooFull;

        public static Action<SCR_CropItems> OnShowCropPediaButton;
        public static Action<SCR_CropItems, int> OnShowCropPediaButtons;
        public static Action OnDisableCroppediaButton;
        public static Action OnDisableCroppediaButton2;

        public class PlayerInventory
        {
            public static Action<SCR_Items, int, ItemQuality> OnAddNewItemStack;
            public static Action<SCR_Items, int, ItemQuality> OnSplit;
            public static Action<SCR_Items, ItemQuality> OnRemoveItemStack;
            public static Action<InventorySlot> OnRemoveItemStackWithSlot;
            public static Action<SCR_Items, int, ItemQuality> OnRemoveFromItemStack;
            public static Action<SCR_Items, int> OnRemoveFromItemStackNoQuality;
            public static Action<SCR_Items, int, ItemQuality> OnAddToItemStack;
            public static Func<Inventory> OnGetInventory;

            public static Action OnUpdateUI;
            public static Action<Inventory> OnGetInventoryData;
            public static Action<bool> ToggleUI;
            public static Action<bool> OnSetAllowanceOfInventory;

            public static Action CreatBag;
            public static Func<int> FindClosestBagID;

            public static Action<InventorySlot> DropItem;
            public static Action<GameObject> OnDeleteBag;
            public static Action OnDeleteAllBags;

            public static Action<int> OnSiloOpened;
            public static Action<InventorySlot, int> OnMoveToSilo;

            public static Action<ItemTypes> onGetTypeOnlyInventory;
            public static Action<PlotBehaviour> OnPlant;
            public static Action<PlotBehaviour> OnFertilize;
            public static Action<FruitTreePlotBehaviour> OnPlantTree;

            public static Action OnSetInventory;
            public static Action OnResetUIInventory;
            public static Action OnOrganise;

            public static Action<InventorySlot, bool> OnOpenInspection;

            public static Action<bool> OnMovingChanged;
            public static Func<InventorySlot> GetMoveablesInventorySlot;
            public static Func<string> GetMoveablesInventoryOrigin;
            public static Func<int> GetMoveablesInventoryID;

            public static Action<InventorySlot, InventorySlot> OnSwapInventorySlots;
            public static Action<InventorySlot, InventorySlot> OnMoveSlots;
            public static Action<InventorySlot, InventorySlot> OnMergeSlotsToAllOfType;
            public static Action<InventorySlot, InventorySlot> OnMergeSlots;

            public static Action<int> OnIncreaseInventorySize;
            public static Func<int> OnGetInventorySize;
            public static Action OnUpdateInventorySize;
        }

        public class BagInventory
        {
            public static Action<SCR_Items, int, ItemQuality, int> OnAddNewItemStack;
            public static Action<InventorySlot, int> OnRemoveItemStackWithSlot;
            public static Action<SCR_Items, int, ItemQuality> OnRemoveItemStack;
            public static Action<SCR_Items, int, int, ItemQuality> OnAddToItemStack;
            public static Action<SCR_Items, int, int, ItemQuality> OnRemoveFromItemStack;
            public static Action<SCR_Items, int, ItemQuality, int> OnSplit;
            public static Action<int> OnOpenBag;

            public static Action<int> OnUpdateUI;
            public static Action<Inventory, int> OnGetInventoryData;
            public static Action<bool, int> ToggleUI;

            public static Action<InventorySlot, bool, int> OnOpenInspection;

            public static Action<bool> OnMovingChanged;
            public static Action<InventorySlot, InventorySlot, int> OnSwapInventorySlots;
            public static Action<InventorySlot, InventorySlot, int> OnMoveSlots;
            public static Action<InventorySlot, InventorySlot, int> OnMergeSlotsToAllOfType;
            public static Action<InventorySlot, InventorySlot, int> OnMergeSlots;
        }

        public class SiloInventory
        {
            public static Action<SCR_Items, int, ItemQuality, int> OnAddNewItemStack;
            public static Action<InventorySlot, int> OnRemoveItemStackWithSlot;
            public static Action<SCR_Items, ItemQuality, int> OnRemoveItemStack;
            public static Action<SCR_Items, int, ItemQuality, int> OnAddToItemStack;
            public static Action<SCR_Items, int, ItemQuality, int> OnRemoveFromItemStack;
            public static Action<SCR_Items, int, ItemQuality, int> OnSplit;
            public static Action<int> OnOpenSilo;

            public static Action<int> OnUpdateUI;
            public static Action<Inventory, int> OnGetInventoryData;
            public static Action<bool, int> ToggleUI;

            public static Action<InventorySlot, bool, int> OnOpenInspectionSilo;
            public static Action<InventorySlot, bool, int> OnOpenInspectionPlayer;

            public static Action<bool> OnMovingChanged;
            public static Action<InventorySlot, InventorySlot, int> OnSwapInventorySlots;
            public static Action<InventorySlot, InventorySlot, int> OnMoveSlots;
            public static Action<InventorySlot, InventorySlot, int> OnMergeSlotsToAllOfType;
            public static Action<InventorySlot, InventorySlot, int> OnMergeSlots;
        }

    }

    public class FruitTrees
    {
        public static Action<FruitPlotState> OnSetPlotState;
    }
    
    public class IslandSystem
    {
        public static Action<int> OnBuyIsland;
        public static Action<string, string, int, int> OnSetUI;
        public static Action<bool> OnToggleUI;

        public class Teleportation
        {
            public static Action<Vector3, string> OnAddTeleportPos;
            public static Action OnResetTeleportPos;
            public static Action<Vector3> OnTeleportPlayer;
            public static Action OnSetDropdownUI;
            public static Action<bool> OnToggleUI;
        }
    }

    public class ModelViewer
    {
        public static Action<GameObject> OnChangeModel;
        public static Action<bool> OnAllowInspector;
    }

    public class PlayerControls
    {
        public static Action<Vector3> OnSnapCamToPlayer;
        public static Action<float> OnChangePlayerRotation;
        public static Action<bool> OnRemovePlayerControl;
        public static Action<Vector3> OnSetNewSpawnPos;
        public static Action<bool> OnToggleSliderUI;
    }

    public class PlayerCustomisation
    {
        public static Action<bool> OnToggleUI;
    }
    
    public class SaveSystem
    {
        public static Func<int> OnSaveTimeData;
        public static Func<CentralPlotSaveDataContainer> OnSavePlotData;

        public static Action OnDataLoaded;
    }

    public class SceneManagement
    {
        public static Action<string> OnLoadNewScene;
        public static Action<bool> OnSetNewGame;
    }

    public class Scripts
    {
        public static Action<bool> OnDisableCameraMovement;
        public static Action<bool> OnDisablePlayer;
    }

    public class ShopSystem
    {
        //public static Func<> OnAddRandomItems;
        public static Action<SCR_Items> OnBuyItem;
        public static Action<SCR_Items> OnSellItem;
        public static Action<ShopInventory> OnGetShopInventory;
        public static Action OnResetShop;
        public static Action<bool> ToggleUI;
        public static Action OnUpdateUI;

        public static Action<SCR_Items, int, ItemQuality, int> OnAddNewItemStack;
        public static Action<SCR_Items, ItemQuality, int> OnRemoveItemStack;
        public static Action<SCR_Items, int, ItemQuality, int> OnAddToItemStack;
        public static Action<SCR_Items, int, ItemQuality, int> OnRemoveFromItemStack;
        public static Action<InventorySlot, int> OnRemoveItemStackWithSlot;

        public static Action<int> OnToggleShop;
        public static Action<int> OnUpdateInventoryUI;
        public static Action<Inventory, int> OnSetUpShopUI;

        public static Action<InventorySlot, bool, int> OnOpenInspectionShop;
        public static Action<InventorySlot, bool, int> OnOpenInspectionPlayer;

        public class SpecialShop
        {
            public static Action<SCR_Items, int, ItemQuality, int> OnAddNewItemStack;
            public static Action<SCR_Items, ItemQuality, int> OnRemoveItemStack;
            public static Action<SCR_Items, int, ItemQuality, int> OnAddToItemStack;
            public static Action<SCR_Items, int, ItemQuality, int> OnRemoveFromItemStack;
            public static Action<InventorySlot, int> OnRemoveItemStackWithSlot;

            public static Action<int> OnOpenSpecialShop;
            public static Action OnWarning;
            public static Action<int> OnUpdateUI;
            public static Action<Inventory, int> OnGetInventoryData;
            public static Action<bool, int> ToggleUI;

            public static Action<InventorySlot, bool, int> OnOpenInspectionShop;
            public static Action<InventorySlot, bool, int> OnOpenInspectionPlayer;
        }

    }

    public class TaskSystem
    {
        //public static Func<iExecutable> OnRequestNewTask;
        public static Action<int> OnRequestNewTask;
        public static Action<iExecutable> OnAddNewTask;
        public static Action<Vector3> OnPlayerMoved;
        public static Action OnPlayerStopMovement;
        public static Func<float, IEnumerator> OnWaitForTaskCompletion;
        public static Action OnTaskComplete;
        public static Action OnClearAllTasks;
        public static Action<int> OnRemoveTask;
        public static Action<iExecutable> OnCheckClearTask;
        public static Action<PlayerState> OnSetPlayerState;
        public static Func<List<iExecutable>> OnGetTaskList;

        public static Action OnUpdateUI;
    }

    public class Tent
    {
        public static Action<int> OnBuyTent;
        public static Action<int, int> OnSetupUI;
        public static Action<bool> OnToggleUI;
    }

    public class TimeCycle
    {
        public static Action OnNewDaySetup;
        public static Action OnReadyForDayStart;
        public static Action OnDayStart;
        public static Action OnSleep;
        public static Action OnDayEnd;
        public static Action<bool> OnPauseTimeOnly;
        public static Action OnPause;
        public static Action OnUnpause;
        public static Func<bool> OnGetPaused;

        public static Action OnUpdating;
        public static Action OnUpdated;
        public static Action OnGetHour;
    }

    public class Tutorial
    {
        public static Action OnStartTutorial;
        public static Action<bool> OnFloorPressed;
        public static Action<bool> OnCameraMovement;
        public static Action<bool> OnDoorPressed;
        public static Action<bool> OnPlotClicked;
        public static Action<bool> OnSeedPlanted;
        public static Action<bool> OnNewDay;
        public static Action<bool> OnHarvested;
    }

    public class UI
    {
        public static Action<int> OnTimeChangedMinutes;
        public static Action<int> OnTimeChangedHours;
        public static Action<int> OnDayChanged;
        public static Action<int> OnMoneyChanged;
        public static Action OnDestroyContextualMenu;

        public static Action<bool> OnTogglePauseButton;

        public static Action OnRandomFadeOut;
        public static Action OnRandomFadeIn;
        public static Action OnTransitionFinish;

        public static Action<bool, string, string> OnToggleInputWindowUI;
        public static Action OnConfirmInput;

        public static Action OnPickupNotifDestroyed;
    }

    public class VFX
    {
        public static Action OnOutlineRemove;
    }

    public class WeatherSystem
    {
        public static Action OnChangeWeather;
        public static Action OnWaterAllCrops;
    }

    public class WeeklyTasks
    {
        public static Action<int> OnUpdateUI;
        public static Action<List<PlayerTask>, int> OnGetTaskList;
        public static Action<bool, int> ToggleUI;

        public static Action<int> OnOpenTaskboard;
        public static Action<PlayerTask, int> OnDeleteTask;

        public static Action OnTaskCompleted;
        public static Action<int> OnUpdateTasksCompletedText;
    } 
}
