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
		index = 70;

		// Setting position ID
		position_ID = gameObject.name[12];
		position_ID -= 48;

		spriteRenderer.transform.Rotate( Vector3.back * 90 );

		// Sleeping after initializing.
		gameObject.SetActive( false );
	}

	void FixedUpdate ()
	{
		spriteRenderer.sprite = sprites[0];
		// Rotate the click note per frame
		if ( index < rotatingFrames )
			spriteRenderer.transform.Rotate( Vector3.forward * degreePerFrame );

		++index;
		// If the animation ends, sleep.
		if ( index == totalFrames )
			gameObject.SetActive( false );
	}

	// OnDisable() is called when the game object becomes inactive.
	void OnDisable()
	{
		Grader.Instance.grading( position_ID, GameConfig.NoteTypes.CLICK, index );
		// Reset index to 0 an the angle of the ClickNote
		index = 0;
		spriteRenderer.transform.Rotate( Vector3.back * 90 );
	}

}	// end of class PrepareNote
