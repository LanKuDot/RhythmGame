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
	private GameObject[] gradeText_GUI = new GameObject[9];
	private GameObject comboText_GUI;
	private int combos;
	/* Make Grader.grading() could be called directly. */
	public static Grader Instance;

	enum gradeLevel {
		MISS = 0,
		BAD,
		EARLY,
		PERFECT,
		LATE
	};

	void Awake()
	{
		// Set reference to all textGUI
		for ( int i = 0; i < 9; ++i )
			gradeText_GUI[i] = GameObject.Find( "GradeGUI_" + i );
		comboText_GUI = GameObject.Find( "ComboGUI" );

		Instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		combos = 0;
		comboText_GUI.guiText.text = " ";
	}

	/* Called from PrepareNote.OnDisable().
	 * When a PrepareNote is inactive, it sends the stop frame to Grader
	 * to grade. After finished grading, the grade will be shown at the
	 * corresponding position.
	 */
	public void grading( int position_ID, int stopFrame )
	{
		gradeLevel level = gradeLevel.MISS;

		/* Grading and diaplay the grade. */
		if ( stopFrame < 15 )
		{
			level = gradeLevel.BAD;
			gradeText_GUI[ position_ID ].guiText.text = "BAD";
		}
		else if ( stopFrame < 19 )
		{
			level = gradeLevel.EARLY;
			gradeText_GUI[ position_ID ].guiText.text = "EARLY";
		}
		else if ( stopFrame < 21 )
		{
			level = gradeLevel.PERFECT;
			gradeText_GUI[ position_ID ].guiText.text = "PERFECT";
		}
		else if ( stopFrame < 26 )
		{
			level = gradeLevel.LATE;
			gradeText_GUI[ position_ID ].guiText.text = "LATE";
		}
		else if ( stopFrame == 26 )
		{
			level = gradeLevel.MISS;
			gradeText_GUI[ position_ID ].guiText.text = "MISS";
		}
		/* stopFrame > 26?
		 * To avoid grading the initialization of the PrepareNote, which
		 * will set the initial index to be "sprites.Length + 10" and
		 * call OnDisable() after finished initialization.
		 * The Grader would discard this value in grading, and display nothing.
		 */

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
		}
	}

}	// end of class Grader
