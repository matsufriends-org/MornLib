﻿using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace MornBeat
{
    public static class MornBeatCore
    {
        private static MornBeatMemoSo s_currentBeatMemo;
        private static int s_tick;
        private static float s_lastBgmTime;
        private static bool s_waitLoop;
        private static Subject<BeatTimingInfo> s_beatSubject = new();
        private static Subject<Unit> s_initializeBeatSubject = new();
        private static Subject<Unit> s_endBeatSubject = new();
        public static IObservable<BeatTimingInfo> OnBeat => s_beatSubject;
        public static IObservable<Unit> OnInitializeBeat => s_initializeBeatSubject;
        public static IObservable<Unit> OnEndBeat => s_endBeatSubject;

        public static float GetMusicPlayingTime<TBeatType>() where TBeatType : Enum =>
            MornBeatSolverMonoBase<TBeatType>.Instance.MusicPlayingTimeImpl + s_currentBeatMemo.Offset;

        public static void Reset()
        {
            s_currentBeatMemo = null;
            s_tick = 0;
            s_lastBgmTime = 0;
            s_waitLoop = false;
            s_beatSubject = new Subject<BeatTimingInfo>();
            s_initializeBeatSubject = new Subject<Unit>();
            s_endBeatSubject = new Subject<Unit>();
        }

        public static float GetBeatTiming(int tick)
        {
            if (s_currentBeatMemo == null)
            {
                return Mathf.Infinity;
            }

            return s_currentBeatMemo.GetBeatTiming(tick);
        }

        public static void UpdateBeat<TBeatType>() where TBeatType : Enum
        {
            var time = GetMusicPlayingTime<TBeatType>();
            if (s_currentBeatMemo == null)
            {
                return;
            }

            if (s_waitLoop)
            {
                if (s_lastBgmTime <= time)
                {
                    return;
                }

                s_waitLoop = false;
            }

            s_lastBgmTime = time;
            if (s_lastBgmTime < s_currentBeatMemo.GetBeatTiming(s_tick))
            {
                return;
            }

            s_beatSubject.OnNext(new BeatTimingInfo(s_tick, s_currentBeatMemo.BeatCount));
            s_tick++;
            if (s_tick == s_currentBeatMemo.TickSum)
            {
                if (s_currentBeatMemo.IsLoop)
                {
                    s_tick = 0;
                }

                s_waitLoop = true;
                s_endBeatSubject.OnNext(Unit.Default);
            }
        }

        public static void InitializeBeat<TBeatType>(TBeatType beatType, bool isForceInitialize = false) where TBeatType : Enum
        {
            var solver = MornBeatSolverMonoBase<TBeatType>.Instance;
            if (s_currentBeatMemo == solver[beatType] && isForceInitialize == false)
            {
                return;
            }

            s_tick = 0;
            s_currentBeatMemo = solver[beatType];
            s_waitLoop = false;
            solver.OnInitializeBeatImpl(beatType);
            s_initializeBeatSubject.OnNext(Unit.Default);
        }

        public static int GetNearTick<TBeatType>(int beat, out float nearDif) where TBeatType : Enum
        {
            Assert.IsTrue(beat <= s_currentBeatMemo.BeatCount);
            var tickSize = s_currentBeatMemo.BeatCount / beat;
            var lastTick = s_tick - s_tick % tickSize;
            var nextTick = lastTick + tickSize;
            var curTime = GetMusicPlayingTime<TBeatType>();
            var preTime = GetBeatTiming(lastTick);
            var nexTime = GetBeatTiming(nextTick);
            while (curTime < preTime && lastTick - tickSize >= 0)
            {
                lastTick -= tickSize;
                nextTick -= tickSize;
                preTime = GetBeatTiming(lastTick);
                nexTime = GetBeatTiming(nextTick);
            }

            while (nexTime < curTime && nextTick + tickSize < s_currentBeatMemo.TickSum)
            {
                lastTick += tickSize;
                nextTick += tickSize;
                preTime = GetBeatTiming(lastTick);
                nexTime = GetBeatTiming(nextTick);
            }

            if (curTime < (preTime + nexTime) / 2f)
            {
                nearDif = preTime - curTime;
                return lastTick;
            }

            nearDif = nexTime - curTime;
            return nextTick;
        }
    }
}
