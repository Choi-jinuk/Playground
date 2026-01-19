using System;
using UnityEngine;

public class UI_Title : BaseUI
{
    private AudioSource _titleBGMSource;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        _titleBGMSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _PlayTitleSound(true);
    }

    void _PlayTitleSound(bool play)
    {
        if (_titleBGMSource == null)
            return;
        
        if(play) _titleBGMSource.Play();
        else _titleBGMSource.Stop();
    }
    public override void OnBack()
    {
        
    }
}
