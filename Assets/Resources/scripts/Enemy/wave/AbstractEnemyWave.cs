using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractEnemyWave: MonoBehaviour
{
    public abstract void StartWave();
    public event System.Action OnAllEnemiesDestroyed;
    protected int totalNumToGen; // NOTE: every wave should set this before start
    public bool autoStart = true;
    
    private int numDestroyed;
    private object numDestroyedLock = new Object();
    private float waveStartTime; // TODO: move this to AbstractEnemyWaveWithRewards

    protected void notifyAllEnemiesDestroyed()
    {
        if (OnAllEnemiesDestroyed != null)
        {
            OnAllEnemiesDestroyed();
        }
    }

    protected float getTimeSinceStart()
    {
        return Time.time - waveStartTime;
    }

    void Start()
    {
        waveStartTime = Time.time; //TODO: this is actually assuming wave is started automatically
        // update sound volume slider
        if (AudioManager.instance != null)
        {
            AudioManager.instance.UpdateVolume();
            var slider = GameObject.FindGameObjectWithTag("ui:volume-slider");
            if (slider != null)
            {
                slider.GetComponent<Slider>().value = GlobalConfig.volume;
            }
        }
    }
    
    // wave class should attach this listener to enemy obj
    protected void onSingleEnemyDestoryed()
    {
        Debug.Assert(totalNumToGen!=0,"totalNumToGen must be positive");
        lock (numDestroyedLock)
        {
            if (numDestroyed < totalNumToGen)
            {
                numDestroyed++;
                if (numDestroyed == totalNumToGen)
                {
                    notifyAllEnemiesDestroyed();
                    Destroy(gameObject);
                }
            }
        }
    }
}
