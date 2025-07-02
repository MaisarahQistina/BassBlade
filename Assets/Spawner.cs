using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Spawner : MonoBehaviour
{
    public GameObject[] cubes;
    public Transform[] points;
    public float beat = (60f / 130f) * 2f;
    private float timer;
    private List<GameObject> spawnedCubes = new List<GameObject>();
    private int currentIndex = 0;

    private List<MapNote> mapNotesList = new List<MapNote>();

    // Layer for cubes (make sure this matches your Saber's layer mask)
    public int cubeLayer = 8; // Change this to match your setup

    private void Awake()
    {
        ParseMapData();
    }

    private void ParseMapData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "mapData.txt");

#if UNITY_ANDROID && !UNITY_EDITOR
        StartCoroutine(LoadJsonFromAndroid(filePath));
#else
        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);
            MapData mapData = JsonUtility.FromJson<MapData>(jsonText);
            mapNotesList = mapData.data;
            Debug.Log($"Loaded {mapNotesList.Count} notes from map data");
        }
        else
        {
            Debug.LogError($"Map data file not found at: {filePath}");
        }
#endif
    }

    IEnumerator LoadJsonFromAndroid(string filePath)
    {
        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                string jsonText = www.downloadHandler.text;
                MapData mapData = JsonUtility.FromJson<MapData>(jsonText);
                mapNotesList = mapData.data;
                Debug.Log($"Loaded {mapNotesList.Count} notes from map data");
            }
            else
            {
                Debug.LogError("Failed to load mapData.txt: " + www.error);
            }
        }
    }

    private void SpawnCube(MapNote note)
    {
        // Ensure spawn point index is valid
        int spawnPoints = Mathf.Clamp(note._lineIndex + (note._lineLayer * 4), 0, points.Length - 1);
        int noteType = Mathf.Clamp(note._type == 0 ? 1 : 0, 0, cubes.Length - 1);

        if (points[spawnPoints] == null)
        {
            Debug.LogError($"Spawn point {spawnPoints} is null!");
            return;
        }

        if (cubes[noteType] == null)
        {
            Debug.LogError($"Cube prefab {noteType} is null!");
            return;
        }

        GameObject currentCube = Instantiate(cubes[noteType], points[spawnPoints]);
        currentCube.transform.localPosition = Vector3.zero;
        currentCube.transform.Rotate(transform.forward, 90 * note._cutDirection);

        // Ensure proper collider setup
        BoxCollider boxCollider = currentCube.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = currentCube.AddComponent<BoxCollider>();
        }

        // Set the layer for proper detection
        currentCube.layer = cubeLayer;
        currentCube.tag = "Cube";

        // Setup rigidbody
        Rigidbody rb = currentCube.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = currentCube.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;

        // Ensure the cube has the movement script
        if (currentCube.GetComponent<Cube>() == null)
        {
            currentCube.AddComponent<Cube>();
        }

        spawnedCubes.Add(currentCube);

        Debug.Log($"Spawned cube at point {spawnPoints}, type {noteType}, layer {currentCube.layer}");
    }

    void Start()
    {
        // Verify setup
        if (cubes == null || cubes.Length == 0)
        {
            Debug.LogError("No cube prefabs assigned!");
        }

        if (points == null || points.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
        }

        Debug.Log($"Spawner ready with {mapNotesList.Count} notes to spawn");
    }

    void Update()
    {
        if (currentIndex >= mapNotesList.Count)
            return;

        timer += Time.deltaTime;

        while (currentIndex < mapNotesList.Count && timer >= mapNotesList[currentIndex]._time)
        {
            SpawnCube(mapNotesList[currentIndex]);
            currentIndex++;
        }
    }

    public void ClearSpawnedCubes()
    {
        foreach (GameObject cube in spawnedCubes)
        {
            if (cube != null)
                Destroy(cube);
        }

        spawnedCubes.Clear();
        currentIndex = 0;
        timer = 0f;
    }
}

[System.Serializable]
public class MapData
{
    public List<MapNote> data;
}

[System.Serializable]
public class MapNote
{
    public float _time;
    public int _lineIndex;
    public int _lineLayer;
    public int _type;
    public int _cutDirection;
}