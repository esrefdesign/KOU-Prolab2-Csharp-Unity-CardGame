using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // Savaş başlatma fonksiyonu, selectedCards parametresi alır
    public void StartBattle(List<Savas_Araclari> selectedCards)
    {
        // Burada savaş işlemleri başlatılır
        Debug.Log($"Savaş başladı! Seçilen Kartlar:");

        foreach (var card in selectedCards)
        {
            // Kartlarla ilgili savaş hesaplamalarını yapabilirsiniz
            Debug.Log($"Kart: {card.AltSinif}, Dayanıklılık: {card.Dayaniklilik}");
        }
    }
}
