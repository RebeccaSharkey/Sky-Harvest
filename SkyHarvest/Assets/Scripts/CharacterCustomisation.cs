using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Rendering.UI;

/// <summary>
/// Script which changes the material of the player's body parts
/// </summary>
public class CharacterCustomisation : MonoBehaviour, iSaveable
{
    [SerializeField] private GameObject menuToOpen;
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera customisationCam;
    
    [Header("0: Skin // 1: Hair // 2: Eyes // 3: Hats // 4: Clothing")]
    [SerializeField] private SkinnedMeshRenderer[] renderers;

    [SerializeField] private Mesh[] hairVariations;
    
    [SerializeField] private Material[] hairShortMaterials;
    [SerializeField] private Material[] hairLongMaterials;
    [SerializeField] private Material[] hairPonyTailMaterials;

    [SerializeField] private Mesh[] hatVariations;
    [SerializeField] private Material[] farmerHatMaterials;
    [SerializeField] private Material[] capHatMaterials;

    [SerializeField] private Mesh[] clothingVariations;
    [SerializeField] private Material[] overallMaterials;
    [SerializeField] private Material[] tracksuitMaterials;
    
    
    [SerializeField] private Material[] eyesMaterials;
    [SerializeField] private Material[] skinMaterials;

    private int skinID = 0;
    private int eyesID = 0;
    
    private int hairVariationID = 0;
    private int hairMaterialID = 0;

    private int hatVariationID = 0;
    private int hatMaterialID = 0;

    private int clothesVariationID = 0;
    private int clothingMaterialID = 0;

    public void SetEyeMaterial(bool bIsForward)
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");
        if (bIsForward)
        {
            if (eyesID == eyesMaterials.Length - 1)
            {
                eyesID = 0;
                ChangeBodyPart("Eyes", eyesID);
            }
            else
            {
                eyesID++;
                ChangeBodyPart("Eyes", eyesID);
            }
        }
        else
        {
            if (eyesID == 0)
            {
                eyesID = eyesMaterials.Length - 1;
                ChangeBodyPart("Eyes", eyesID);
            }
            else
            {
                eyesID--;
                ChangeBodyPart("Eyes", eyesID);
            }
        }
    }

    public void SetSkinMaterial(bool bIsForward)
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");

        if (bIsForward)
        {
            if (skinID == skinMaterials.Length - 1)
            {
                skinID = 0;
                ChangeBodyPart("Skin", skinID);
            }
            else
            {
                skinID++;
                ChangeBodyPart("Skin", skinID);
            }
        }
        else
        {
            if (skinID == 0)
            {
                skinID =  skinMaterials.Length - 1;
                ChangeBodyPart("Skin", skinID);
            }
            else
            {
                skinID--;
                ChangeBodyPart("Skin", skinID);
            }
        }
    }
    
    public void SetHairVariation(bool bIsForward)
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");

        if (bIsForward)
        {
            if (hairVariationID == hairVariations.Length - 1)
            {
                hairVariationID = 0;
                ChangeHairVariation((hairVariationID));
            }
            else
            {
                hairVariationID++;
                ChangeHairVariation((hairVariationID));
            }
        }
        else
        {
            if (hairVariationID == 0)
            {
                hairVariationID = hairVariations.Length - 1;
                ChangeHairVariation((hairVariationID));
            }
            else
            {
                hairVariationID--;
                ChangeHairVariation((hairVariationID));
            }
        }
    }

    private void ChangeHairVariation(int variationID)
    {
        switch (variationID)
        {
            case 0:
                hairMaterialID = 0;
                renderers[1].sharedMesh = hairVariations[variationID];
                break;
            case 1:
                hairMaterialID = 0;
                renderers[1].sharedMesh = hairVariations[variationID];
                break;
            case 2:
                hairMaterialID = 0;
                renderers[1].sharedMesh = hairVariations[variationID];
                break;
            default:
                Debug.LogWarning("No defined hair variation");
                break;
        }
    }

    public void SetHairColour(bool bIsForward)
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");

        if (bIsForward)
        {
            switch (hairVariationID)
            {
                case 0:
                    if (hairMaterialID == hairShortMaterials.Length - 1)
                    {
                        hairMaterialID = 0;
                        ChangeHairColour(hairMaterialID);
                    }
                    else
                    {
                        hairMaterialID++;
                        ChangeHairColour(hairMaterialID);
                    }
                    break;
                case 1:
                    if (hairMaterialID == hairLongMaterials.Length - 1)
                    {
                        hairMaterialID = 0;
                        ChangeHairColour(hairMaterialID);
                    }
                    else
                    {
                        hairMaterialID++;
                        ChangeHairColour(hairMaterialID);
                    }
                    break;
                case 2:
                    if (hairMaterialID == hairPonyTailMaterials.Length - 1)
                    {
                        hairMaterialID = 0;
                        ChangeHairColour(hairMaterialID);
                    }
                    else
                    {
                        hairMaterialID++;
                        ChangeHairColour(hairMaterialID);
                    }
                    break;
            }
        }
        else
        {
            switch (hairVariationID)
            {
                case 0:
                    if (hairMaterialID == 0)
                    {
                        hairMaterialID = hairShortMaterials.Length - 1;
                        ChangeHairColour(hairMaterialID);
                    }
                    else
                    {
                        hairMaterialID--;
                        ChangeHairColour(hairMaterialID);
                    }

                    break;
                case 1:
                    if (hairMaterialID == 0)
                    {
                        hairMaterialID = hairLongMaterials.Length - 1;
                        ChangeHairColour(hairMaterialID);
                    }
                    else
                    {
                        hairMaterialID--;
                        ChangeHairColour(hairMaterialID);
                    }

                    break;
                case 2:
                    if (hairMaterialID == 0)
                    {
                        hairMaterialID = hairPonyTailMaterials.Length - 1;
                        ChangeHairColour(hairMaterialID);
                    }
                    else
                    {
                        hairMaterialID--;
                        ChangeHairColour(hairMaterialID);
                    }
                    break;
            }
        }
    }

    private void ChangeHairColour(int colourID)
    {
        switch (hairVariationID)
        {
            case 0:
                renderers[1].material = hairShortMaterials[colourID];
                break;
            case 1:
                renderers[1].material = hairLongMaterials[colourID];
                break;
            case 2:
                renderers[1].material = hairPonyTailMaterials[colourID];
                break;
        }
    }

    public void SetHatVariation(bool bIsForward)
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");

        if (bIsForward)
        {
            if (hatVariationID == hatVariations.Length - 1)
            {
                hatVariationID = 0;
                ChangeHatVariation((hatVariationID));
            }
            else
            {
                hatVariationID++;
                ChangeHatVariation((hatVariationID));
            }
        }
        else
        {
            if (hatVariationID == 0)
            {
                hatVariationID = hatVariations.Length - 1;
                ChangeHatVariation((hatVariationID));
            }
            else
            {
                hatVariationID--;
                ChangeHatVariation((hatVariationID));
            }
        }
    }
    
    private void ChangeHatVariation(int variationID)
    {
        switch (variationID)
        {
            case 0:
                hatMaterialID = 0;
                renderers[3].sharedMesh = hatVariations[variationID];
                break;
            case 1:
                hatMaterialID = 0;
                renderers[3].sharedMesh = hatVariations[variationID];
                break;
            case 2:
                hatMaterialID = 0;
                renderers[3].sharedMesh = hatVariations[variationID];
                break;
            default:
                Debug.LogWarning("No defined hat variation");
                break;
        }
    }
    
    public void SetHatColour(bool bIsForward)
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");

        if (bIsForward)
        {
            switch (hatVariationID)
            {
                case 0:
                    if (hatMaterialID == farmerHatMaterials.Length - 1)
                    {
                        hatMaterialID = 0;
                        ChangeHatColour(hatMaterialID);
                    }
                    else
                    {
                        hatMaterialID++;
                        ChangeHatColour(hatMaterialID);
                    }
                    break;
                case 1:
                    if (hatMaterialID == capHatMaterials.Length - 1)
                    {
                        hatMaterialID = 0;
                        ChangeHatColour(hatMaterialID);
                    }
                    else
                    {
                        hatMaterialID++;
                        ChangeHatColour(hatMaterialID);
                    }
                    break;
            }
        }
        else
        {
            switch (hatVariationID)
            {
                case 0:
                    if (hatMaterialID == 0)
                    {
                        hatMaterialID = farmerHatMaterials.Length - 1;
                        ChangeHatColour(hatMaterialID);
                    }
                    else
                    {
                        hatMaterialID--;
                        ChangeHatColour(hatMaterialID);
                    }

                    break;
                case 1:
                    if (hatMaterialID == 0)
                    {
                        hatMaterialID = capHatMaterials.Length - 1;
                        ChangeHatColour(hatMaterialID);
                    }
                    else
                    {
                        hairMaterialID--;
                        ChangeHatColour(hatMaterialID);
                    }
                    break;
            }
        }
    }

    private void ChangeHatColour(int colourID)
    {
        switch (hatVariationID)
        {
            case 0:
                renderers[3].material = farmerHatMaterials[colourID];
                break;
            case 1:
                renderers[3].material = capHatMaterials[colourID];
                break;
        }
    }
    
    public void SetClothingColour(bool bIsForward)
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");

        if (bIsForward)
        {
            switch (clothesVariationID)
            {
                case 0:
                    if (clothingMaterialID == overallMaterials.Length - 1)
                    {
                        clothingMaterialID = 0;
                        ChangeClothingColour(clothingMaterialID);
                    }
                    else
                    {
                        clothingMaterialID++;
                        ChangeClothingColour(clothingMaterialID);
                    }
                    break;
                case 1:
                    if (clothingMaterialID == tracksuitMaterials.Length - 1)
                    {
                        clothingMaterialID = 0;
                        ChangeClothingColour(clothingMaterialID);
                    }
                    else
                    {
                        clothingMaterialID++;
                        ChangeClothingColour(clothingMaterialID);
                    }
                    break;
            }
        }
        else
        {
            switch (clothesVariationID)
            {
                case 0:
                    if (clothingMaterialID == 0)
                    {
                        hatMaterialID = overallMaterials.Length - 1;
                        ChangeClothingColour(clothingMaterialID);
                    }
                    else
                    {
                        clothingMaterialID--;
                        ChangeClothingColour(clothingMaterialID);
                    }

                    break;
                case 1:
                    if (clothingMaterialID == 0)
                    {
                        clothingMaterialID = tracksuitMaterials.Length - 1;
                        ChangeClothingColour(clothingMaterialID);
                    }
                    else
                    {
                        clothingMaterialID--;
                        ChangeClothingColour(clothingMaterialID);
                    }
                    break;
            }
        }
    }
    
    private void ChangeClothingColour(int colourID)
    {
        switch (clothesVariationID)
        {
            case 0:
                renderers[4].material = overallMaterials[colourID];
                break;
            case 1:
                renderers[4].material = tracksuitMaterials[colourID];
                break;
        }
    }
    

    private void ResetAllHats()
    {
        foreach (Mesh hat in hatVariations)
        {
            hat.Clear();
        }
    }
    
    public void SetClothingVariation(bool bIsForward)
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");

        if (bIsForward)
        {
            if (clothesVariationID == clothingVariations.Length - 1)
            {
                clothesVariationID = 0;
                ChangeClothesVariation(clothesVariationID);
            }
            else
            {
                clothesVariationID++;
                ChangeClothesVariation(clothesVariationID);
            }
        }
        else
        {
            if (clothesVariationID == 0)
            {
                clothesVariationID = hairVariations.Length - 1;
                ChangeClothesVariation((clothesVariationID));
            }
            else
            {
                clothesVariationID--;
                ChangeClothesVariation((clothesVariationID));
            }
        }
    }
    
    private void ChangeClothesVariation(int variationID)
    {
        switch (variationID)
        {
            case 0:
                clothingMaterialID = 0;
                renderers[4].sharedMesh = clothingVariations[variationID];
                break;
            case 1:
                clothingMaterialID = 0;
                renderers[4].sharedMesh = clothingVariations[variationID];
                break;
            case 2:
                clothingMaterialID = 0;
                renderers[4].sharedMesh = clothingVariations[variationID];
                break;
            default:
                Debug.LogWarning("No defined hair variation");
                break;
        }
    }


    /// <summary>
    /// Checks which body part is passed in and changes the material of the renderer
    /// </summary>
    /// <param name="bodyPart">String passed in to change a particular body part</param>
    /// <param name="partID">Index used to grab an element from a material array</param>
    private void ChangeBodyPart(string bodyPart, int partID)
    {
        switch(bodyPart)
        {
            case "Skin":
                renderers[0].material = skinMaterials[partID];
                break;
            case "Eyes":
                renderers[2].material = eyesMaterials[partID];
                break;
            default:
                Debug.LogWarning("Undefined Body Part");
                break;
        }

        Debug.Log("Test");
    }

    private void OpenUI(bool state)
    {
        menuToOpen.SetActive(state);
        if (state)
        {
            CustomEvents.Scripts.OnDisablePlayer?.Invoke(false);
            CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(false);
            CustomEvents.TimeCycle.OnPauseTimeOnly?.Invoke(false);
            mainCam.enabled = false;
            customisationCam.enabled = true;
            CustomEvents.PlayerControls.OnChangePlayerRotation?.Invoke(0f);
        }
        else
        {
            CustomEvents.Scripts.OnDisablePlayer?.Invoke(true);
            CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(true);
            CustomEvents.TimeCycle.OnPauseTimeOnly?.Invoke(true);
            mainCam.enabled = true;
            customisationCam.enabled = false;
            CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Waiting);
            CustomEvents.PlayerControls.OnChangePlayerRotation?.Invoke(0f);
        }
    }

    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();
        
        data.Add(skinID.ToString());
        data.Add(eyesID.ToString());
        data.Add(hairVariationID.ToString());
        data.Add(hairMaterialID.ToString());
        data.Add(hatVariationID.ToString());
        data.Add(hatMaterialID.ToString());
        data.Add(clothesVariationID.ToString());
        data.Add(clothingMaterialID.ToString());

        return data;
    }

    public void LoadData(SerializableList _data)
    {
        ChangeBodyPart("Skin", int.Parse(_data[0]));
        ChangeBodyPart("Eyes", int.Parse(_data[1]));
        ChangeHairVariation(int.Parse(_data[2]));
        ChangeHairColour(int.Parse(_data[3]));
        ChangeHairVariation(int.Parse(_data[4]));
        ChangeHatColour(int.Parse(_data[5]));
        ChangeClothesVariation(int.Parse(_data[6]));
        ChangeClothingColour(int.Parse(_data[7]));
    }

    private void OnEnable()
    {
        CustomEvents.PlayerCustomisation.OnToggleUI += OpenUI;
        
    }

    private void OnDisable()
    {
        CustomEvents.PlayerCustomisation.OnToggleUI -= OpenUI;
    }
}
