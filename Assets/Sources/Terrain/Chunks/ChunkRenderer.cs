using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ChunkRenderer : MonoBehaviour
{
    [SerializeField]
    private BlockAssetProvider _assetProvider;

    private MeshRenderer _meshRenderer;

    private MeshFilter _meshFilter;

    public void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();
        gameObject.layer = LayerMask.NameToLayer(Constants.TerrainLayer);
    }

    public void Render(Chunk chunk)
    {
        var chunkMesh = CreateMesh(chunk);

        chunkMesh.Optimize();
        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();
        
        this.UseMeshFilterFor(chunkMesh);
        this.UseMeshColliderFor(chunkMesh);
    }

    private Mesh CreateMesh(Chunk chunk)
    {
        var vertices = new List<Vector3>();
        var subMeshes = new Dictionary<BlockAsset, ICollection<int>>();
        var uvs = new List<Vector2>();

        foreach (var block in chunk)
        {
            var asset = _assetProvider.GetAsset(block);
            var sides = asset.GetSides(block);
            
            foreach (var side in sides)
            {
                var blockVertices = side.GetVertices(block.LocalPosition);
                vertices.AddRange(blockVertices);

                var triangles = new List<int>
                {
                    vertices.Count - 4,
                    vertices.Count - 3,
                    vertices.Count - 2,
                    vertices.Count - 3,
                    vertices.Count - 1,
                    vertices.Count - 2
                };

                if (!subMeshes.TryAdd(asset, triangles))
                {
                    subMeshes[asset].AddRange(triangles);
                }

                var sideUvs = asset.GetUVs(side);
                uvs.AddRange(sideUvs);
            }
        }

        var chunkMesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = subMeshes.SelectMany(subMesh => subMesh.Value).ToArray(),
            uv = uvs.ToArray(),
            subMeshCount = subMeshes.Count
        };

        var textures = new List<Material>(capacity: chunkMesh.subMeshCount);
        var nextTrianglesStartIndex = 0;
        var subMeshIndex = 0;
        
        foreach (var subMesh in subMeshes)
        {
            var trianglesCount = subMesh.Value.Count;
            
            var descriptor = new SubMeshDescriptor(nextTrianglesStartIndex, trianglesCount);
            chunkMesh.SetSubMesh(subMeshIndex, descriptor);
            textures.Add(subMesh.Key.Material);
            
            nextTrianglesStartIndex += trianglesCount;
            subMeshIndex++;
        }

        _meshRenderer.materials = textures.ToArray();

        return chunkMesh;
    }
}
