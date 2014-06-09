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
	private GameObject[] gradeText_GUI
		= new GameObject[GameConfig.numOfTapNotes];
	private GameObject comboText_GUI;
	private int[] showing_tick
		= new int[GameConfig.numOfTapNotes];	// Record the displaying time of text
	private int combos;
	/* Make Grader.grading() could be called directly. */
	public static Grader Instance;

	enum gradeLevel {
		MISS = 0,
		BAD,
		EARLY,
		PERFECT,
		LATE,
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
		// Hard coding...
		Time.fixedDeltaTime = 0.040816f;
	}

	/* Counting the showing_tick of the gradeGUIs.
	 * Make gradeGUIs showing the grade in certain time interval.
	 */
	void FixedUpdate()
	{
		for ( int i = 0; i < GameConfig.numOfTapNotes; ++i )
		{
			++showing_tick[i];
			if ( showing_tick[i] > 10 )
			{
				gradeText_GUI[i].guiText.text = " ";
				showing_tick[i] = 0;
			}
		}
	}

	/* Called from PrepareNote.OnDisable().
	 * When a PrepareNote is inactive, it sends the stop frame to Grader
	 * to grade. After finished grading, the grade will be shown at the
	 * corresponding position.
	 */
	public void grading( int position_ID, GameConfig.NoteTypes whichNote, int stopFrame )
	{
		gradeLevel level = gradeLevel.MISS;

		/* Grading for specific note */
		if ( whichNote == GameConfig.NoteTypes.CLICK )
			level = gradingClick( stopFrame );

		/* Display the grade */
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
		else
		{
			++combos;
			comboText_GUI.guiText.text = combos + " combos";
			// Combo bonus
			Score.Instance.updateScore( 50 * combos );
		}

		// Reset showing tick after being tapped.
		showing_tick[ position_ID ] = 0;
	}

	/* The grading of CLICK note */
	gradeLevel gradingClick ( int stopFrame )
	{
		if ( stopFrame < GameConfig.click_BAD )
		{
			Score.Instance.updateScore( 100 );
			return gradeLevel.BAD;
		}
		else if ( stopFrame < GameConfig.click_EARLY )
		{
			Score.Instance.updateScore( 400 );
			return gradeLevel.EARLY;
		}
		else if ( stopFrame < GameConfig.click_PERFECT )
		{
			Score.Instance.updateScore( 600 );
			return gradeLevel.PERFECT;
		}
		else if ( stopFrame < GameConfig.click_LATE )
		{
			Score.Instance.updateScore( 400 );
			return gradeLevel.LATE;
		}
		else if ( stopFrame == GameConfig.click_MISS )
		{
			return gradeLevel.MISS;
		}
		/* stopFrame > 26?
		 * To avoid grading the initialization of the PrepareNote, which
		 * will set the initial index to be "sprites.Length + 10" and
		 * call OnDisable() after finished initialization.
		 * The Grader would discard this value in grading, and display nothing.
		 */
		return gradeLevel.DISCARD;
	}

		/* Combo counting and display */
		if ( level == gradeLevel.BAD || level == gradeLevel.MISS )
		{
			combos = 0;
			comboText_GUI.guiText.text = "";
		}
		// Don't need to display the combo text when combo count less than 1.
		else if ( combos == 0 )
			++combos;
		else
		{
			++combos;
			comboText_GUI.guiText.text = combos + " combos";
			// Combo bonus
			Score.Instance.updateScore( 50 * combos );
		}

		// Reset showing tick after being tapped.
		showing_tick[ position_ID ] = 0;
	}

}	// end of class Grader
