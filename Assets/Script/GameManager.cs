using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _clearPanel;
    [SerializeField] private GameObject _gameOverPanel;

    public void GameClear()
    {
        _clearPanel.SetActive(true); 
    }

    public void GamoOver()
    {
        _gameOverPanel.SetActive(true);
    }
}
