using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BirdCollisionManager : MonoBehaviour
{
    #region CONSTANTS

    private const int _PLATINUM_MEDAL_INDEX = 4;
    private const int _AMOUNT_OF_MEDALS = 4;
    private const int _BEST_SCORE_INDEX = 0;

    private const float _MIN_DISTANCE_FOR_PLAY_HIT_SOUND_AFTER_PIPE_COLLIDING = -0.6f;

    #endregion

    [Header("Bird components")]
    [SerializeField] private GameObject bird;
    [SerializeField] private BirdBumpingVelocity baseBumping;
    [SerializeField] private Animator birdAnimator;

    [Header("Tools")]
    [SerializeField] private MovingManager movingManager;
    [SerializeField] private GameObject touchController;

    [Header("Labels")]
    [SerializeField] private Text resultedScoreLabel;
    [SerializeField] private Text bestScoreLabel;

    [Header("Sources")]
    [SerializeField] private AnimationManager animationSource;
    [SerializeField] private SoundManager audioSource;

    [Header("Other")]
    [SerializeField] private GameLogic logic;
    [SerializeField] private GameObject pipeSpawner;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private ScorePanelObjects scorePanel;

    private ScoreData _scoreData;

    private bool _isDistanceBiggerThenMin = true;

    #region MONO

    private void Start() => _scoreData = new ScoreData();

    private void OnEnable()
    {
        CollisionListener.OnBirdCollided += CollisionChecking_OnBirdCollided;
        CollisionListener.OnBirdTriggered += CollisionChecking_OnBirdTriggered;
    }

    private void OnDisable()
    {
        CollisionListener.OnBirdCollided -= CollisionChecking_OnBirdCollided;
        CollisionListener.OnBirdTriggered -= CollisionChecking_OnBirdTriggered;
    }

    #endregion

    private void CollisionChecking_OnBirdCollided(string nameOfCollidedObject)
    {
        birdAnimator.enabled = false;
        pipeSpawner.SetActive(false);
        touchController.SetActive(false);

        RemovePipeColliders();
        movingManager.StopMoving();

        if (nameOfCollidedObject == "Sky")
            audioSource.PlayDie();
        else if (nameOfCollidedObject == "Pipe")
        {
            audioSource.PlayDie();
            baseBumping.BirdBumpingInPipe();
            audioSource.PlayHit();

            if (bird.transform.position.y < _MIN_DISTANCE_FOR_PLAY_HIT_SOUND_AFTER_PIPE_COLLIDING)
                _isDistanceBiggerThenMin = false;
        }
        else if (nameOfCollidedObject == "Base")
        {
            if (_isDistanceBiggerThenMin)
            {
                baseBumping.BirdBumpingInBase();
                audioSource.PlayHit();
            }
            _isDistanceBiggerThenMin = true;

            _scoreData.LoadData();

            int bestPreviousScore = TryToGetBestPreviousScore();
            int currentScore = logic.Score;

            _scoreData.CurrentScore = currentScore;
            _scoreData.SaveData();

            ChangeBestScoreLabelText();
            GameObject newLabel = TryToGetNewLabel(currentScore, bestPreviousScore);
            GameObject medal = scorePanel.AddMedal(CheckScores());

            if (medal != null && newLabel != null)
                PlayGameOverScreen(medal, newLabel);
            else if (medal != null)
                PlayGameOverScreen(medal);
            else if (newLabel != null)
                PlayGameOverScreen(newLabel: newLabel);
            else
                PlayGameOverScreen();

            ScoreUpdate();
        }
    }

    private void CollisionChecking_OnBirdTriggered(string nameOfCollidedObject)
    {
        if (nameOfCollidedObject == "PipeTrigger")
        {
            logic.AddScore();

            audioSource.PlayPoint();

            RemovePipeTrigger();
        }
    }

    private GameObject TryToGetNewLabel(int currentScore, int bestPreviousScore)
    {
        if (currentScore > bestPreviousScore)
        {
            return scorePanel.AddNewLabel();
        }

        return null;
    }

    private int TryToGetBestPreviousScore()
    {
        if (_scoreData.ScoreNumbersInTabelList.Count > 0)
        {
            return _scoreData.ScoreNumbersInTabelList[_BEST_SCORE_INDEX];
        }
        
        return 0;
    }

    private int CheckScores()
    {
        int score = logic.Score;

        if (score >= 40)
        {
            return _PLATINUM_MEDAL_INDEX;
        }
        else
        {
            for (int i = 1; i < _AMOUNT_OF_MEDALS; i++)
            {
                if (score >= 10 * i && score < 10 * (i + 1))
                {
                    return i;
                }
            }
        }

        return 0;
    }

    private void ChangeBestScoreLabelText()
    {
        if (_scoreData.ScoreNumbersInTabelList.Count > 0)
        {
            bestScoreLabel.text = _scoreData.ScoreNumbersInTabelList[_BEST_SCORE_INDEX].ToString();
        }
        else
        {
            bestScoreLabel.text = "";
        }
    }

    private void PlayGameOverScreen(GameObject medal = null, GameObject newLabel = null)
    {
        gameOverScreen.SetActive(true);

        animationSource.AppearGameOverScreenAnimation(medal, newLabel);
    }

    private void RemovePipeColliders()
    {
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");

        if (pipes != null)
        {
            foreach (var pipe in pipes)
            {
                BoxCollider2D pipeCollider = pipe.GetComponent<BoxCollider2D>();
                pipeCollider.enabled = false;
            }
        }
    }

    private void RemovePipeTrigger()
    {
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("PipePrefab");

        foreach (var pipe in pipes)
        {
            Component[] pipeComponents = pipe.GetComponentsInChildren(typeof(BoxCollider2D));

            foreach (var pipeComponent in pipeComponents)
            {

                if (pipeComponent is BoxCollider2D pipeBoxCollider &&
                    pipeBoxCollider.isTrigger &&
                    pipeBoxCollider.IsTouching(bird.GetComponent<CircleCollider2D>()))
                {
                    pipeBoxCollider.enabled = false;
                    break;
                }
            }
        }
    }

    private void ScoreUpdate()
    {
        resultedScoreLabel.text = logic.Score.ToString();
        logic.Score = 0;
    }
}
