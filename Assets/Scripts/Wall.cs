using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RolePlayOverlord
{
    public class Wall : MonoBehaviour
    {
        [HideInInspector]
        public MeshRenderer Renderer;

        void Awake()
        {
            Renderer = GetComponent<MeshRenderer>();
            AlignUV();
        }

        // IMPORTANT(SpectatorQL): Only works with Unity's default cube GameObjects.
        // NOTE(SpectatorQL): Changes the UVs so that the negative-Z-facing side
        // of the cube is aligned correctly.
        void AlignUV()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();

            Vector2[] uvs = meshFilter.sharedMesh.uv;
            uvs[6] = new Vector2(0, 0);
            uvs[7] = new Vector2(1, 0);
            uvs[10] = new Vector2(0, 1);
            uvs[11] = new Vector2(1, 1);

            meshFilter.sharedMesh.uv = uvs;
        }
    }
}
