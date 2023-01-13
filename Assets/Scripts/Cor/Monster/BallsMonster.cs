using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BallsMonster : MonoBehaviour
{
    [Header("Balls")]
    [SerializeField] List<GameObject> currencyBalls = new List<GameObject>();
    [SerializeField] private int needAmmountBalls;
    [SerializeField] private int ammountBalls;

    [Header("NeedCharacter")]
    [SerializeField] CharacterColorType _characterColorType;

    [Header("MonsterCanvas")]
    [SerializeField] GameObject monsterCanvas;
    [SerializeField] TextMeshProUGUI textCountBalls;

    public bool IsTrueCharacter(CharacterColorType characterColorType)
    {
        if (_characterColorType == characterColorType)
            return true;

        return false;
    }

    public bool IsFullMonster()
    {
        if (ammountBalls >= needAmmountBalls)
            return true;

        return false;
    }

    private void Start()
    {
        needAmmountBalls = currencyBalls.Count;
        textCountBalls.text = ammountBalls + "/" + needAmmountBalls;
    }

    public void BallActiveted()
    {
        ammountBalls++;

        if (ammountBalls >= needAmmountBalls)
        {
            monsterCanvas.transform.DOScale(0, 0.3f);
            return;
        }

        textCountBalls.text = ammountBalls + "/" + needAmmountBalls;
        currencyBalls[ammountBalls - 1].SetActive(true);
    }
}
