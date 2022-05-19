using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

/// <summary>
/// Manager used to keep track and control the weather
/// </summary>
public class WeatherManager : MonoBehaviour
{
    [SerializeField] private string[] weatherTypes;
    [SerializeField] private Material rainMaterial;
    private string currentWeatherType;
    private int numOfWeatherTypes;

    private ParticleSystem weatherSystem;

    [SerializeField] private bool bWeatherCanRun = true;

    private void Start()
    {
        weatherSystem = GetComponent<ParticleSystem>();
        currentWeatherType = weatherTypes[0];
        StartWeather();
    }

    /// <summary>
    /// Checks to see if the weather has any other effects
    /// Instantiates the current weather type FXs + deletes them when the timer is up
    /// </summary>
    private void StartWeather()
    {
        if (bWeatherCanRun)
        {
            if (weatherSystem != null)
            {
                weatherSystem.Play();
            }

        }
    }

    /// <summary>
    /// Grabs the particle system of the current weather playing and stops it from playing
    /// </summary>
    private void StopWeather()
    {
        weatherSystem.Stop();
    } 
    
    /// <summary>
    /// Creates an index from 0 to the size of the weather list
    /// Grabs a weather element from the list using the indexer
    /// Checks each type and calls the StartWeather() method accordingly
    /// </summary>
    private void ChangeWeatherType()
    {
        StopWeather();
        int weatherIndex = Random.Range(0, weatherTypes.Length);
        currentWeatherType = weatherTypes[weatherIndex];

        switch (currentWeatherType)
        {
            case "Clear":
                StopWeather();
                break;
            case "Raining":
                GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().material = rainMaterial;
                StartWeather();
                break;
            default:
                Debug.LogWarning("Unhandled weather type");
                break;
        }
    }

    private void OnEnable()
    {
        CustomEvents.WeatherSystem.OnChangeWeather += ChangeWeatherType;
    }

    private void OnDisable()
    {
        CustomEvents.WeatherSystem.OnChangeWeather -= ChangeWeatherType;
    }
}
