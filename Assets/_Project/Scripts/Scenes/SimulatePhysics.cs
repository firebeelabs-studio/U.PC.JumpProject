using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Managing.Timing;
using UnityEngine;

public class SimulatePhysics : MonoBehaviour
{
    private PhysicsScene2D _physicsScene;
    private TimeManager _tm;

    private void Awake()
    {
        _tm = InstanceFinder.TimeManager;
        _tm.OnPhysicsSimulation += TimeManager_OnPhysicsSimulation;
        _physicsScene = gameObject.scene.GetPhysicsScene2D();
        //Physics2D.autoSimulation = false;
        Physics2D.simulationMode = SimulationMode2D.Script;
    }

    private void TimeManager_OnPhysicsSimulation(float delta)
    {
        _physicsScene.Simulate(delta);
    }

    private void OnDestroy()
    {
        if (_tm is null) return;
        _tm.OnPhysicsSimulation -= TimeManager_OnPhysicsSimulation;
    }
}
