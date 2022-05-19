using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fish Object", menuName = "Fish Object")]
public class SO_FishObject : ScriptableObject
{
    [SerializeField] SCR_Items item;
    [HideInInspector] public SCR_Items Item { get => item; set => item = value; }

    [Header("Fishing Data")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float probability;
    [HideInInspector] public float Probability { get => probability; set => probability = value; }
    [Range(15.0f, 35.0f)]
    [SerializeField] private float fishingTime;
    [HideInInspector] public float FishingTime { get => fishingTime; set => fishingTime = value; }
    [Range(0.0f, 1.0f)]
    [SerializeField] private float pullSpeed;
    [HideInInspector] public float PullSpeed { get => pullSpeed; set => pullSpeed = value; }
}
