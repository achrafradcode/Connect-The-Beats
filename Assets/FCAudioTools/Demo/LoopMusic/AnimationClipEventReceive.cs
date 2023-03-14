#if (UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
#define DISABLE_MANUALTIME
#endif

using UnityEngine;
using System;
using System.Collections;

namespace FutureCartographer.FCAudioTools.Demo
{
	public class AnimationClipEventReceive : MonoBehaviour
	{
		public AudioSource syncAudio;
		public Action<AnimationEvent> OnDidAudioMarkerEvent;
		public Action<AnimationEvent> OnDidAudioLoopEvent;

#if !DISABLE_MANUALTIME
		private Animator animator;

		void Start()
		{
			animator = GetComponent<Animator>();
			animator.SetTimeUpdateMode(UnityEngine.Experimental.Director.DirectorUpdateMode.Manual);
		}

		void Update()
		{
			animator.SetTime(syncAudio.time);
		}
#endif

		void OnAudioMarkerEvent(AnimationEvent evt)
		{
			if (Mathf.Abs(syncAudio.time - evt.time) > 0.15f)
				return;

			Debug.Log(string.Format("OnAudioMarkerEvent: {0:0.000} {1} {2}", evt.time, evt.intParameter, evt.stringParameter));

			if (OnDidAudioMarkerEvent != null)
				OnDidAudioMarkerEvent(evt);
		}

		void OnAudioLoopEvent(AnimationEvent evt)
		{
			if (Mathf.Abs(syncAudio.time - evt.time) > 0.15f)
				return;

            Debug.Log(string.Format("OnAudioLoopEvent: {0:0.000} {1} {2}", evt.time, evt.intParameter, evt.stringParameter));

            if (evt.stringParameter == "loopStart")
			{
			}
			else if (evt.stringParameter == "loopEnd" && syncAudio.loop == true)
			{
#if !DISABLE_MANUALTIME
				animator.SetTime(syncAudio.time);
#endif
			}

			if (OnDidAudioLoopEvent != null)
				OnDidAudioLoopEvent(evt);
		}
	}
}
