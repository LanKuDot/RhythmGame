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
	}

	/* Be called from NoteTable.
	 * Wake a PrepareNote which belongs to this TapPoint up, and
	 * the waked PrepareNote will play the animation.
	 */
	void wakeUpPrepareNote()
	{
		/* The order of waking up is Up, Right, Down, Left, and back to Up again.
		 */
		// Wake up a PrepareNote
		preNotes[ next ].SetActive( true );
		// Update the index of next PrepareNote
		++next;
		next = next % preNotes.Length;
	}
	
}	// end of class TapPoint
