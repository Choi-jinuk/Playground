using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPacket : BasePacket
{
    protected override void Request(BasePacket packet, bool showIndicate, bool isCryptography)
    {
        if (packet is WebPacket localPacket)
        {
            
        }
    }
}
