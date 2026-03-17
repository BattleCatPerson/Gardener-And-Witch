using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    public bool timePaused;
    [SerializeField] List<Action> moveQueue;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        moveQueue = new();
    }

    void Update()
    {
        if (!timePaused && moveQueue.Count > 0)
        {
            moveQueue[0]?.Invoke();
            moveQueue.RemoveAt(0);
            timePaused = true;
        }
    }
    public void AddMove(Action move) => moveQueue.Add(move);
    public void EndMove()
    {
        timePaused = false;
    }
}
