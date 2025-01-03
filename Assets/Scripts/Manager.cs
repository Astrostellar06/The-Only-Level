using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance { get; set;}

    public GameObject levelPrefab;
    public IntVar level;
    public GameObject player;
    public GameObject cameraHandler;
    public GameObject respawn;
    public Exit currentExit;
    public int clickCount = 0;
    public Camera mainCamera;
    public GameObject robot;
    public int spikes = 0;
    public Animator spinCamera;
    public GameObject rotationCamera;
    public int deaths = 0;

    private bool canDie = true;
    private string path = "Assets/Resources/Texture/";

    private void Awake()
    {
        // S'assure qu'une seule instance existe
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // si on veut garder ce Manager entre les scènes
    }

    void Start()
    {
        //GenerateTextures();
        //GenerateMaterials();
        level.value = 1;
        DetectExit previousDetectExit = null;
        DetectExit previousPreviousDetectExit = null;
        for (int i = 0; i < 40; i++)
        {
            GameObject newLevel = Instantiate(Manager.instance.levelPrefab, new Vector3(i * 14, 0, 0), Quaternion.identity);
            newLevel.name = "Level " + (i + 1).ToString();
            // Modif
            if (i == 6 || i == 9 || i == 21 || i == 22 || i == 27 || i == 31)
                enableSpecificBlocks(i + 1, newLevel);
            if (i == 22 || i == 25 || i == 31)
                enableSpecificBlocks(10, newLevel);
            if (i + 1 == 10)
                GameObject.Find($"Level 10/Exit").GetComponent<BoxCollider>().enabled = false;
            
            if (i > 2)
                newLevel.SetActive(false);
            if (i > 1)
                previousPreviousDetectExit.nextNextLevel = newLevel;
            if (i != 0)
            {
                previousPreviousDetectExit = previousDetectExit;
                previousDetectExit.nextDetectExit = newLevel.GetComponentInChildren<DetectExit>();
            }
            previousDetectExit = newLevel.GetComponentInChildren<DetectExit>();
        }

        GameObject.Find("Level 2/Exit").GetComponent<Exit>().Open();
        currentExit = GameObject.Find("Level 1/Exit").GetComponent<Exit>();
    }

 
    public void Death()
    {
        if (!canDie)
            return;
        player.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        player.transform.position = respawn.transform.position;
        Physics.SyncTransforms();   // This is super useful -> avoid tp flickering
        if (level.value != 2 && level.value != 15)
            currentExit.Close();
        else if (level.value == 2 || level.value == 15)
            currentExit.Open();
        if (level.value == 6)
            currentExit.transform.parent.GetComponentInChildren<ActivateExit>().StopPush();
        if (level.value == 14)
            player.GetComponent<PlayerControl>().Jump2();
        clickCount = 0;
        deaths++;
        canDie = false;
        Invoke(nameof(ResetDeath), 0.05f);
    }

    private void ResetDeath()
    {
        canDie = true;
    }

    private void enableSpecificBlocks(int level, GameObject parent)
    {
        List<Transform> children = GetAllChilds(parent.transform);
        foreach (Transform child in children)
            if (child.tag == $"Level{level}")
                child.gameObject.SetActive(!child.gameObject.activeInHierarchy);
    }

    private List<Transform> GetAllChilds(Transform _t)
    {
        List<Transform> ts = new List<Transform>();

        foreach (Transform t in _t)
        {
            ts.Add(t);
            if (t.childCount > 0)
                ts.AddRange(GetAllChilds(t));
        }

        return ts;
    }

    private void GenerateTextures()
    {
        // Récupère tous les fichiers PNG dans le répertoire
        string[] fileInfo = Directory.GetFiles(path, "*.png");

        // Boucle pour créer 5 niveaux de textures
        for (int i = 0; i < 40; i++)
        {
            string levelPath = $"{path}Level{i}/";

            // Assure que le dossier pour le niveau existe
            if (!Directory.Exists(levelPath))
            {
                Directory.CreateDirectory(levelPath);
            }

            foreach (string file in fileInfo)
            {
                // Retire "Assets/Resources/" et ".png" pour le chargement avec Resources.Load
                string filePath = file.Replace($"Assets/Resources/", "").Replace(".png", "");
                string fileName = Path.GetFileName(file);

                // Charge la texture originale
                Texture2D originalTexture = Resources.Load<Texture2D>(filePath);
                if (originalTexture == null)
                {
                    Debug.LogError($"Texture not found: {filePath}");
                    continue;
                }

                // Crée une nouvelle texture avec les mêmes dimensions et format
                Texture2D newTexture = new Texture2D(originalTexture.width, originalTexture.height, TextureFormat.RGBA32, false)
                {
                    filterMode = FilterMode.Point,
                    wrapMode = TextureWrapMode.Repeat
                };

                // Copie et modifie les pixels
                Color[] pixels = originalTexture.GetPixels();
                Color color = Color.HSVToRGB(1f / 40 * (i % 40), 1f, 1f);
                for (int j = 0; j < pixels.Length; j++)
                {
                    if (pixels[j] == Color.white) // Exemple: change les blancs en rouge
                    {
                        pixels[j] = color;
                    }
                }
                newTexture.SetPixels(pixels);
                newTexture.Apply();

                // Sauvegarde la texture modifiée en PNG
                byte[] pngData = newTexture.EncodeToPNG();
                string outputPath = $"{levelPath}{fileName}";
                File.WriteAllBytes(outputPath, pngData);
                UnityEditor.AssetDatabase.Refresh();
                // Assigner les paramètres d'import dans Unity
                string assetPath = $"{path}Level{i}/{fileName}";
                UnityEditor.TextureImporter importer = UnityEditor.AssetImporter.GetAtPath(assetPath) as UnityEditor.TextureImporter;
                Debug.Log(assetPath);

                if (importer != null)
                {
                    importer.textureCompression = UnityEditor.TextureImporterCompression.Uncompressed; // Pas de compression
                    importer.filterMode = FilterMode.Point; // Filter Mode: Point
                    importer.maxTextureSize = Mathf.Max(originalTexture.width, originalTexture.height); // Max Size: Taille d'origine
                    importer.npotScale = UnityEditor.TextureImporterNPOTScale.None; // Non-power of 2: None
                    UnityEditor.AssetDatabase.ImportAsset(assetPath, UnityEditor.ImportAssetOptions.ForceUpdate);
                }

                Debug.Log($"Texture saved at: {outputPath}");
            }
        }
        // Rafraîchit l'AssetDatabase pour Unity
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    private void GenerateMaterials()
    {
        for (int i = 0; i < 40; i++)
        {
            string levelPath = $"{path}Level{i}/";
            string[] textureFiles = System.IO.Directory.GetFiles(levelPath, "*.png");

            foreach (string textureFile in textureFiles)
            {
                // Charger la texture
                string texturePath = textureFile.Replace("\\", "/");
                string assetPath = texturePath.Replace(Application.dataPath, "Assets");

                Texture2D texture = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                if (texture == null)
                {
                    Debug.LogWarning($"Texture not found at: {assetPath}");
                    continue;
                }

                // Créer un nouveau Material
                Material newMaterial = new Material(Shader.Find("Standard")); // Utilise un shader Unity standard
                newMaterial.mainTexture = texture; // Associe la texture au Material

                // Sauvegarder le Material dans le dossier correspondant
                string materialName = System.IO.Path.GetFileNameWithoutExtension(textureFile) + "_Material.mat";
                string materialPath = $"{levelPath}{materialName}";
                UnityEditor.AssetDatabase.CreateAsset(newMaterial, materialPath);
                Debug.Log($"Material created at: {materialPath}");
            }
        }
        UnityEditor.AssetDatabase.Refresh(); // Rafraîchir pour afficher les nouveaux assets
    }

}
