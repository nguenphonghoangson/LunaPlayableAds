using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] public ConfigBotInGame configBotInGame;
    [SerializeField] public GameResultData gameResultData;
    [SerializeField] private List<Spawn> spawns;
    public static GamePlayManager Instance;
    public int Turn { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (CheckTurnDone())
        {
            BotManager.Instance.TotalBotOnMap = OnCheckTotalBotOnMap();
            UIManager.Instance.UpdateInitBot(BotManager.Instance.TotalBotOnMap);
            PathManager.Instance.ResetPath();
            StartCoroutine(TurnDelay());
        }
    }

    private void OnDisable()
    {
        OnResetResultData();
    }

    void OnResetResultData()
    {
        gameResultData.TurnCount = 0;
    }

    private IEnumerator TurnDelay()
    {
        yield return new WaitForSeconds(1);
        SetData();
        GameStart();
        gameResultData.TurnCount++;
        EventManager.Invoke(EventName.OnShowEndCard, gameResultData.TurnCount);
        Turn++;
    }

    public bool CheckTurnDone()
    {
        int currentTurn = gameResultData.IsCountTurn ? gameResultData.TurnCount : Turn;
        return BotManager.Instance.TotalBotOnMap <= 0 && currentTurn < configBotInGame.fightRound.Length;
    }

    public void SetData()
    {
        int currentTurn = gameResultData.IsCountTurn ? gameResultData.TurnCount : Turn;
        foreach (var spawn in spawns)
        {
            spawn.InitData(configBotInGame.fightRound[currentTurn].botConfigs);
        }
    }

    public void GameStart()
    {
        foreach (var spawn in spawns)
        {
            spawn.Run();
        }
    }

    private int OnCheckTotalBotOnMap()
    {
        int currentTurn = gameResultData.IsCountTurn ? gameResultData.TurnCount : Turn;
        int botCount = 0;
        foreach (var botConfig in configBotInGame.fightRound[currentTurn].botConfigs)
        {
            if (!botConfig.isNotUse && !botConfig.isNotCount)
            {
                botCount += botConfig.botQuantity;
            }
        }
        return botCount;
    }

    public void LunaClick()
    {
        Debug.LogError("Luna Clicked");
        Luna.Unity.Playable.InstallFullGame();
    }
}
