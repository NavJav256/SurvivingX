using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{

    [Range(0f, 1f)]
    public float time;
    public float dayLength; // In seconds
    public float startDayTime = 0.4f; // Morning sun
    private float timeRate;
    public Vector3 noon; // Rotation of the sun
    public GameObject targetLight;
    public GameObject targetMainCamera;
    public Material[] skys;

    [Header("Sun settings")]
    public Light lightOfSun;
    public Gradient sunGradientColour;
    public AnimationCurve sunLightIntensity;

    [Header("Moon settings")]
    public Light lightOfMoon;
    public Gradient moonGradientColour;
    public AnimationCurve moonLightIntensity;

    [Header("Other Lighting options")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;

    void Awake() 
    {
    //    targetLight = GameObject.FindGameObjectsWithTag("Light");
        targetMainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Start()
    {
        timeRate = 1f / dayLength;
        time = startDayTime;
    }

    void Update()
    {
        time += timeRate * Time.deltaTime;

        if (time >= 1f)
        {
            time = 0f;
        }

        // Light rotation
        lightOfSun.transform.eulerAngles = (time - 0.25f) * noon * 4f;
        lightOfMoon.transform.eulerAngles = (time - 0.75f) * noon * 4f;

        //Light intensity
        lightOfSun.intensity = sunLightIntensity.Evaluate(time);
        lightOfMoon.intensity = moonLightIntensity.Evaluate(time);

        // Change colours
        lightOfSun.color = sunGradientColour.Evaluate(time);
        lightOfMoon.color = moonGradientColour.Evaluate(time);

        // enabling lights
        if(lightOfSun.intensity == 0 && lightOfSun.gameObject.activeInHierarchy)
        {
            lightOfSun.gameObject.SetActive(false);
            targetMainCamera.GetComponent<Skybox>().material = skys[1];
        }
        else if(lightOfSun.intensity > 0 && !lightOfSun.gameObject.activeInHierarchy)
        {
            lightOfSun.gameObject.SetActive(true);
            targetMainCamera.GetComponent<Skybox>().material = skys[0];
        }

        if (lightOfMoon.intensity == 0 && lightOfMoon.gameObject.activeInHierarchy)
        {
            lightOfMoon.gameObject.SetActive(false);
            targetMainCamera.GetComponent<Skybox>().material = skys[0];
        }
        else if (lightOfMoon.intensity > 0 && !lightOfMoon.gameObject.activeInHierarchy)
        {
            lightOfMoon.gameObject.SetActive(true);
            targetMainCamera.GetComponent<Skybox>().material = skys[1];
        }

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        //RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
    }

}
