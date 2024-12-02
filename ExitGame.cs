using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{   
    public Button exitButton;
    // Bu metot oyunu kapatır
    public void QuitGame()
    {
        // Editor'de test ederken çalışmasını sağlar
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Oyunu kapatır
        Application.Quit();
#endif
    }


    public void Start()
    {
        exitButton.onClick.AddListener(QuitGame);
    }
}