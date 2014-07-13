/**
 * - Filename: ClickNote.cs
 * - The lifetime of PrepareNote:
 * 1. Sleep after initializing.
 * 2. Sleeping until waked by TapPoint, and then playing the animation.
 * 3. If the animation ends, sleep.
 * 4. Goto 2.
 * - To increase the performance of the ClickNote,
 * the frame of PrepareNote would update every fixed time interval,
 * not busy calling.
 * Hence, using FixedUpdate() instead of Update().
 */
using UnityEngine;
using System.Collections;

public class ClickNote : MonoBehaviour
{
	public Sprite[] sprites;		// Array storing all frames of prepareNote
	private SpriteRenderer spriteRenderer;
	private float degreePerFrame;	// The rotation degree of the click note per frame
	private int rotatingFrames;		// The number of frames in rotating
	private int delayingFrames;		// The number of frames in delaying
	private int totalFrames;		// = rotatingFrame + delayingFrame
	private int index;				// The index of frame
	private int position_ID;		// The index of the position of PrepareNotes
	private Color color;			// The color setting of the renderer

	// Use this for initialization
	void Start ()
	{
		spriteRenderer = renderer as SpriteRenderer;

		// The click note would rotate 90 degrees in 2 beats
		rotatingFrames = ( int )GameConfig.framePerBeats * 2;
		degreePerFrame = 90.0f / ( float )rotatingFrames;
		// Delaying some frames to avoid the ClickNote disappered suddenly
		delayingFrames = 10;
		// Calculate total frames
		totalFrames = rotatingFrames + delayingFrames;

		/* Initialize the index number of frame by assigning a garbage value to
		 * avoid grading after initialization. The note will call OnDisable()
		 * which sends the stop frame to Grader, but there is no note assigned
		 * to TapPoint at initialization. The Grader will discard the invaild value.
		 */
		index = 9999;

		// Get the color setting of the renderer and initialize the alpha to 0
		color = spriteRenderer.material.color;

		// Setting position ID
		position_ID = gameObject.name[12];
		position_ID -= 48;

		spriteRenderer.transform.Rotate( Vector3.back * 90 );

		// Sleeping after initializing.
		gameObject.SetActive( false );
	}

	/* Time.fixedDeltaTime is defined at NoteBank. */
	void FixedUpdate ()
	{
		spriteRenderer.sprite = sprites[0];
		// Rotate the click note per frame
		if ( index < rotatingFrames )
			spriteRenderer.transform.Rotate( Vector3.forward * degreePerFrame );

		// Fade in effect
		if ( color.a != 1.0f )
		{
			color.a += 0.1f;
			spriteRenderer.material.color = color;
		}

		++index;
		// If the animation ends, sleep.
		if ( index == totalFrames )
			gameObject.SetActive( false );
	}

	/* Send the grade to the grader, and reset the arguments.
	 */
	void OnDisable()
	{
		// Grading
		Grader.gradeLevel level;
		if ( index < 40 )
			level = Grader.gradeLevel.BAD;
		else if ( index < 44 )
			level = Grader.gradeLevel.EARLY;
		else if ( index < 51 )
			level = Grader.gradeLevel.PERFECT;
		else if ( index < totalFrames - 1 )
			level = Grader.gradeLevel.LATE;
		else if ( index == totalFrames )
			level = Grader.gradeLevel.MISS;
		else
			level = Grader.gradeLevel.DISCARD;
		Grader.Instance.grading( position_ID, GameConfig.NoteTypes.CLICK, level );

		// Reset index to 0 and the start angle of the ClickNote
		index = 0;
		spriteRenderer.transform.Rotate( Vector3.back * 90 );
		// Reset alpha value
		color.a = 0.0f;
		spriteRenderer.material.color = color;
	}

}	// end of class PrepareNote
