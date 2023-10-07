using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopScoreNumbers : MonoBehaviour
{
    private const int INDEX_OF_EACH_SYMBOL = 0;

    [SerializeField] private GameObject[] instancesOfScoreNumbers;

    [SerializeField] private AnimationManager animationSource;

    [SerializeField] private GameObject scoreNumbersParent;

    [SerializeField] private GameLogic logic;

    public List<GameObject> CurrentScoreNumbersList { get; private set; }
    private List<RectTransform> listOfNumbersForScale;

    private string scoreInString = "0";
    private string previousScore;

    private int numberPositionOffset = 110;

    private void Awake()
    {
        listOfNumbersForScale = new List<RectTransform>();
        CurrentScoreNumbersList = new List<GameObject>();
        
        foreach (var number in instancesOfScoreNumbers)
        {
            listOfNumbersForScale.Add(number.GetComponent<RectTransform>());
        }
    }

    private void OnEnable()
    {
        CollisionListener.OnBirdCollided += CollisionChecking_OnBirdCollided;
        CollisionListener.OnBirdTriggered += CollisionChecking_OnBirdTriggered;
        GameLogic.OnGameStarted += LogicScript_OnGameStarted;
    }

    private void OnDisable()
    {
        CollisionListener.OnBirdCollided -= CollisionChecking_OnBirdCollided;
        CollisionListener.OnBirdTriggered -= CollisionChecking_OnBirdTriggered;
        GameLogic.OnGameStarted -= LogicScript_OnGameStarted;
    }

    private void LogicScript_OnGameStarted()
    {
        SetNumberInGame("0");

        previousScore = "0";
    }

    private void CollisionChecking_OnBirdTriggered(string nullStr)
    {
        if (logic.Score == 0)
            DestroyNumber(true, 0);

        scoreInString = logic.Score.ToString();

        int amountOfSymbolsInScore = scoreInString.Length;
        int amountOfSymbolsInPreviousScore = previousScore.Length;

        if (amountOfSymbolsInScore > amountOfSymbolsInPreviousScore)
        {
            for (int i = 0; i < amountOfSymbolsInPreviousScore; i++)
            {
                DestroyNumber(CurrentScoreNumbersList.Count > 0, INDEX_OF_EACH_SYMBOL);
            }

            for (int i = 0; i < amountOfSymbolsInScore; i++)
            {
                SetNumberInGame(scoreInString.Substring(i, 1), numberPositionOffset * i);
            }
        }
        else
        {
            int indexOfRemovedNumber = 0;

            for (int i = 0; i < amountOfSymbolsInScore; i++)
            {
                if (previousScore.Substring(i, 1) != scoreInString.Substring(i, 1))
                {
                    SetNumberInGame(scoreInString.Substring(i, 1), numberPositionOffset * i);

                    // 1 - при рестарте игры создается число (код создания на строку выше) и, чтобы не удалять его,
                    // нужно написать условие, при котором число не должно удаляться.

                    DestroyNumber(CurrentScoreNumbersList.Count > 1, indexOfRemovedNumber);
                }
                else indexOfRemovedNumber++;
            }
        }

        previousScore = scoreInString;
    }

    private void CollisionChecking_OnBirdCollided(string nullStr) => StartCoroutine(PlayFadingScoreAnimationAndDestroyNumbers());

    private IEnumerator PlayFadingScoreAnimationAndDestroyNumbers()
    {
        animationSource.FadeScoreNumbersAnimation(CurrentScoreNumbersList, numberPositionOffset);

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < scoreInString.Length; i++)
        {
            DestroyNumber(CurrentScoreNumbersList.Count > 0, INDEX_OF_EACH_SYMBOL);
        }
    }

    private void DestroyNumber(bool listIsNotEmpty, int destroyingIndex)
    {
        if (listIsNotEmpty)
        {
            Destroy(CurrentScoreNumbersList[destroyingIndex]);
            CurrentScoreNumbersList.RemoveAt(destroyingIndex);
        }
    }

    private void SetNumberInGame(string scoreSymbol, int numberPositionOffset = default)
    {
        GameObject newNumber;

        var initialPositionOfNumbers = logic.Score != 0 ? 
            new Vector3(numberPositionOffset, 0, 0) : 
            new Vector3(numberPositionOffset, 600, 0);

        switch (scoreSymbol)
        {
            case "0":
                newNumber = GetCreatedNumberInstance(instancesOfScoreNumbers[0], initialPositionOfNumbers);
                break;
            case "1":
                newNumber = GetCreatedNumberInstance(instancesOfScoreNumbers[1], initialPositionOfNumbers);
                break;
            case "2":
                newNumber = GetCreatedNumberInstance(instancesOfScoreNumbers[2], initialPositionOfNumbers);
                break;
            case "3":
                newNumber = GetCreatedNumberInstance(instancesOfScoreNumbers[3], initialPositionOfNumbers);
                break;
            case "4":
                newNumber = GetCreatedNumberInstance(instancesOfScoreNumbers[4], initialPositionOfNumbers);
                break;
            case "5":
                newNumber = GetCreatedNumberInstance(instancesOfScoreNumbers[5], initialPositionOfNumbers);
                break;
            case "6":
                newNumber = GetCreatedNumberInstance(instancesOfScoreNumbers[6], initialPositionOfNumbers);
                break;
            case "7":
                newNumber = GetCreatedNumberInstance(instancesOfScoreNumbers[7], initialPositionOfNumbers);
                break;
            case "8":
                newNumber = GetCreatedNumberInstance(instancesOfScoreNumbers[8], initialPositionOfNumbers);
                break;
            case "9":
                newNumber = GetCreatedNumberInstance(instancesOfScoreNumbers[9], initialPositionOfNumbers);
                break;
            default:
                newNumber = new GameObject();
                break;
        }

        CurrentScoreNumbersList.Add(newNumber);

        newNumber.transform.SetParent(scoreNumbersParent.transform, false);
    }

    private GameObject GetCreatedNumberInstance(GameObject number, Vector3 numberPosition)
    {
        return Instantiate(number, numberPosition, Quaternion.identity);
    }
}
