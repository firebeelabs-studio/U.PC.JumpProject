using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform ParallaxReferenceTransform;
    [SerializeField] private List<ParallaxBackgroundParts> backgroundParts;
    [SerializeField] private float _offsetForDistance = 3f;
    private bool _isXAxis;

    void Awake()
    {
        
    }

    private void Start()
    {
        _isXAxis = CameraSettings.Instance.Axis == CameraSettings.ParallaxAxis.Horizontal ? true : false;   //if true => horizontal
        LoadBackgroundParts(_isXAxis);
    }

    private void Update()
    {
        Parallax(_isXAxis);
    }
    private void Parallax(bool isXAxis)
    {
        float parallaxAxis = isXAxis == true ? ParallaxReferenceTransform.position.x : ParallaxReferenceTransform.position.y;
        if (!CameraSettings.Instance.ShouldParallax) return;

        //loops through all elements in list 'backgroundParts' and executes code below for each of them
        foreach (ParallaxBackgroundParts part in backgroundParts)
        {

            //calculates the temporary position of the part depending on parallax effect power relating to the camera position
            float tempPosOfBgPart = parallaxAxis * (1 - part.ParallaxEffectPower);
            //calculates distance which background part will move
            float distance = parallaxAxis * part.ParallaxEffectPower;
            if (isXAxis)
            {
                //moves each part in x axis depending on it's starting position and distance
                part.BackgroundPart.transform.position = new Vector3(part.StartPosX + distance, part.StartPosY, part.BackgroundPart.transform.position.z);
            }
            else
            {
                part.BackgroundPart.transform.position = new Vector3(part.BackgroundPart.transform.position.x, part.StartPosX + distance, part.BackgroundPart.transform.position.z);
            }
            //if temp pos value is bigger than the starting pos + half of length of sprite teleports the part to make it looping
            if (tempPosOfBgPart > part.StartPosX + part.Length / 2)
            {
                part.StartPosX += part.Length;
            }
            //same as above but in opposite direction
            else if (tempPosOfBgPart < part.StartPosX - part.Length)
            {
                part.StartPosX -= part.Length;
            }
        }
    }
    private void LoadBackgroundParts(bool isXAxis)
    {
        foreach (ParallaxBackgroundParts part in backgroundParts)
        {
            if (isXAxis)
            {
                part.Length = part.BackgroundPart.GetComponent<SpriteRenderer>().bounds.size.x;
                part.StartPosX = part.BackgroundPart.transform.position.x;
                part.StartPosY = part.BackgroundPart.transform.position.y;
            }
            else
            {
                part.Length = part.BackgroundPart.GetComponent<SpriteRenderer>().bounds.size.y;
                part.StartPosX = part.BackgroundPart.transform.position.y;
            }
        }
    }
}