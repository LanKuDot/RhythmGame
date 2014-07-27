/* Filename: SlideNote.cs
 * Play the hint animation of the slide note.
 * Indicate the direction of the SlideNote.
 */

using UnityEngine;
using System.Collections;

public class SlideNote : MonoBehaviour
{
	public Sprite[] sprites;
	private SpriteRenderer spriteRenderer;
	private int index;
	private int delayingFrames = 48;
	private int delayedFrames;			// The counter for the delayed frames
	private bool slideFalied;			// Is the sliding uncompeleted?
	private bool touched;				// Dose the finger slide onto the Note?
	private Color color;

	// Use this for initialization
	void Start ()
	{
		spriteRenderer = renderer as SpriteRenderer;
		color = spriteRenderer.material.color;

		// Invalid initial value
		index = -1;

		gameObject.SetActive( false );
	}
	
	/* Time.fixedDeltaTime is defined at NoteBank */
	/* The compeleted animation: Slide Hint Start --[1 beat]-> Slide Hint Stay
	 * --[1 beat]-> Start Sliding --[1 beat]-> End Sliding.
	 */
	void FixedUpdate ()
	{
		spriteRenderer.sprite = sprites[index];
		++index;
		if ( index == 47 )
		{
			gameObject.SetActive( false );
		}
		if ( index == 24 && delayedFrames != delayingFrames )
		{
			--index;
			++delayedFrames;

			if ( color.a != 1.0f )
			{
				color.a += 0.1f;
				spriteRenderer.material.color = color;
			}
		}

		if ( slideFalied )
			gameObject.SetActive( false );
	}

	/* If the touching ended at the slide note, the Slide action is falied. */
	void touchEnded()
	{
		slideFalied = true;
	}

	void touchMoving()
	{
		touched = true;
	}

	/* Reset on disable */
	void OnDisable()
	{
		// Grading
		if ( index == -1 )
			;	// Discard invalid initial value
		else if ( !touched )
			Grader.Instance.grading( 99, GameConfig.NoteTypes.SLIDE, Grader.gradeLevel.MISS );
		else if ( slideFalied )
			Grader.Instance.grading( 99, GameConfig.NoteTypes.SLIDE, Grader.gradeLevel.MISS );
		else
			Grader.Instance.grading( 99, GameConfig.NoteTypes.SLIDE, Grader.gradeLevel.HIT );

		// Make the program show the direction directly, not gradully
		index = 23;

		delayedFrames = 0;
		color.a = 0.0f;
		spriteRenderer.material.color = color;
		
		slideFalied = false;
		touched = false;
	}
}
