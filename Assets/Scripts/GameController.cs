using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameField gameField;
    [SerializeField] AstarPathFinding astarPathFinding;
    private AICharacter aiCharacter;

    public int score;
    [SerializeField] private int blockCount = 128;
    [SerializeField] private int rewardCount = 16;

    private List<GameObject> rewardList = new List<GameObject>();

    void Awake()
    {
        InitGameController();
    }

    void InitGameController()
    {
        InitGameField();
        InitRewards();
        InitAICharacter();
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

        StartCoroutine(AIFindRewardLoop());
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

    IEnumerator AIFindRewardLoop()
    {
        for (int i = 0; i < rewardList.Count; i++)
        {
            yield return new WaitUntil(() => aiCharacter.isMoving == false);
            var queuePath = astarPathFinding.GetPath(aiCharacter.transform.position, rewardList[i].transform.position);
            aiCharacter.SetPath(queuePath);
        }
    }

    void Update()
    {
        FindObjectOfType<UIController>().SetScore(score);
    }
}
