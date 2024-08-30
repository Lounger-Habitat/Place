// Created by SwanDEV 2017

using System;
using UnityEngine;

public sealed class ProGifPlayerTexture2D : ProGifPlayerComponent
{
    [HideInInspector] public Texture2D m_Texture2D;

    public Action<Texture2D> OnTexture2DCallback;
    public Action OnEndFrameCallBack;
    void Awake()
    {
        displayType = DisplayType.None;
    }

    // Update gif frame for the Player (Update is called once per frame)
    void Update()
    {
        base.ThreadsUpdate();

        if (State == PlayerState.Playing && displayType == DisplayType.None)
        {
            float time = ignoreTimeScale ? Time.unscaledTime : Time.time;
            float dt = Mathf.Min(time - nextFrameTime, interval);
            if (dt >= 0f)
            {
                spriteIndex = (spriteIndex >= gifTextures.Count - 1) ? 0 : spriteIndex + 1;
                nextFrameTime = time + interval / playbackSpeed - dt;

                if (spriteIndex < gifTextures.Count)
                {
                    if (OnPlayingCallback != null) OnPlayingCallback(gifTextures[spriteIndex]);

                    _SetTexture(spriteIndex);
                }

                if (spriteIndex==gifTextures.Count - 1 && spriteIndex!=0)
                {
                    //Debug.Log("最后一帧。");
                    _SetEndFrameCallBack();
                }
            }
        }
    }

    public override void Play(RenderTexture[] gifFrames, float fps, bool isCustomRatio, int customWidth, int customHeight, bool optimizeMemoryUsage)
    {
        base.Play(gifFrames, fps, isCustomRatio, customWidth, customHeight, optimizeMemoryUsage);
        
        displayType = DisplayType.None;
        _SetTexture(0);
        Debug.Log("第一帧播放");
    }

    protected override void _OnFrameReady(GifTexture gTex, bool isFirstFrame)
    {
        if (isFirstFrame)
        {
            displayType = DisplayType.None;
            _SetTexture(0);
        }
    }

    private void _SetTexture(int frameIndex)
    {
        if (optimizeMemoryUsage)
        {
            gifTextures[frameIndex].SetColorsToTexture2D(ref m_Texture2D);
            if (OnTexture2DCallback != null) OnTexture2DCallback(m_Texture2D);
        }
    }

    private void _SetEndFrameCallBack()
    {
        if (OnEndFrameCallBack != null) OnEndFrameCallBack.Invoke();
    }

    public override void Clear(bool clearBytes = true, bool clearCallbacks = true)
    {
        if (m_Texture2D != null)
        {
            Destroy(m_Texture2D);
        }
        base.Clear(clearBytes, clearCallbacks);
    }
}
