/* Filename: GameConfig.cs
 * The setting of the game.
 */

using UnityEngine;
using System.Collections;

public class GameConfig : MonoBehaviour
{
	/* General Setting */
	public static int numOfTapNotes	= 9;
	public static float songBPM		= 140f;

	/* The type of notes */
	public enum NoteTypes {
		CLICK = 0,
		HOLD
	};

	/* Sprites */
	public static int clickNoteSpriteLength = 26;	// Hard coding...
	public static int holdNoteBeginFrames = 19;
	public static int holdNoteEndFrames = 18;
	public static int holdNoteFrames = 37;

	/* Grading */
	// For click notes: the perfect timing is at frame no. 20
	public static int click_BAD		= 15;
	public static int click_EARLY	= 19;
	public static int click_PERFECT	= 22;
	public static int click_LATE	= 26;
	public static int click_MISS	= 26;
	// For hold notes:
	public static int hold_BAD		= 26;
	public static int hold_EARLY	= 32;
	public static int hold_PERFECT	= 38;
}	// end of class GameConfig