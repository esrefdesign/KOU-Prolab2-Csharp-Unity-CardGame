using TMPro; // TextMeshPro kütüphanesini ekleyin
using UnityEngine;

public class LevelDisplay : MonoBehaviour
{
    public TMP_Text levelText; // TextMeshPro nesnesine referans
    public InputFieldLogger startingLevel; // Başlangıç seviyesi

    // Bu metot, başlangıç seviyesini TextMeshPro'ya bastırır
    public void UpdateLevelDisplay()
    {
        if (levelText != null)
        {
            levelText.text = startingLevel.ToString();
        }
        else
        {
            Debug.LogError("TextMeshPro referansi atanmadi!");
        }
    }
}
