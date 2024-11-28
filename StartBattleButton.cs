using UnityEngine;
using UnityEngine.UI;

public class StartBattleButton : MonoBehaviour
{
    private spawnPlayerSlot spawnPlayerSlot;  // Kartları yöneten sınıf
    private BattleManager battleManager;      // Savaşı yöneten sınıf
    public Button startButton;

    // Start() içinde referansları alabiliriz
    public void Start()
    {
        // spawnPlayerSlot ve battleManager referanslarını bulalım
        spawnPlayerSlot = FindFirstObjectByType<spawnPlayerSlot>();
        battleManager = FindAnyObjectByType<BattleManager>();

        // Eğer referanslar null ise, hata mesajı göster
        if (battleManager == null)
        {
            battleManager = FindAnyObjectByType<BattleManager>();
        }

        if (battleManager == null)
        {
            Debug.LogError("BattleManager bulunamadı!");
            return;
        }

        // Butona tıklandığında OnBattleStartButtonClicked fonksiyonunun çalışmasını sağla
        startButton.onClick.AddListener(OnBattleStartButtonClicked);
    }

    public void OnBattleStartButtonClicked()
    {
        // Eğer 3 kart seçilmediyse, kullanıcıya mesaj gösterebilirsiniz
        if (spawnPlayerSlot.selectedCards.Count < 3)
        {
            Debug.Log("En az 3 kart seçmelisiniz!");
            return; // Savaş başlatılmıyor
        }

        // Seçilen kartları BattleManager'a gönder
        battleManager.StartBattle(spawnPlayerSlot.selectedCards);
    }
}
