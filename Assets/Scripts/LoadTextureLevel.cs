using UnityEngine;
using System.Collections.Generic;

public class LoadTextureLevel : MonoBehaviour
{
    [SerializeField] string[] listTextureName;
    void Start()
    {
        Dictionary<string, Material> materials = gameObject.transform.parent.transform.parent.GetComponent<TextureStorage>().materials;
        Material[] listMaterial = gameObject.GetComponent<MeshRenderer>().materials;
        for (int i = 0; i < listTextureName.Length; i++)
            listMaterial[i] = materials[listTextureName[i] + "_Material"];
        gameObject.GetComponent<MeshRenderer>().materials = listMaterial;
    }
}
