using Unity.Entities;
using Unity.Rendering;
using UnityEditor.PackageManager;
using UnityEngine;

public partial class TestSystem : SystemBase
{

    protected override void OnUpdate()
    {
        Entities.ForEach((RenderMesh renderMesh) =>
        {
            renderMesh.material = new Material(renderMesh.material);
        }).WithoutBurst().Run();
    }
}
