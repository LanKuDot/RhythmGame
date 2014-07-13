/* Filename: GameConfig.cs
 * The setting of the game.
 */

using UnityEngine;
using System.Collections;

public class GameConfig : MonoBehaviour
{
	/* General Setting */
	public static int numOfTapNotes	= 9;
	public static float songBPM		= 140.0f;
	public static float framePerBeats = 24.0f;

	/* The type of notes */
	public enum NoteTypes {
		CLICK = 0,
		HOLD
	};

}	// end of class GameConfig