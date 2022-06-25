using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private GameObject _cam;
    [SerializeField] private List<ParallaxBackgroundParts> backgroundParts;
    [SerializeField] private float _offsetForDistance = 3f;

    void Awake()
    {
        foreach (ParallaxBackgroundParts part in backgroundParts)
        {
            part.Length = part.BackgroundPart.GetComponent<SpriteRenderer>().bounds.size.x;
            part.StartPos = part.BackgroundPart.transform.position.x;
        }
    }
    private void FixedUpdate()
    {
        if (!CameraSettings.Instance.ShouldParallax) return;
        foreach (ParallaxBackgroundParts part in backgroundParts)       //loops through all elements in list 'backgroundParts' and executes code below for each of them
        {
            float tempPosOfBgPart = _cam.transform.position.x * (1 - part.ParallaxEffectPower);       //calculates the temporary position of the part depending on parallax effect power relating to the camera position
            float distance = _cam.transform.position.x * part.ParallaxEffectPower - _offsetForDistance;        //calculates distance which background part will move
            part.BackgroundPart.transform.position = new Vector3(part.StartPos + distance, part.BackgroundPart.transform.position.y, part.BackgroundPart.transform.position.z);     //moves each part in x axis depending on it's starting position and distance
            if (tempPosOfBgPart > part.StartPos + part.Length/2)        //if temp pos value is bigger than the starting pos + half of length of sprite teleports the part to make it looping
            {
                part.StartPos += part.Length;
            }
            else if (tempPosOfBgPart < part.StartPos - part.Length)     //same as above but in opposite direction
            {
                part.StartPos -= part.Length;
            }
        }
    }
}