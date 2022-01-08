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


    public GameObject targetMainCamera;
    public Material[] skys;
    public bool isCycle;



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
            //targetMainCamera.GetComponent<Skybox>().material = skys[1];
        }
        else if(lightOfSun.intensity > 0 && !lightOfSun.gameObject.activeInHierarchy)
        {
            lightOfSun.gameObject.SetActive(true);
            //targetMainCamera.GetComponent<Skybox>().material = skys[0];
        }

        if (lightOfMoon.intensity == 0 && lightOfMoon.gameObject.activeInHierarchy)
        {
            lightOfMoon.gameObject.SetActive(false);
            //targetMainCamera.GetComponent<Skybox>().material = skys[0];
        }
        else if (lightOfMoon.intensity > 0 && !lightOfMoon.gameObject.activeInHierarchy)
        {
            lightOfMoon.gameObject.SetActive(true);
            //targetMainCamera.GetComponent<Skybox>().material = skys[1];
        }

        ChangeCycle();

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        //RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
    }

    void ChangeCycle()
    {
        // from 0.15 to 0.65, from 0.65 to 0.8, from 0.8 to 0.15
        if (time >= 0.15f && time <= 0.65f) targetMainCamera.GetComponent<Skybox>().material = skys[0];
        if (time > 0.65f && time <= 0.8f) targetMainCamera.GetComponent<Skybox>().material = skys[1];
        if (time > 0.8f || time < 0.15f) targetMainCamera.GetComponent<Skybox>().material = skys[2];
    }

}
