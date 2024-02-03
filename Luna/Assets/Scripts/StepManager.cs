using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepManager : MonoBehaviour
{
   [SerializeField]private SpawnBot _spawnBotNormal;
   [SerializeField]private SpawnBot _spawnBotParachute;
   public static StepManager Instance;
   public int test=0;
   public int Step  { get; set; }


   private void Awake()
   {
      Instance = this;
   }

   private void OnEnable()
   {
      SetData();
      StepStart();
   }

   private void Update()
   {
      if (CheckStepDone())
      {
         test = 0;
         Debug.LogError("Step"+Step+"done");
         PathManager.Instance.ResetPath();
         SetData();
         StepStart();
      }
   }
   public bool CheckStepDone()
   {
      //return BotManager.I== ;
      return false;
   }
   public void SetData()
   {
         _spawnBotParachute.InitData(ConfigManager.Instance.GetStepData(Step).NumberParachute);
         _spawnBotNormal.InitData(ConfigManager.Instance.GetStepData(Step).NumberBot);
      
   }
   public void StepStart()
   {
      _spawnBotParachute.Run();
      _spawnBotNormal.Run();
   }
}
