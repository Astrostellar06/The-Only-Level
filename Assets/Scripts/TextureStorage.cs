using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureStorage : MonoBehaviour
{
    public Dictionary<string, Material> materials = new Dictionary<string, Material>();
    void Start()
    {
        int level = int.Parse(gameObject.name.Replace("Level ", "")) - 1;
        string[] nameMaterials = System.IO.Directory.GetFiles("Assets/Resources/Texture/Level" + level, "*.mat");
        for (int i = 0; i < nameMaterials.Length; i++)
        {
            Material material = Resources.Load<Material>(nameMaterials[i].Replace("Assets/Resources/", "").Replace(".mat", "").Replace("\\", "/"));
            materials.Add(material.name, material);
        }
    }
}
