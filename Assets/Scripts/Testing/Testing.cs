using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Collections;


public class Testing : MonoBehaviour
{
    [SerializeField]
    private Mesh mesh;
    [SerializeField]
    private Material material;
    // Start is called before the first frame update
    void Start()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(LevelComponent),
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(MoveSpeedComponent)
        );
        NativeArray<Entity> entityArray = new NativeArray<Entity>(10, Allocator.Temp);
        entityManager.CreateEntity(entityArchetype,entityArray);
        for(int i = 0; i < entityArray.Length; i++){
            Entity entity = entityArray[i];
            entityManager.SetComponentData<LevelComponent>(entity, new LevelComponent{ level = UnityEngine.Random.Range(10,20) });

            entityManager.SetComponentData<MoveSpeedComponent>(entity, new MoveSpeedComponent{ moveSpeed = UnityEngine.Random.Range(2f,4f) });

            entityManager.SetSharedComponentData(entity, new RenderMesh{
                mesh = mesh,
                material = material
            });

            entityManager.SetComponentData<Translation>(entity, new Translation{
                Value = new float3(UnityEngine.Random.Range(-2f,2f), UnityEngine.Random.Range(-4f,4f),0f)
            });
        }
        entityArray.Dispose();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
           // Debug.Log("Click");
            ScreenPointTest.setScreenPos();
        }
    }
}
