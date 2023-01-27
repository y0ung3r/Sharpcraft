using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class PhysicalItem : Item, ICleanup
{
    private MeshRenderer _meshRenderer;

    private Rigidbody _rigidbody;

    public void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected override void OnRender(ItemAsset asset)
    {
        var mesh = CreateMesh(asset, new List<BlockSide>
        {
            new MiddleBlockSide(),
            new BackMiddleBlockSide()
        });

        mesh.Optimize();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        this.UseMeshFilterFor(mesh);
        this.UseMeshColliderFor(mesh);
    }

    private Mesh CreateMesh(ItemAsset asset, List<BlockSide> sides)
    {
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var uvs = new List<Vector2>();

        foreach (var side in sides)
        {
            var sideVertices = side.GetVertices(Vector3Int.zero);
            vertices.AddRange(sideVertices);

            triangles.AddRange(new List<int>
            {
                vertices.Count - 4,
                vertices.Count - 3,
                vertices.Count - 2,
                vertices.Count - 3,
                vertices.Count - 1,
                vertices.Count - 2
            });

            var sideUvs = side.GetUVs();
            uvs.AddRange(sideUvs);
        }

        var mesh = new Mesh()
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            uv = uvs.ToArray()
        };

        _meshRenderer.material = asset.Material;

        return mesh;
    }

    public void SetPosition(Vector3 worldPosition)
        => transform.position = worldPosition;

    public void Push(Vector3 direction)
        => _rigidbody.AddForce(direction, ForceMode.Impulse);

    public void Release()
        => Destroy(gameObject);
}