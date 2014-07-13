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
	private Color color;			// The color setting of the sprite

	private int position_ID;		// The index of the position of HoldNote

	// Use this for initialization
	void Start ()
	{
		spriteRenderer = renderer as SpriteRenderer;
		color = spriteRenderer.material.color;

		// Hint for 2 beats
		rotatingFrames = ( int )GameConfig.framePerBeats * 2;
		degreePerFrame = 90.0f / ( float ) rotatingFrames;

		gotNewHoldTime = false;

		// Set position ID: "HoldNote_#"
		position_ID = gameObject.name[9];
		position_ID -= 48;

		// Avoid the Grader grading after initialization.
		// By assigning an invalid value.
		touchBeganFrameIndex = 999999;
		index = 999999;

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

			// Fade-in effect
			if ( color.a != 1.0f )
			{
				color.a += 0.1f;
				spriteRenderer.material.color = color;
			}

			if ( index < rotatingFrames )
				// Rotate the sprite counterclockwise
				spriteRenderer.transform.Rotate( Vector3.forward * degreePerFrame );
			else if ( index == rotatingFrames + 20 )
			{
				if ( touchBeganFrameIndex == 0 )
					gameObject.SetActive( false );
			}
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
		if ( index > rotatingFrames )
			gameObject.SetActive( false );
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

		// Grading
		Grader.gradeLevel level;
		if ( touchBeganFrameIndex == 0 )
			level = Grader.gradeLevel.MISS;
		else if ( index < totalFrames - 8 )
			level = Grader.gradeLevel.BAD;
		else if ( index < totalFrames - 3 )
			level = Grader.gradeLevel.EARLY;
		else if ( index < totalFrames + 1 )
			level = Grader.gradeLevel.PERFECT;
		else
			level = Grader.gradeLevel.DISCARD;
		Grader.Instance.grading( position_ID, GameConfig.NoteTypes.HOLD, level );

		// Reset
		gotNewHoldTime = false;
		index = 0;
		touchBeganFrameIndex = 0;
		spriteRenderer.sprite = sprites[0];
		spriteRenderer.transform.eulerAngles = new Vector3( 0.0f, 0.0f, 90.0f );
		color.a = 0.0f;
	}

}	// end of class HoldNote
