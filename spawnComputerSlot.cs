using UnityEngine;
using UnityEngine.UI;

public class spawnComputerSlot : MonoBehaviour
{
    public GameObject prefab; // Prefab şablonunuz
    public Transform parentTransform; // Prefablerin ekleneceği Canvas üzerindeki ebeveyn objesi
    public int numberOfElements = 6; // Kaç tane prefab oluşturulacak
    public Button logButton;
    public float spacing = 2f;
    void Start()
    {
        logButton.onClick.AddListener(SpawnPrefabs);
    }

    void SpawnPrefabs()
    {

        for (int i = 0; i < numberOfElements; i++)
        {
            // Prefab'i oluştur
            GameObject newElement = Instantiate(prefab, parentTransform);

            // RectTransform üzerinden pozisyon ayarla
            RectTransform rectTransform = newElement.GetComponent<RectTransform>();
            

            if (rectTransform != null)
            {
                // Pozisyonu ayarla (yan yana dizim için)
                rectTransform.anchoredPosition = new Vector2(i*spacing, 0);
            }
        }
    }
}
