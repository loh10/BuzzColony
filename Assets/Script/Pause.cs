using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject PauseMenu;

    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
    }
  
    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }

    public void Quit()
    {
        SaveAndLoad.Instance.SaveGame();
        SceneManager.LoadScene(0);
    }

}
