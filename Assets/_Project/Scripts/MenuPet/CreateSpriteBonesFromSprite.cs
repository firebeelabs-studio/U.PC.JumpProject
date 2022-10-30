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

public class CreateSpriteBonesFromSprite : MonoBehaviour
{
    [SerializeField] private Sprite _spriteWithBones;
    [SerializeField] private int _nativeArraySize;

    //it has to be used only once per sprite, there is no point in doing it over and over run time
    //UPDATE: unity is broken af and sometimes sprites ain't working, so we have to redo this runtime xD
    [ContextMenu("SetBones")]
    public void CreateBones()
    {
        NativeArray<BoneWeight> boneWeights = new NativeArray<BoneWeight>(_nativeArraySize, Allocator.Temp); 
        SpriteBone[] spriteBones = _spriteWithBones.GetBones();
        NativeArray<Matrix4x4> bindPoses = _spriteWithBones.GetBindPoses();
        _spriteWithBones.GetVertexAttribute<BoneWeight>(VertexAttribute.BlendWeight).CopyTo(boneWeights);
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        sprite.SetBones(spriteBones);
        sprite.SetBindPoses(bindPoses);
        sprite.SetVertexAttribute<BoneWeight>(VertexAttribute.BlendWeight, boneWeights);
        boneWeights.Dispose();

        //note to future myself: DON'T FUCK WITH VERTEXATTRIBUTE.POSITION
    }
    
}
