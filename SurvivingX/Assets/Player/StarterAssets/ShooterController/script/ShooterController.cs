using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class ShooterController : MonoBehaviour
{

    [SerializeField]
    private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField]
    private LayerMask ColliderLayerMask = new LayerMask();
    [SerializeField]
    private Transform debugTran;
    [SerializeField]
    private Transform bulletPrefab;
    [SerializeField]
    private Transform spawnBullet;

    
    private float normalSensitivity;
    private float aimSensitivity;

    private StarterAssetsInputs starter;
    private ThirdPersonController third;

    private void Start()
    {
        normalSensitivity = StateController.gameSensitivity;
        aimSensitivity = StateController.aimSensitivity;
    }

    private void Awake()
    {
        starter = GetComponent<StarterAssetsInputs>();
        third = GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        Vector3 mousePosition = Vector3.zero;
        Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(center);
        if (Physics.Raycast(ray, out RaycastHit rayHit, 999f, ColliderLayerMask))
        {
            debugTran.position = rayHit.point;
            mousePosition = rayHit.point;
        }



        if (starter.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            third.setSensitivity(aimSensitivity);
            third.changeRotation(false);
            Vector3 aimTarget = mousePosition;
            aimTarget.y = transform.position.y;
            Vector3 aimDirection = (aimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            third.setSensitivity(normalSensitivity);
            third.changeRotation(true);
        }

        if(starter.shoot)
        {
            Vector3 aimDirection = (mousePosition - spawnBullet.position).normalized;
            Instantiate(bulletPrefab, spawnBullet.position, Quaternion.LookRotation(aimDirection, Vector3.up));
            starter.shoot = false;
        }

    }

    public void changeMouseSensitivity(float sen)
    {
        this.normalSensitivity = sen;
    }

    public void changeAimSensitivity(float sen)
    {
        this.aimSensitivity = sen;
    }

}
