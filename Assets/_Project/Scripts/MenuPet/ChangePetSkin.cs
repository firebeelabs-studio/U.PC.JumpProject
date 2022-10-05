using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;
using UnityEngine.U2D.Animation;

public class ChangePetSkin : MonoBehaviour
{
    [SerializeField] private SpriteResolver _spriteResolver;
    [SerializeField] private Sprite _spriteWithBones;
    [SerializeField] private SpriteBone[] _spriteBones = new SpriteBone[9];
    private NativeArray<Matrix4x4> _bindPoses;
    private NativeSlice<Vector3> _positions = new NativeSlice<Vector3>();
    private NativeArray<Vector3> _positions2;
    private NativeArray<BoneWeight> _boneWeights; 
    private void Awake()
    {
        _positions2 = new NativeArray<Vector3>(48, Allocator.Persistent);
        _boneWeights = new NativeArray<BoneWeight>(48, Allocator.Persistent); 
        _spriteBones = _spriteWithBones.GetBones();
        _bindPoses = _spriteWithBones.GetBindPoses();
        _spriteWithBones.GetVertexAttribute<Vector3>(VertexAttribute.Position).CopyTo(_positions2);
        _spriteWithBones.GetVertexAttribute<BoneWeight>(VertexAttribute.BlendWeight).CopyTo(_boneWeights);
    }

    [ContextMenu("ChangeSkin")]
    public void ChangeSkin()
    {
        _spriteResolver.SetCategoryAndLabel("NewCategory", "Entry_0");
        _spriteResolver.ResolveSpriteToSpriteRenderer();
    }

    [ContextMenu("SetBones")]
    public void Test()
    {
        // SpriteBone[] spriteBones = new[7] ;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        sprite.SetBones(_spriteBones);
        sprite.SetBindPoses(_bindPoses);
        // sprite.SetVertexAttribute<Vector3>(UnityEngine.Rendering.VertexAttribute.Position, _positions2);
        sprite.SetVertexAttribute<BoneWeight>(UnityEngine.Rendering.VertexAttribute.BlendWeight, _boneWeights);
        // //sprite.SetVertexAttribute<Color32>(UnityEngine.Rendering.VertexAttribute.Color, colors);
    }
    
}
