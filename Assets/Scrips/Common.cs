using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Common
{
    public enum Directions
    {
        none,
        right,
        left,
        up,
        down
    }

    public static class TagCommon
    {
        public static bool Contains(string targetTag, string targetString)
        {
            if (targetTag.Contains(targetString))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    enum WaterElementType { 
        normal,
        bullet
    }
}

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(T);

                instance = (T)FindObjectOfType(t);
                if (instance == null)
                {
                    Debug.LogError(t + " をアタッチしているGameObjectはありません");
                }
            }

            return instance;
        }
    }

    virtual protected void Awake()
    {
        // 他のゲームオブジェクトにアタッチされているか調べる
        // アタッチされている場合は破棄する。
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = this as T;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }
        Destroy(this);
        return false;
    }
}