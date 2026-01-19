using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    private DateTime _serverTime;
    
    public bool INIT_TIME
    {
        get; private set;
    }
    public long CURRENT_TICKS
    {
        get { return _serverTime.Ticks; }
    }
    public DateTime NOW
    {
        get { return _serverTime; }
    }
}
