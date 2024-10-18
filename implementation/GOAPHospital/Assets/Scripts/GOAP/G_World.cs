using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//We only need one world. This is a singleton

public sealed class G_World
{
    private static readonly G_World _instance = new G_World();
    private static WorldStates _world;
    private static Queue<GameObject> _patients;
    private static Queue<GameObject> _cubicles;

    static G_World()
    {
        _world = new WorldStates();
        _patients = new Queue<GameObject>();
        _cubicles = new Queue<GameObject>();

        GameObject[] tempCubicleArr = GameObject.FindGameObjectsWithTag("Cubicle");
        foreach(GameObject tempCubicle in tempCubicleArr)
            _cubicles.Enqueue(tempCubicle);

        if (tempCubicleArr.Length > 0)
            _world.ModifyState("FreeCubicle", tempCubicleArr.Length);

        //Time.timeScale = 5;
    }

    private G_World() 
    { 
    }

    public void AddPatient(GameObject patient)
    {
        _patients.Enqueue(patient);
    }

    public GameObject DequeuePatient()
    {
        if (_patients.Count == 0) return null;
        return _patients.Dequeue();
    }

    public void AddCubicle(GameObject cubicle)
    {
        _cubicles.Enqueue(cubicle);
    }

    public GameObject DequeueCubicle()
    {
        if (_cubicles.Count == 0) return null;
        return _cubicles.Dequeue();
    }

    public static G_World Instance
    { 
        get { return _instance; } 
    }

    public WorldStates GetWorld()
    {
        return _world;
    }
}
