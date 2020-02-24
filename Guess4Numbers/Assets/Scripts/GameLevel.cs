
using UnityEngine;
using UnityEngine.EventSystems;


public class GameLevel : MonoBehaviour
{
    public GameObject[] LevelButtons;
    public static string gameLevel;

    public void GetLevel()
    {
        gameLevel = EventSystem.current.currentSelectedGameObject.name;
    }

    public void Quit()
    {
        Application.Quit();
    }



}
