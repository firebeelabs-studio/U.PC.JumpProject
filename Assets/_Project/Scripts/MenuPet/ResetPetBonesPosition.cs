using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPetBonesPosition : MonoBehaviour
{
    [SerializeField] private List<Transform> _bones = new();
    [SerializeField] private PetMenuInteraction _petInteraction;
    private List<BonesWithOrigins> _bonesWithOrigins = new();
    private struct BonesWithOrigins
    {
        public Transform Transform { get; set; }
        public Vector3 Origin { get; set; }

        public BonesWithOrigins(Transform transform, Vector3 origin)
        {
            Transform = transform;
            Origin = origin;
        }
    }
    private void Start()
    {
        foreach (Transform bone in _bones)
        {
            _bonesWithOrigins.Add(new(bone, bone.position));
        }
    }
    private void OnEnable()
    {
        _petInteraction.ToggleIdle(false);
        foreach (var bone in _bonesWithOrigins)
        {
            bone.Transform.position = bone.Origin;
        }
    }
    private void OnDisable()
    {
        _petInteraction.ToggleIdle(true);
    }
}
