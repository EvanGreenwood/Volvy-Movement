﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ClipOrBank
{

    public bool HasValue { get { return _isBank ? _soundBank != null : _audioClip != null; } }
    public bool IsEmpty { get { return _isBank ? _soundBank == null : _audioClip == null; } }
    public bool IsBank { get { return _isBank; } }
    public bool IsClip { get { return !_isBank; } }
    public AudioClip Clip { get { return _audioClip; } }
    public SoundBank Bank { get { return _soundBank; } }

    [SerializeField]
    private bool _isBank = true;

    [SerializeField]
    private AudioClip _audioClip;

    [SerializeField]
    private SoundBank _soundBank;

    public ClipOrBank(bool isBank)
    {
        _isBank = isBank;
    }

    public string GetName()
    {
        if (HasValue)
        {
            return _isBank ? _soundBank.name : _audioClip.name;
        }

        return "Empty";
    }

    public SoundInstance Fetch(ISoundPool pool)
    {
        if (IsEmpty) return null;

        if (_isBank)
        {
            Type type = _soundBank.GetType();
            if (type == typeof(AmbienceSoundBank)) return (_soundBank as AmbienceSoundBank).Fetch(pool);
            if (type == typeof(BlendSoundBank)) return (_soundBank as BlendSoundBank).Fetch(pool);
            if (type == typeof(EffectSoundBank)) return (_soundBank as EffectSoundBank).Fetch(pool);
            if (type == typeof(ImpactSoundBank)) return (_soundBank as ImpactSoundBank).Fetch(pool, 0f);
            if (type == typeof(SequenceSoundBank)) return (_soundBank as SequenceSoundBank).Fetch(pool);

            throw new NotImplementedException("SoundBank type not implemented: " + type);
        }

        EffectSoundInstance sound = pool.FetchFromPool<EffectSoundInstance>();
        sound.SetClip(_audioClip);

        return sound;
    }

}
