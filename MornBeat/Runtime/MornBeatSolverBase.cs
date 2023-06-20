﻿using System;
using MornDictionary;
using UnityEngine;

namespace MornBeat
{
    public abstract class MornBeatSolverBase<TBeatType> : MonoBehaviour where TBeatType : Enum
    {
        [Header("MakeBeat")] [SerializeField] private MornDictionary<TBeatType, MornBeatMemoSo> _beatDictionary;
        private static MornBeatSolverBase<TBeatType> s_instance;

        internal MornBeatMemoSo this[TBeatType beatType] => _beatDictionary[beatType];

        internal static MornBeatSolverBase<TBeatType> Instance
        {
            get
            {
                if (s_instance != null)
                {
                    return s_instance;
                }

                s_instance = FindObjectOfType<MornBeatSolverBase<TBeatType>>();
                if (s_instance == null)
                {
                    Debug.LogError($"{nameof(MornBeatSolverBase<TBeatType>)} is not found.");
                }

                return s_instance;
            }
        }
        internal float MusicPlayingTimeImpl => MusicPlayingTime;

        internal void OnInitializeBeatImpl(TBeatType beatType)
        {
            OnInitializedBeat(beatType, _beatDictionary[beatType].Clip);
        }

        protected abstract float MusicPlayingTime { get; }
        protected abstract void OnInitializedBeat(TBeatType beatType, AudioClip clip);
    }
}
