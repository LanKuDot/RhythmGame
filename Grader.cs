/* Filename: Greder.cs
 * Grading tapping timing by recieving the stop frame 
 * from PrepareNotes and then display the grade.
 * Counting combos.
 * Update score to Score.
 */
using UnityEngine;
using System.Collections;

public class Grader : MonoBehaviour
{
	private GameObject[] gradeText_GUI = new GameObject[GameConfig.numOfTapNotes];
	private GameObject comboText_GUI;
	private int[] showing_tick = new int[GameConfig.numOfTapNotes];	// Record the displaying time of text
	private int combos;
	/* Make Grader.grading() could be called directly. */
	public static Grader Instance;

	public enum gradeLevel {
		MISS = 0,
		BAD,
		EARLY,
		PERFECT,
		LATE,
		HIT,
		DISCARD		// Discarding initialzation judging
	};

	void Awake()
	{
		// Set reference to all textGUI and initialize the showing_tick
		for ( int i = 0; i < GameConfig.numOfTapNotes; ++i )
		{
			gradeText_GUI[i] = GameObject.Find( "GradeGUI_" + i );
			showing_tick[i] = 0;
		}
		comboText_GUI = GameObject.Find( "ComboGUI" );

		Instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		combos = 0;
		comboText_GUI.guiText.text = " ";
	}

	/* Time.fixedDeltaTime is defined at the NoteBank
	 * Counting the showing_tick of the gradeGUIs.
	 * Make gradeGUIs showing the grade in certain time interval.
	 */
	void FixedUpdate()
	{
		for ( int i = 0; i < GameConfig.numOfTapNotes; ++i )
		{
			++showing_tick[i];
			if ( showing_tick[i] > 30 )
			{
				gradeText_GUI[i].guiText.text = " ";
				showing_tick[i] = 0;
			}
		}
	}

	/* Called by [PrepareNote].OnDisable().
	 * Get the grade level from the notes and calculate the score.
	 * Then display the result.
	 */
	public void grading( int position_ID, GameConfig.NoteTypes whichNote, gradeLevel level )
	{
		/* Grading for specific note */
		if ( whichNote == GameConfig.NoteTypes.CLICK )
			gradingClick( level );
		else if ( whichNote == GameConfig.NoteTypes.HOLD )
			gradingHold( level );
		else if ( whichNote == GameConfig.NoteTypes.SLIDE )
			gradingSlide( level );

		/* Display the grade */
		if ( position_ID < 9 )
		{
			switch( level )
			{
			case gradeLevel.BAD:
				gradeText_GUI[ position_ID ].guiText.text = "BAD";
				break;
			case gradeLevel.EARLY:
				gradeText_GUI[ position_ID ].guiText.text = "EARLY";
				break;
			case gradeLevel.LATE:
				gradeText_GUI[ position_ID ].guiText.text = "LATE";
				break;
			case gradeLevel.MISS:
				gradeText_GUI[ position_ID ].guiText.text = "MISS";
				break;
			case gradeLevel.PERFECT:
				gradeText_GUI[ position_ID ].guiText.text = "PERFECT";
				break;
			case gradeLevel.HIT:
				gradeText_GUI[ position_ID ].guiText.text = "HIT";
				break;
			case gradeLevel.DISCARD:
				gradeText_GUI[ position_ID ].guiText.text = " ";
				break;
			}
		}

		/* Combo counting and display */
		// If the grade is BAD, MISS, or DISCARD, reset the combo count.
		if ( level == gradeLevel.BAD ||
		    level == gradeLevel.MISS ||
		    level == gradeLevel.DISCARD )
		{
			combos = 0;
			comboText_GUI.guiText.text = "";
		}
		// Don't need to display the combo text when combo count less than 1.
		else if ( combos == 0 )
			++combos;
		else if ( position_ID == 99 )
			;	// No need to count the slideNote, only nodeNote need.
		else
		{
			++combos;
			comboText_GUI.guiText.text = combos + " combos";
			// Combo bonus
			Score.Instance.updateScore( 50 );
		}

		// Reset showing tick after being tapped.
		if ( position_ID < 9 )
			showing_tick[ position_ID ] = 0;
	}

	/* The grading of CLICK note */
	void gradingClick ( gradeLevel level )
	{
		switch ( level )
		{
		case gradeLevel.BAD:
			Score.Instance.updateScore( 100 );
			break;
		case gradeLevel.EARLY:
			Score.Instance.updateScore( 400 );
			break;
		case gradeLevel.PERFECT:
			Score.Instance.updateScore( 600 );
			break;
		case gradeLevel.LATE:
			Score.Instance.updateScore( 400 );
			break;
		}
	}

	/* The grading of HOLD note */
	void gradingHold( gradeLevel level )
	{
		switch ( level )
		{
		case gradeLevel.BAD:
			Score.Instance.updateScore( 200 );
			break;
		case gradeLevel.EARLY:
			Score.Instance.updateScore( 500 );
			break;
		case gradeLevel.PERFECT:
			Score.Instance.updateScore( 1000 );
			break;
		}
	}

	/* The grading of SLIDE note */
	void gradingSlide( gradeLevel level )
	{
		switch( level )
		{
		case gradeLevel.BAD:
			Score.Instance.updateScore( 200 );
			break;
		case gradeLevel.HIT:
			Score.Instance.updateScore( 1000 );
			break;
		}
	}

}	// end of class Grader
