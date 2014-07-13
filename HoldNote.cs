/*
 * Filename: HoldNote.cs
 */
using UnityEngine;
using System.Collections;

public class HoldNote : MonoBehaviour {

	public Sprite[] sprites;
	private SpriteRenderer spriteRenderer;
	public HoldEndNote holdEndNote;
	private bool gotNewHoldTime;	// Does it get the new holding beats from TapPoint?
	private int index;				// The index of frame
	private int rotatingFrames;		// The number of frames in rotating
	private float degreePerFrame;	// The rotating degree per frame
	private int totalFrames;		// The number of frames the note existing
	private int touchBeganFrameIndex = 0;
	private int touchEndedFrameIndex = 0;

	private int position_ID;		// The index of the position of HoldNote

	// Use this for initialization
	void Start ()
	{
		spriteRenderer = renderer as SpriteRenderer;

		// Hint for 2 beats
		rotatingFrames = ( int )GameConfig.framePerBeats * 2;
		degreePerFrame = 90.0f / ( float ) rotatingFrames;

		gotNewHoldTime = false;

		// Set position ID: "HoldNote_#"
		position_ID = gameObject.name[9];
		position_ID -= 48;

		// Avoid the Grader grading after initialization.
		// By assigning an invalid value.
		touchEndedFrameIndex = -1;

		// Sleep after initialization
		gameObject.SetActive( false );
	}	// end of Start

	/* Time.fixedDeltaTime is defined at NoteBank */
	void FixedUpdate()
	{
		// Wait until get the new holding beats
		if ( gotNewHoldTime )
		{
			spriteRenderer.sprite = sprites[0];

			++index;
			// If the animation ended, sleep.
			if ( index == totalFrames )
				gameObject.SetActive( false );

			if ( index < rotatingFrames )
				// Rotate the sprite counterclockwise
				spriteRenderer.transform.Rotate( Vector3.forward * degreePerFrame );
		}
	}	// end of FixedUpdate

	/* Be called at TapPoint.wakeUpPrepareNote().
	 * Get the duration of the holding beats from TapPoint, and calculate the frames.
	 * Wake up a HoldEndNote, and send the total displaying frames to it.
	 */
	public void setNewHoldBeats( int holdBeats )
	{
		// Rotating frames ( Preparing ) plus staying frames ( Holding )
		totalFrames = ( int )GameConfig.framePerBeats * holdBeats;
		totalFrames += rotatingFrames;
		gotNewHoldTime = true;

		// Wake up holdEndNote
		holdEndNote.gameObject.SetActive( true );
		holdEndNote.setHoldingFrames( totalFrames );
	}

	/* Be called from TapPoint.touched().
	 * Record the touching starting frame.
	 */
	public void touched()
	{
		touchBeganFrameIndex = index;
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
		// Also sleep the holdEndNote
		if ( holdEndNote.gameObject.activeSelf )
			holdEndNote.gameObject.SetActive( false );

		// Grading when the object is unactive.
		Grader.Instance.grading( position_ID, GameConfig.NoteTypes.HOLD, touchEndedFrameIndex );

		// Reset
		gotNewHoldTime = false;
		index = 0;
		touchBeganFrameIndex = 0;
		touchEndedFrameIndex = 0;
		spriteRenderer.sprite = sprites[0];
	}

}	// end of class HoldNote
