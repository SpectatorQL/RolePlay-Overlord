using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RolePlayOverlord
{
    public class Wall : MonoBehaviour
    {
        MeshRenderer _renderer;
        int _wallMaterialIndex = -1;

        public void ChangeWallTexture(Texture2D tex)
        {
            _renderer.materials[_wallMaterialIndex].mainTexture = tex;
        }
        
        void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            Material[] mats = _renderer.materials;
            /*
                NOTE(SpectatorQL): This is... an unusual name.
                But I really do need to get the right one.
            */
            string wallMatName = "WallMaterial (Instance)";
            for(int i = 0; i < mats.Length; ++i)
            {
                if(mats[i].name == wallMatName)
                {
                    _wallMaterialIndex = i;
                }
            }
            Utils.Debug.Assert(_wallMaterialIndex != -1);
        }
    }
}
