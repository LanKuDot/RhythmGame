/**
 * Filename: TapPoint.cs
 * Manage the behavior ( sleep or wake ) of PrepareNotes which this TapPoint has.
 */
using UnityEngine;
using System.Collections;

public class TapPoint : MonoBehaviour
{
	private GameObject[] preNotes = new GameObject[4];	// Each TapPoint has 4 PrepareNote objects
	private string TPname;	// The name of TapPoints object
	private int next;		// The index of the next PrepareNote would be waked up
	private int waitTouch;	// The index of the PrepareNote that waiting for being touched
	
	// Awake() is called before Start()
	void Awake()
	{
		/* Setting reference to all PrepareNotes that belongs to this TapPoint.
		 * The name of TapPoints objects is "Notes_tapPoints_#",
		 * and the name of PrepareNotes objects is  "PrepareNote_#_U/R/D/L".
		 */
		TPname = gameObject.name;
		char tapPoint_index = TPname[ TPname.Length - 1 ];
		preNotes[0] = GameObject.Find( "PrepareNote_" + tapPoint_index + "_0" );
		preNotes[1] = GameObject.Find( "PrepareNote_" + tapPoint_index + "_1" );
		preNotes[2] = GameObject.Find( "PrepareNote_" + tapPoint_index + "_2" );
		preNotes[3] = GameObject.Find( "PrepareNote_" + tapPoint_index + "_3" );
	}
	
	// Use this for initialization
	void Start()
	{
		// Initialize the index of the PrepareNotes
		next = 0;
		waitTouch = 0;
	}
	
	/* Be called from NoteTable.
	 * Wake a PrepareNote which belongs to this TapPoint up, and
	 * the waked PrepareNote will play the animation.
	 */
	public void wakeUpPrepareNote()
	{
		/* The order of waking up is Up, Right, Down, Left, and back to Up again.
		 */
		// Wake up a PrepareNote
		preNotes[ next ].SetActive( true );
		// Update the index of next PrepareNote
		++next;
		next = next % preNotes.Length;
	}
	
	/* Be called from TouchingEvent.checkTouch() if the TouchingEvent
	 * detects the touched point is on the TapPoints.
	 * Force a PrepareNote to sleep.
	 * The order of forcing a PrepareNote to sleep is the order of PrepareNotes
	 * played. For example, the order of playing PrepareNotes is 0, 1, 2, and 3,
	 * then the order of forcing PrepareNotes is also 0, 1, 2, and 3.
	 */
	void touched()
	{
		// waitTouch can't be more than or equal to next
		if ( waitTouch != next )
		{
			// Force a PrepareNote to sleep.
			preNotes[waitTouch].SetActive( false );
			// Update the index of waitTouch
			++waitTouch;
			waitTouch = waitTouch % preNotes.Length;
		}
	}
}	// end of class TapPoint
