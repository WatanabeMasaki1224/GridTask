using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _clearPanel;

    public void GameClear()
    {
        _clearPanel.SetActive(true); 
    }
}
