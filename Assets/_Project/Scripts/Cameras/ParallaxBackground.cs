using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public GameObject _parallaxReferenceObject;
    [SerializeField] private List<ParallaxBackgroundParts> backgroundParts;
    [SerializeField] private float _offsetForDistance = 3f;
    private bool _parallaxAxis;

    void Awake()
    {
        _parallaxAxis = CameraSettings.Instance.Axis == CameraSettings.ParallaxAxis.Horizontal ? true : false;   //if true => horizontal
        LoadBackgroundParts(_parallaxAxis);
    }
    private void FixedUpdate()
    {
        Parallax(_parallaxAxis);
    }
    private void Parallax(bool axis)
    {
        float parallaxAxis = axis == true ? _parallaxReferenceObject.transform.position.x : _parallaxReferenceObject.transform.position.y;
        if (!CameraSettings.Instance.ShouldParallax) return;

        //loops through all elements in list 'backgroundParts' and executes code below for each of them
        foreach (ParallaxBackgroundParts part in backgroundParts)
        {

            //calculates the temporary position of the part depending on parallax effect power relating to the camera position
            float tempPosOfBgPart = parallaxAxis * (1 - part.ParallaxEffectPower);
            //calculates distance which background part will move
            float distance = parallaxAxis * part.ParallaxEffectPower;
            if(axis)
            {
                //moves each part in x axis depending on it's starting position and distance
                part.BackgroundPart.transform.position = new Vector3(part.StartPos + distance , part.BackgroundPart.transform.position.y, part.BackgroundPart.transform.position.z);
            }
            else
            {
                part.BackgroundPart.transform.position = new Vector3(part.BackgroundPart.transform.position.x, part.StartPos + distance, part.BackgroundPart.transform.position.z);
            }
            //if temp pos value is bigger than the starting pos + half of length of sprite teleports the part to make it looping
            if (tempPosOfBgPart > part.StartPos + part.Length / 2)
            {
                part.StartPos += part.Length;
            }
            //same as above but in opposite direction
            else if (tempPosOfBgPart < part.StartPos - part.Length)
            {
                part.StartPos -= part.Length;
            }
        }
    }
    private void LoadBackgroundParts(bool axis)
    {
        foreach (ParallaxBackgroundParts part in backgroundParts)
        {
            if (axis)
            {
                part.Length = part.BackgroundPart.GetComponent<SpriteRenderer>().bounds.size.x;
                part.StartPos = part.BackgroundPart.transform.position.x;
            }
            else
            {
                part.Length = part.BackgroundPart.GetComponent<SpriteRenderer>().bounds.size.y;
                part.StartPos = part.BackgroundPart.transform.position.y;
            }
        }
    }
}