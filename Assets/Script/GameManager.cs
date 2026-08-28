using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _clearPanel;
    [SerializeField] private GameObject _gameOverPanel;

    public void GameClear()
    {
        _clearPanel.SetActive(true); 
    }

    public void GameOver()
    {
        _gameOverPanel.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
