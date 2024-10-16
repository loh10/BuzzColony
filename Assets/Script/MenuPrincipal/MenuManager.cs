using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject startMenu, parameterMenu;
    private string seedEnter;
    private int seed;

    public void Play()
    {
        SaveAndLoad.Instance.DeleteSaveFile();
        SaveAndLoad.Instance.SaveMap(seed);
        SaveAndLoad.Instance.SaveGame();
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    #region Open and Close Menu function

    public void OpenMenuStart()
    {
        startMenu.SetActive(!startMenu.activeSelf);
    }

    public void OpenMenuParameter()
    {
        parameterMenu.SetActive(!parameterMenu.activeSelf);
    }

    #endregion


    public void seedInput(string input)
    {
        seedEnter = input;
        SeedConverter seedConverter = new SeedConverter();
        seed = seedConverter.SeedConvertion(seedEnter);
    }
}