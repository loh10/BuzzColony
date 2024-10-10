using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
        public GameObject startMenu;
        public string seedEnter;
        public void Play()
        {
                SceneManager.LoadScene(1);
        }

        public void Quit()
        {
                Application.Quit();
        }

        public void OpenMenuStart()
        {
                startMenu.SetActive(true);
        }

        public void seedInput(string input)
        {
                seedEnter = input;
        }
        
}
