using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int mode; // 1 - 메인 스테이지 2- 미니게임2

    // minigame2
    private List<Card> allCards;
    private Card flippedCard;
    private bool isFlipping = false;
    private int matchesFound = 0;
    private int totalMatches = 10;

    //main
    private int stageHP = 3;
    private int stageStar = 0;
    private Canvas canvas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Don't destroy this when move scenes.
        DontDestroyOnLoad(instance);

    }

    // Start is called before the first frame update
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Level1" || sceneName == "Level1" || sceneName == "Level1")
        {

        } else if (sceneName == "MiniGame2")
        {

        }
    }

    public void forMainGame()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    public void forCardGame()
    {
        Board board = FindObjectOfType<Board>();
        allCards = board.GetCards();

        StartCoroutine("FlipAllCardsRoutine");
    }

    IEnumerator FlipAllCardsRoutine()
    {
        isFlipping = true;
        yield return new WaitForSeconds(0.5f);
        FlipAllCards();
        yield return new WaitForSeconds(3f);
        FlipAllCards();
        yield return new WaitForSeconds(0.5f);
        isFlipping = false;
    }

    void FlipAllCards()
    {
        foreach (Card card in allCards)
        {
            card.FlipCard();
        }
    }

    public void CardClicked(Card card)
    {
        if (isFlipping)
        {
            return;
        }
        card.FlipCard();

        if (flippedCard == null)
        {
            flippedCard = card;
        }
        else
        {
            StartCoroutine(CheckMatchRoutin(flippedCard, card));
        }
    }

    IEnumerator CheckMatchRoutin(Card card1, Card card2)
    {
        isFlipping = true;

        if (card1.cardID == card2.cardID)
        {
            card1.setMatched();
            card2.setMatched();

            matchesFound++;

            if (matchesFound == totalMatches)
            {
                CardGameOver();
            }
        }
        else
        {

            yield return new WaitForSeconds(1f);

            card1.FlipCard();
            card2.FlipCard();

            yield return new WaitForSeconds(0.4f);
        }
        isFlipping = false;
        flippedCard = null;
    }

    void CardGameOver()
    {
        SceneManager.LoadScene("map2");
    }

    public void loseHP()
    {
        Debug.Log("hp " + stageHP);
        if (stageHP < 5 && stageHP > 0)
        {
            stageHP--;
        }
        if (stageHP < 1)
        {
            SceneManager.LoadScene("over");
        }
    }

    public void addHP()
    {
        stageHP++;
    }

    public void addStar()
    {
        stageStar++;
        Debug.Log("star " + stageStar);
    }
    public int getStar()
    {
        return stageStar;
    }

    public int getHp()
    {
        return stageHP;
    }
}