using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    /// <summary>
    /// Trinty 1.0
    /// Level hazýrlama iþlemleri ARTIK KULLANILMIYOR GLOBAL ÝÇERÝSÝNDE TANIMLI
    /// </summary>

    [SerializeField] public static GameObject kayaPrefab; // Prefab for rocks
    public static GameObject duvarPrefab; // Prefab for boxes
    public static GameObject kutuPrefab; // Prefab for boxes
    public static GameObject canavarPrefab; // Prefab for monsters
    public static GameObject oyuncuPrefab; // Prefab for player
    public static Terrain zemin; 

    void Start()
    {
        
    }

    public static void PrepareNewLevel()
    {
        //if (global.map != null) Destroy(global.map);
        // Create a map array with the given dimensions
        global.map = new global.Nesne[global.en, global.boy];
    }
    public static void GenerateLevel()
    {

        // Terrain Büyüklüðü ayarlanýyor geniþlik katsayýsýna göre
        zemin.terrainData.size = new Vector3(global.en*global.genislikKatsayisi, 0, global.boy * global.genislikKatsayisi);
        Debug.Log("Harita oluþturuldu :");
        // Place objects on the map
        //PlaceObjectsOnMap(kayaPrefab, 60);

        // Kayalar haritaya yerleþtiriliyor
        for (int i = 1; i< global.en-1; i+=2)
            for (int j = 1; j< global.boy-1; j+=2)
            {
                global.map[i, j] = global.Nesne.Kaya;
                PlaceObject(kayaPrefab, i, j);
            }

        
        PlaceObjectsOnMap(kutuPrefab, 45);          // Kutular oluþturuluyor
        PlaceObjectsOnMap(canavarPrefab, 20);       // Canavarlar oluþturuluyor

        // Oyuncu karakteri yerleþtiriliyor
        PlacePlayerOnMap(oyuncuPrefab);
    }

    static void PlaceObjectsOnMap(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Debug.Log("Nesne Olusturuluyor :" + i.ToString());
            PlaceObject(prefab);

        }
    }

    // Oyuncu oluþturuluyor ve global.Player1 e referansý baðlanýyor.
    static void PlacePlayerOnMap(GameObject prefab)
    {
        // Bir nesne oluþturur
        global.map[0, 0] = global.Nesne.Player;
        global.Player1 = PlaceObject(prefab, 0, 0);
    }

    // Oluþturulan nesnenin referansýný döndererek bir nesne oluþturur
    static GameObject PlaceObject(GameObject prefab)
    {
        // Rastgele bir konum belirle
        GameObject gameobj = null;
        int x, z;
        do    // Dizideki konum boþ olana kadar yeni sayý üret
        {
            x = Random.Range(0, global.en - 1);
            z = Random.Range(0, global.boy - 1);
        } while (global.map[x, z] != global.Nesne.Bos);

        // Prefab ý yerleþtir
        Vector3 position = new Vector3((x + 0.5f) * global.genislikKatsayisi, 0.5f, (z + 0.5f) * global.genislikKatsayisi);
        Debug.Log("Prefab Olusturuluyor :");
        gameobj = Instantiate(prefab, position, Quaternion.identity);
        Debug.Log("Prefab Olusturuldu :");
        global.map[x, z] = global.Nesne.Kutu; // Mark the position as occupied

        return gameobj;
    }

    // Oluþturulan nesnenin referansýný döndererek bir nesne oluþturur
    static GameObject PlaceObject(global.Nesne nesne)
    {
        // Rastgele bir konum belirle
        GameObject gameobj = null;
        GameObject prefab=null;
        int x, z;
        do    // Dizideki konum boþ olana kadar yeni sayý üret
        {
            x = Random.Range(0, global.en - 1);
            z = Random.Range(0, global.boy - 1);
        } while (global.map[x, z] != global.Nesne.Bos);

        // Prefab ý yerleþtir
        Vector3 position = new Vector3((x + 0.5f) * global.genislikKatsayisi, 0.5f, (z + 0.5f) * global.genislikKatsayisi);
        switch (nesne)
        {
            case global.Nesne.Kutu: prefab = kutuPrefab; break;
            case global.Nesne.Player: prefab = oyuncuPrefab; break;
            case global.Nesne.Canavar: prefab = canavarPrefab; break;
            case global.Nesne.Kaya: prefab = kayaPrefab; break;
            case global.Nesne.Duvar: prefab = duvarPrefab; break;
            //case global.Nesne.Kutu: prefab = kutuPrefab; break;
            default: Debug.Log("Nesne Bulunamadý"); break;
        }
        gameobj = Instantiate(prefab, position, Quaternion.identity);
        Debug.Log("Prefab Olusturuldu :");
        global.map[x, z] = nesne; // Mark the position as occupied

        return gameobj;
    }

    // x y konumuna Prefab oluþturur
    static GameObject PlaceObject(GameObject prefab, int x, int y)
    {
        GameObject gameobj = null;
        // Check if the position is empty
        if (global.map[x, y] != global.Nesne.Bos)
        {
            Vector3 position = new Vector3((x + 0.5f) * global.genislikKatsayisi, 0.5f, (y + 0.5f) * global.genislikKatsayisi);
            gameobj = Instantiate(prefab, position, Quaternion.identity);
        }
        return gameobj;
    }
}
