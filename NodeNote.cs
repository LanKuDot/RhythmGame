/* Filename: NodeNote.cs
 * Play the animation of the node note.
 */

using UnityEngine;
using System.Collections;

public class NodeNote : MonoBehaviour
{
	public Sprite[] sprites;
	private SpriteRenderer spriteRenderer;
	private BoxCollider2D boxCollider;
	private float degreePerFrame;
	private int rotatingFrames;
	private int delayingFrames;		// The number of frames in delaying
	private int totalFrames;		// = rotatingFrame + delayingFrame
	private int index;				// The index of frame
	private int waitingFrames;		// The waiting frames from waken up to playing animation
	private int touchedFrame;		// The index of the sprites when the note is touched
	private int position_id;
	private Color color;			// The color setting of the renderer

	// Use this for initialization
	void Start ()
	{
		spriteRenderer = renderer as SpriteRenderer;
		boxCollider = gameObject.collider2D as BoxCollider2D;
		
		// The click note would rotate 90 degrees in 2 beats
		rotatingFrames = ( int )GameConfig.framePerBeats * 2;
		degreePerFrame = 90.0f / ( float )rotatingFrames;
		// Delaying some frames to avoid the ClickNote disappered suddenly
		delayingFrames = 10;
		// Calculate total frames
		totalFrames = rotatingFrames + delayingFrames;

		// Invalid initial value would be discarded at the grading.
		touchedFrame = 9999;

		// Get the color setting of the renderer and initialize the alpha to 0
		color = spriteRenderer.material.color;

		// Get position ID: NodeNote_#
		position_id = gameObject.name[9];
		position_id -= 48;

		gameObject.SetActive( false );
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		if ( waitingFrames == 0 )
		{
			// Enable the collider when start to playing the note
			if ( touchedFrame == -1 )
				boxCollider.enabled = true;

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
		else
			--waitingFrames;
	}

	public void setWaitingFrames( int frames )
	{
		waitingFrames = frames;
	}

	/* Maybe the node is the first node. */
	void touched()
	{
		touchedFrame = index;
		boxCollider.enabled = false;
	}

	/* Maybe the node is the internal node. */
	void touchMoving()
	{
		touchedFrame = index;
		boxCollider.enabled = false;
	}

	/* Maybe the node is the last node, or the touching ended at the node. */
	void touchEnded()
	{
		touchedFrame = index;
		boxCollider.enabled = false;
	}

	void OnDisable()
	{
		// Grading
		if ( touchedFrame == 9999 )
			;	// Discard invalid initial value
		else if ( touchedFrame == -1 )
			Grader.Instance.grading( position_id, GameConfig.NoteTypes.SLIDE, Grader.gradeLevel.MISS );
		else if ( touchedFrame < 10 )
			Grader.Instance.grading( position_id, GameConfig.NoteTypes.SLIDE, Grader.gradeLevel.BAD );
		else
			Grader.Instance.grading( position_id, GameConfig.NoteTypes.SLIDE, Grader.gradeLevel.HIT );

		// Reset Index
		index = 0;
		spriteRenderer.transform.eulerAngles = new Vector3( 0.0f, 0.0f, 90.0f );
		// Reset alpha value
		color.a = 0.0f;
		spriteRenderer.material.color = color;

		waitingFrames = 999;
		touchedFrame = -1;
		boxCollider.enabled = false;
	}
}
