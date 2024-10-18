using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region SINGLETON INSTANCE
    private static SpawnManager _instance;
    public static SpawnManager Instance
    {
        get
        {
            if (_instance == null && !ApplicationQuitting)
            {
                _instance = FindObjectOfType<SpawnManager>();
                if (_instance == null)
                {
                    GameObject newInstance = new GameObject("Singleton_SpawnManager");
                    _instance = newInstance.AddComponent<SpawnManager>();
                }
            }
            return _instance;
        }
    }

    //Checks if the singleton is alive, useful to reference it when the game is about to close down to avoid memory leaks
    public static bool Exists
    {
        get
        {
            return _instance != null;
        }
    }

    public static bool ApplicationQuitting = false;
    protected virtual void OnApplicationQuit()
    {
        ApplicationQuitting = true;
    }
    #endregion

    public float _spawnTime = 3;

    private float _accuTime = 0;

    private void Awake()
    {
        //we want this object to persist when a scene changes
        DontDestroyOnLoad(gameObject);
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SpawnWave();
    }
    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }


    private List<Spawner> _spawnPoints = new List<Spawner>();
    public void RegisterSpawnPoint(Spawner spawnPoint)
    {
        if (!_spawnPoints.Contains(spawnPoint))
            _spawnPoints.Add(spawnPoint);

    }
    public void UnRegisterSpawnPoint(Spawner spawnPoint)
    {
        _spawnPoints.Remove(spawnPoint);
    }
    // Update is called once per frame
    void Update()
    {
        //remove any objects that are null
        _spawnPoints.RemoveAll(s => s == null);

        _accuTime += Time.deltaTime;
        if(_accuTime >= _spawnTime)
        {
            SpawnWave();
            _accuTime = 0;
        }

        /*
        //if you do not know what predicates are: a while loop that 
        //will remove the first null it finds as long as it finds any
        while (_spawnPoints.Remove(null)) { }
        */
    }
    public void SpawnWave()
    {
        foreach (Spawner point in _spawnPoints)
        {
            point.Spawn();
        }
    }
}