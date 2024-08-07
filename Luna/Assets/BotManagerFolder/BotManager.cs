﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BotManager : MonoBehaviour
{
    private static BotManager _instance;
    [SerializeField] private List<BotNetwork> _botNetworks = new List<BotNetwork>();
    [FormerlySerializedAs("totalBotConfig")][SerializeField] private int totalBotOnMap;
    [FormerlySerializedAs("totalBotConfig")][SerializeField] private int totalBotOnTurn;

    public int TotalBotOnMap
    {
        get { return totalBotOnMap; }
        set { totalBotOnMap = value; }
    }

    public int TotalBotOnTurn
    {
        get { return totalBotOnTurn; }
        set { totalBotOnTurn = value; }
    }

    public static BotManager Instance => _instance;
    public List<BotNetwork> BotNetworks => _botNetworks;

    private void Awake()
    {
        _instance = this;
    }

    public void SpawnBot(BotNetwork botPrefab, Vector3 spawnPosition, WayPoint path)
    {
        BotNetwork newBot = Instantiate(botPrefab, spawnPosition, Quaternion.identity);
        newBot.SetPath(path);
        newBot.OnBotDead += OnBotDead;
        newBot.transform.localRotation = Quaternion.Euler(0, 180, 0);
        newBot.gameObject.SetActive(true);
        _botNetworks.Add(newBot);
    }

    private void OnBotDead()
    {
        totalBotOnMap--;
    }

    // Thêm hàm này để xóa tất cả các bot khi lượt kết thúc
    public void ClearAllBots()
    {
        foreach (var bot in _botNetworks)
        {
            Destroy(bot.gameObject);
        }
        _botNetworks.Clear();
    }
}
