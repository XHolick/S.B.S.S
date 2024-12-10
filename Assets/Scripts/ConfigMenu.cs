using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfigMenu : MonoBehaviour
{
    public void LoadScene(string cena)
    {
        
        SceneManager.LoadScene(cena);
    }


    public void RestartGame(string cena)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Sair do game");
    }
}