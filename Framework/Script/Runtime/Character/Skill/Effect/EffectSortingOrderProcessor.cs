using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSortingOrderProcessor : MonoBehaviour
{
    private List<ParticleSystemRenderer> _rendererList = new List<ParticleSystemRenderer>();
    private int _addLayer = 0;

    public void Init(ParticleSystem[] particles, float y, int addLayer)
    {
        _addLayer = addLayer;
        foreach (var particle in particles)
        {
            var rd = particle.gameObject.GetComponent<ParticleSystemRenderer>();
            if (rd == null)
                continue;

            UpdateSortingOrder(y);
            _rendererList.Add(rd);
        }
    }

    public void UpdateSortingOrder(float y)
    {
        for (int i = 0; i < _rendererList.Count; i++)
        {
            _UpdateSortingOrder(_rendererList[i], y);
        }
    }

    private void _UpdateSortingOrder(ParticleSystemRenderer rd, float y)
    {
        int value = DataUtil.CalcSortingOrderByY(y) + _addLayer;
        if (rd.sortingOrder != value)
            rd.sortingOrder = value;
    }
}
