using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace GameplayFramework
{
    /// <summary>
    /// https://forum.unity.com/threads/how-to-know-when-a-timeline-is-complete.493058/
    /// </summary>
    internal class TimelineCallbackHandler : MonoBehaviour
    {
        const double EPSILON = 0.00001;
        event TimelineCallbackEventHandler Played;
        event TimelineCallbackEventHandler Paused;
        event TimelineCallbackEventHandler Stopped;
        event TimelineCallbackEventHandler Completed;
        event TimelineCallbackEventHandler<PlayableState> StateChanged;
        event TimelineCallbackEventHandler<DirectorWrapMode> Wrapped;
        PlayableDirector _director;
        Coroutine _watchRoutine;
        double _lastTime = double.MinValue;
        System.Action OnComplete;
        
        internal static TimelineCallbackHandler Create(PlayableDirector director, System.Action OnComplete)
        {
            var hnd = director.gameObject.AddComponent<TimelineCallbackHandler>();
            hnd._director = director;
            //If the director played in awake, we might miss the played event invocation,
            //if our OnEnable is called after its Awake
            if (hnd._director.playOnAwake)
            {
                hnd.Director_Played(director);
            }

            hnd._director.played += hnd.Director_Played;
            hnd._director.paused += hnd.Director_Paused;
            hnd._director.stopped += hnd.Director_Stopped;
            hnd.OnComplete = OnComplete;
            hnd.Completed += hnd.Hnd_Completed;
            hnd.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
            return hnd;
        }

        void Hnd_Completed()
        {
            OnComplete?.Invoke();
            Destroy(this);
        }

        void OnDisable()
        {
            this._director.played -= Director_Played;
            this._director.paused -= Director_Paused;
            this._director.stopped -= Director_Stopped;
            this.Completed -= Hnd_Completed;
            if (_watchRoutine != null) StopCoroutine(_watchRoutine);
        }

        void Director_Played(PlayableDirector director)
        {
            StateChanged?.Invoke(PlayableState.Playing);
            Played?.Invoke();

            _watchRoutine = StartCoroutine(WatchDirector());
        }

        void Director_Paused(PlayableDirector director)
        {
            StateChanged?.Invoke(PlayableState.Paused);
            Paused?.Invoke();

            if (_watchRoutine != null) StopCoroutine(_watchRoutine);
        }

        void Director_Stopped(PlayableDirector director)
        {
            StateChanged?.Invoke(PlayableState.Stopped);
            Stopped?.Invoke();

            //If the delta time + the last recorded time is greater than the duration of the current playable,
            //assume the playable finished playing with a Wrap mode of None, and invoke the Completed event.
            if (_lastTime + Time.deltaTime >= director.playableAsset.duration)
            {
                _lastTime = double.MinValue;

                Completed?.Invoke();
            }

            if (_watchRoutine != null) StopCoroutine(_watchRoutine);
        }

        IEnumerator WatchDirector()
        {
            _lastTime = double.MinValue;

            while (_director.time < EPSILON) yield return null;

            while (true)
            {
                //If the time is less than or equal to the last time recorded, invoke the Wrapped event
                if (_director.time - _lastTime < EPSILON)
                {
                    Wrapped?.Invoke(_director.extrapolationMode);

                    //If the wrap mode is set to Hold, exit the method to avoid repeatedly invoking the method
                    if (_director.extrapolationMode == DirectorWrapMode.Hold) yield break;
                }

                _lastTime = _director.time;
                yield return null;
            }
        }
    }
}