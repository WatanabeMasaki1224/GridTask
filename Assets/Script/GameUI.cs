using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private TMP_Text _turnText;

    public void SetHP(int currentHP, int maxHP)
    {
        _hpText.text = "HP : " + currentHP + " / " + maxHP;
    }

    public void SetTurn(int turn)
    {
        _turnText.text = "Turn : " + turn;
    }
}
