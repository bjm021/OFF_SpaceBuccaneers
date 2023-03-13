using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Unity.Transforms;
using UnityEngine.UIElements;

public class Testing : MonoBehaviour
{

    [SerializeField]
    private Mesh mesh;
    [SerializeField]
    private Material material;
    
    private void Start()
    {
        Debug.Log("Hello World!");
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        var entities = new NativeArray<Entity>(100, Allocator.Temp);
        
        var archetype = entityManager.CreateArchetype(
            typeof(RenderMesh),
            typeof(WorldTransform),
            typeof(LocalToWorld),
            typeof(WorldToLocal_Tag));

            entityManager.CreateEntity(archetype, entities);
        
        foreach (var t in entities)
        {
            entityManager.SetSharedComponentManaged(t, new RenderMesh
            {
                mesh = mesh,
                material = material
            });

            entityManager.SetComponentData(t, new WorldTransform
            {
                Position = new float3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0),
                Scale = 1
            });
            
            entityManager.SetComponentData(t, new LocalToWorld
            {
                Value = new float4x4(1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1)
            });
            


            Debug.Log(" Entity Created!");
            
        }
        
        entities.Dispose();
    }
}
