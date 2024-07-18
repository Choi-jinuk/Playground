using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ActionComponent : BaseComponent
{
    private AniEventData _aniEventData = new AniEventData();
    private PoolUtil<AniEventDataElement> _eventPool = new PoolUtil<AniEventDataElement>();
    
}
