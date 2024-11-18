using UnityEngine;

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
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseMenu.SetActive(!PauseMenu.activeSelf);
    }
}
