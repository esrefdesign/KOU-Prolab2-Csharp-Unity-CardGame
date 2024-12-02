using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SpawnTextOnCanvas : MonoBehaviour
{
    public GameObject textPrefab; // Text Prefab'ini atayacağınız alan
    public Transform canvasTransform; // Canvas'ın Transform'u

    public void SpawnText(string message)
    {
        // Eğer prefab veya canvas atanmadıysa hata mesajı ver
        if (textPrefab == null || canvasTransform == null)
        {
            Debug.LogError("Text prefab veya canvas atanmadı!");
            return;
        }

        // Prefab'i spawnla
        GameObject spawnedText = Instantiate(textPrefab, canvasTransform);

        // Prefab içindeki Text bileşenine eriş ve mesajı ata
        Text textComponent = spawnedText.GetComponent<Text>();
        if (textComponent != null)
        {
            textComponent.text = message;
        }
        else
        {
            Debug.LogError("Prefab içinde Text bileşeni bulunamadı!");
        }

         RectTransform rectTransform = spawnedText.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = Vector2.zero; // Ekranın tam ortasına yerleştir
        }
    }

    // Örnek bir koşul
}
