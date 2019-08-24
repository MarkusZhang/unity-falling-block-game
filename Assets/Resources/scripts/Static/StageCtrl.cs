using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public static class StageCtrl
{
    // state var
    private static int stage = 1;
    
    // static config
    private static int maxStage = 3;
    private static string[] playSceneNames = new string[]{"stage-1","stage-2","stage-3","stage-4"};
    private static string[] stageIntros = new string[]{"Strong Man's Space","Successful Man's Kingdom","Professor's Challenge","Final Fantasy"};
    
    public static int GetStage()
    {
        return stage;
    }

    public static int NextStage()
    {
        if (stage < maxStage)
        {
            stage++;       
        }
        return stage;
    }

    public static void SetStage(int s)
    {
        stage = s;
    }

    public static string GetPlaySceneName(int stageNum)
    {
        Debug.Assert(stageNum <= playSceneNames.Length);
        return playSceneNames[stageNum - 1];
    }

    public static string GetStageIntro(int stageNum)
    {
        Debug.Assert(stageNum <= playSceneNames.Length);
        return stageIntros[stageNum - 1];
    }

    public static void Reset()
    {
        stage = 1;
    }
}
