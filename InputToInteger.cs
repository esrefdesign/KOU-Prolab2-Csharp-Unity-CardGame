using UnityEngine;
using TMPro;

public class TMPInputToInteger : MonoBehaviour
{
    public TMP_InputField inputField; // TMP_InputField referansı
    public int intValue;              // Çevrilen int değer

    // Bu metod InputField içeriği değiştiğinde çağrılır
    public void ConvertInputToInt()
    {
        if (int.TryParse(inputField.text, out intValue))
        {
            Debug.Log("Input başarıyla integer'a çevrildi: " + intValue);
        }
        else
        {
            Debug.LogWarning("Geçersiz bir değer girildi, integer'a çevrilemedi!");
        }
    }
}
