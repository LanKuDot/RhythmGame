/*
 * Filename: HoldNote.cs
 */
using UnityEngine;
using System.Collections;

public class HoldNote : MonoBehaviour {

	public Sprite[] sprites;
	private SpriteRenderer spriteRenderer;
	private bool gotNewHoldTime;	// Does it get the new holding beats from TapPoint?
	private bool delaying = false;
	private int holdBeats = 0;		// The holding beats
	private int framesPerBeat;		// The number of frames per beat
	private int index;				// The index of frame
	private int delayFrames = 0;	// The frames delayed

	// Use this for initialization
	void Start ()
	{
		spriteRenderer = renderer as SpriteRenderer;
		// From hint starting to perfect holding starting time would be displayed in 2 beats.
		Time.fixedDeltaTime = ( float ) 60.0f * 2.0f / GameConfig.songBPM / GameConfig.holdNoteBeginFrames;
		framesPerBeat = GameConfig.holdNoteBeginFrames / 2;
		gotNewHoldTime = false;
		// Sleep after initialization
		gameObject.SetActive( false );
	}	// end of Start

	void FixedUpdate()
	{
		// Wait until getting the new value of holding time
		if ( gotNewHoldTime )
		{
			// ....
			if ( !delaying )
			{
				++index;
				if ( index == GameConfig.holdNoteFrames )
					gameObject.SetActive( false );

				spriteRenderer.sprite = sprites[ index ];

				if ( index == GameConfig.holdNoteBeginFrames - 1 )
					delaying = true;
			}
			else 	// Delaying
			{
				++delayFrames;
				if ( delayFrames / framesPerBeat == ( holdBeats - 2 ) )
				{
					+++index;
					spriteRenderer.sprite = sprites[ index ];
					delaying = false;
				}
			}
		}
	}	// end of FixedUpdate

	public void setNewHoldBeats( int newValue )
	{
		holdBeats = newValue;
		gotNewHoldTime = true;
	}

	void OnDisable()
	{
		gotNewHoldTime = false;
		delaying = false;
		// Reset
		index = 0;
		spriteRenderer.sprite = sprites[ index ];
	}

}	// end of class HoldNote
