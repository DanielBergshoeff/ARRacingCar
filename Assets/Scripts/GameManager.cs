using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameMode {
        Placement,
        Racing
    }

    [Header("Car Placement")]
    public GameObject TargetPlacement;
    public GameObject StartPrefab;

    public static Car car;

    [Header("Light Spawner")]
    public GameObject LightSpawnerPrefab;
    public float SpawnRate = 0.3f;
    public float SpawnIncreaseRate = 0.03f;
    public float SpawnRange = 5.0f;
    
    private List<GameObject> LightSpawners;
    private float timer = 0f;

    private GameMode currentGameMode;
    private ARSessionOrigin arOrigin;
    private ARRaycastManager raycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;

    void Start()
    {
        LightSpawners = new List<GameObject>();

        if (Application.isEditor) {
            currentGameMode = GameMode.Racing;
            return;
        }

        arOrigin = FindObjectOfType<ARSessionOrigin>();
        raycastManager = FindObjectOfType<ARRaycastManager>();
        currentGameMode = GameMode.Placement;
    }
    
    void Update()
    {
        if (currentGameMode == GameMode.Racing)
            CheckLightSpawner();

        if (Application.isEditor)
            return;

        if (currentGameMode == GameMode.Placement) {
            UpdatePlacementPose();
            UpdatePlacementIndicator();
        }
    }

    public static void HitByLight() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Update the target position placement pose based on a raycast from the middle of the screen
    /// </summary>
    private void UpdatePlacementPose() {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid) {
            placementPose = hits[0].pose;
        }
    }

    /// <summary>
    /// Update the target position placement object based on placement pose
    /// </summary>
    private void UpdatePlacementIndicator() {
        if (placementPoseIsValid) {
            TargetPlacement.SetActive(true);
            TargetPlacement.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);

            if(Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);
                var carObject = Instantiate(StartPrefab, placementPose.position, placementPose.rotation);
                car = carObject.GetComponentInChildren<Car>();
                TargetPlacement.SetActive(false);
                currentGameMode = GameMode.Racing;
            }
        }
        else {
            TargetPlacement.SetActive(false);
        }
    }

    /// <summary>
    /// Updates timer and spawnrate to check if a new light spawner has to be instantiated
    /// </summary>
    private void CheckLightSpawner() {
        SpawnRate += SpawnIncreaseRate * Time.deltaTime;
        timer += Time.deltaTime;
        if(timer > 1 / SpawnRate) {
            SpawnLightSpawner();
            timer = 0f;
        }
    }

    /// <summary>
    /// Instantiate a light spawner at a random position
    /// </summary>
    private void SpawnLightSpawner() {
        GameObject lightSpawner = Instantiate(LightSpawnerPrefab);
        lightSpawner.transform.position = placementPose.position + new Vector3(Random.Range(-SpawnRange, SpawnRange), 0f, Random.Range(-SpawnRange, SpawnRange));
        LightSpawners.Add(lightSpawner);
    }
}
