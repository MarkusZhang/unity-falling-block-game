using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LifeCtrl
{
    private const int MAXHEALTH = 5;
    
    public static event System.Action OnLivesLeftChange;
    public static event System.Action OnLifeLost;
    public static event System.Action OnLifeUp; // this is triggered when life is awarded
    public static event System.Action OnHealthChange;
    
    // state vars
    private static int livesLeft = 20;
    private static int currentHealth = MAXHEALTH;

    public static void RemoveEventListeners()
    {
        OnLivesLeftChange = null;
        OnHealthChange = null;
        OnLifeUp = null;
        OnLifeLost = null;
    }

    public static void Reset()
    {
        RemoveEventListeners();
        livesLeft = 2;
        currentHealth = MAXHEALTH;
    }
    
    public static bool HasLifeLeft()
    {
        return livesLeft > 0;
    }

    public static void ConsumeLife()
    {
        if (livesLeft > 0)
        {
            livesLeft--;
            if (OnLivesLeftChange != null)
            {
                OnLivesLeftChange();
            }

            if (OnLifeLost != null)
            {
                OnLifeLost();
            }

            currentHealth = MAXHEALTH;
            if (OnHealthChange != null)
            {
                OnHealthChange();
            }
        }
    }

    public static int GetLifeLeft()
    {
        return livesLeft;
    }

    public static void AddLife(int num)
    {
        livesLeft += num;
        if (OnLivesLeftChange != null)
        {
            OnLivesLeftChange();
        }

        if (OnLifeUp != null)
        {
            OnLifeUp();
        }
    }

    public static void AddLife()
    {
        AddLife(1);
    }

    // NOTE: this should only be called in game start scene
    public static void SetLife(int num)
    {
        livesLeft = num;
    }

    public static void UpdateHealth(int val)
    {
        Debug.Assert(val >= 0);
        if (val >= 0)
        {
            currentHealth = val;
            if (OnHealthChange != null)
            {
                OnHealthChange();
            }
        }        
    }

    public static int GetCurrentHealth()
    {
        return currentHealth;
    }
}
