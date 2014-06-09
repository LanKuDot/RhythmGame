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
	private int realFrames = 0;		// The frames that the hold note passed
	private int touchBeganFrameIndex = 0;
	private int touchEndedFrameIndex = 0;

	private int position_ID;		// The index of the position of HoldNote

	// Use this for initialization
	void Start ()
	{
		spriteRenderer = renderer as SpriteRenderer;
		// From hint starting to perfect holding starting time would be displayed in 2 beats.
		Time.fixedDeltaTime = ( float ) 60.0f * 2.0f / GameConfig.songBPM / GameConfig.holdNoteBeginFrames;
		framesPerBeat = GameConfig.holdNoteBeginFrames / 2;
		gotNewHoldTime = false;
		// Setting position ID: "HoldNote_#"
		position_ID = gameObject.name[9];
		position_ID -= 48;
		// Avoid the Grader grading after initialization.
		touchEndedFrameIndex = GameConfig.holdNoteFrames + 10;
		// Sleep after initialization
		gameObject.SetActive( false );
	}	// end of Start

	void FixedUpdate()
	{
		// Wait until getting the new value of holding time
		if ( gotNewHoldTime )
		{
			++realFrames;
			// If 5 frames after the hint ended and still untouched, then sleep.
			if ( realFrames == GameConfig.holdNoteBeginFrames + 5 &&
			    touchBeganFrameIndex == 0 )
				gameObject.SetActive( false );

			// ....
			if ( !delaying )
			{
				++index;
				if ( index == GameConfig.holdNoteFrames )
				{
					touchEndedFrameIndex = index;	// Record the ended frame
					gameObject.SetActive( false );
				}

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

	/* Be called from TapPoint.touched().
	 * Record the touching starting frame.
	 */
	public void touched()
	{
		touchBeganFrameIndex = realFrames;
	}

	/* Be called from TapPoing.touchEnded().
	 * Record the touching ending frame.
	 */
	public void touchEnded()
	{
		if ( index > GameConfig.holdNoteBeginFrames - 1 )
		{
			touchEndedFrameIndex = index;
			gameObject.SetActive( false );
		}
		else
			/* If the touching ended before the hold starting,
			 * the touching is invaild, and then discard the
			 * touching record.
			 */
			touchBeganFrameIndex = 0;
	}

	void OnDisable()
	{
		// Grading when the object is unactive.
		Grader.Instance.grading( position_ID, GameConfig.NoteTypes.HOLD, touchEndedFrameIndex );
		Debug.Log( "Hold Stop Frame: " + touchEndedFrameIndex );
		// Reset
		gotNewHoldTime = false;
		delaying = false;
		index = 0;
		delayFrames = 0;
		realFrames = 0;
		touchBeganFrameIndex = 0;
		touchEndedFrameIndex = 0;
		spriteRenderer.sprite = sprites[ index ];
	}

}	// end of class HoldNote
