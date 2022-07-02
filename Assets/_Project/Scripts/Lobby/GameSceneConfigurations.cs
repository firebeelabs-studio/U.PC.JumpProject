using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class GameSceneConfigurations : MonoBehaviour
{
    //ignore this atm
    [SerializeField] private LocalPhysicsMode _physicsMode = LocalPhysicsMode.Physics3D;
    public virtual LocalPhysicsMode PhysicsMode { get { return _physicsMode; } }
    
    //scenes to load
    [SerializeField] private Object[] _scenes = new Object[0];
    //same but for builds
    [SerializeField, HideInInspector] private string[] _sceneNames = new string[0];

    private void OnValidate()
    {
        List<string> additives = new List<string>();
        if (_scenes is not null)
        {
            foreach (Object item in _scenes)
            {
                if (item is not null)
                {
                    additives.Add(item.name);
                }
            }
        }
        _sceneNames = additives.ToArray();
    }

    public virtual string[] GetGameScenes()
    {
        return _sceneNames;
    }
}
