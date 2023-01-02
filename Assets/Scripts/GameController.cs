using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameField gameField;
    [SerializeField] private AstarPathFinding astarPathFinding;
    [SerializeField] private CameraFollow cameraFollow;
    private AICharacter aiCharacter;

    [SerializeField] private int score;
    [SerializeField] private int blockCount = 128;
    [SerializeField] private int rewardCount = 16;

    private List<GameObject> rewardList = new List<GameObject>();

    private bool isWin = false;

    void Awake()
    {
        InitGameController();
    }

    void InitGameController()
    {
        InitGameField();
        InitRewards();
        InitAICharacter();

        cameraFollow.Init(aiCharacter.transform);
    }

    void InitAICharacter()
    {
        score = 0;

        aiCharacter = gameField.InitAICharacter(0, 0);

        if(aiCharacter == null)
        {
            Debug.LogWarning("Please setup AI Character object properly.");
            return;
        }

        aiCharacter.SetOnPathCompleted(OnPathCompleted); // set AI callback
        MoveAI();
    }

    void InitGameField()
    {
        gameField.InitGameField(64, 64);

        while (blockCount > 0)
        {
            int rdX = Random.Range(0, 64);
            int rdY = Random.Range(0, 64);
            if (gameField.IsCellBlocked(rdX, rdY))
                continue;

            gameField.BlockCell(rdX, rdY);
            blockCount--;
        }
    }

    void InitRewards()
    {
        while (rewardCount > 0)
        {
            int rdX = Random.Range(0, 64);
            int rdY = Random.Range(0, 64);

            if (gameField.IsCellBlocked(rdX, rdY))
                continue;

            var reward = gameField.CreateReward(rdX, rdY);
            rewardList.Add(reward); // add reward to list
            rewardCount--;
        }
    }

    void OnPathCompleted()
    {
        if (isWin) return;

        AddScore();
        if(score < rewardList.Count)
        {
            if (rewardList[score] != null) // reward already claimed
                MoveAI();
        }
        else
        {
            UIController.Instance.DisplayWinText();
            isWin = true;
        }
    }

    void MoveAI()
    {
        var queuePath = astarPathFinding.GetPath(aiCharacter.transform.position, rewardList[score].transform.position);
        aiCharacter.SetPath(queuePath);
    }

    void AddScore()
    {
        score++;
        UIController.Instance.SetScore(score);
    }
}
