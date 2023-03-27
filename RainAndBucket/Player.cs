// /*
//  * Project: RainAndBucket
//  *
//  * Date:   03 06, 2023 2:54 PM
//  *
//  * An Astralbytes, Brunasystems Production.
//  * Author: Phosphor(Jordan Grignon, horuuendillus@gmail.com)
//  */

using Godot;

public partial class Player : Area2D
{
	// cache the audio player so we don't have to use GetNode each time we want to play
	// the rain drop caught audio
	private AudioStreamPlayer _audioPlayer;

	// half the size of the collision shape for the bucket sprite
	private Vector2 _halfOfCollisionBoundrySize;

	// size of the current Godot viewport
	private Vector2 _screenSize;

	// Export the movement speed of the bucket so it can be changed in the Godot editor for
	// fine tuning
	[Export] public float Speed  = 400;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// set the screen size for ease of access
		_screenSize = GetViewportRect().Size;
		
		// set the half size of the  collision shape so we can check out of bounds based on
		// the edge of the collision shape instead of the center
		CollisionShape2D playerCollision = (CollisionShape2D) GetNode("PlayerCollides");
		_halfOfCollisionBoundrySize = playerCollision.Shape.GetRect().Size / 2;
		
		// cache the audio player so it is readily available for playing the audio when a drop is caught.
		_audioPlayer = (AudioStreamPlayer) GetNode("DropCaughtAudio");
		
		// handle signal emitted when another Area2D derived object has collided with the bucket.
		AreaEntered += area => OnAreaEntered(area);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// create the movement vector for the player
		Vector2 velocity = Vector2.Zero;
		
		// set movement along the x axis to be 1 pixel to the right
		if (Input.IsActionPressed("ui_right"))
		{
			velocity.X += 1;
		}
		
		// set movement along the x axis to be 1 pixel to the left
		if (Input.IsActionPressed("ui_left"))
		{
			velocity.X -= 1;
		}

		// if user has pressed either of the above keys then
		// multiply the 1 pixel times the movement speed
		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
		}
		
		// Read the current Position data into a local variable as it is a struct
		Vector2 currentPosition = Position;
		
		// set the current position to be the elapsed time since last update multiplied by the
		// velocity which was set based on the value of the Speed field.
		currentPosition += velocity * (float)delta;
		
		// get the maximum x axis value based on the collision shape extents for the Player
		float maximumXvalue = _screenSize.X -  _halfOfCollisionBoundrySize.X;		
		
		// use Clamp to ensure the position of the bucket is inside the bounds of hte viewport
		currentPosition.X = Mathf.Clamp(currentPosition.X, _halfOfCollisionBoundrySize.X, maximumXvalue);

		// set the Player's position to the currentPosition value
		Position = currentPosition;
	}

	/// <summary>
	///  Handles emit of the signal onAreaEntered for the Player object
	/// </summary>
	/// <param name="area">The collision area that has collided with the player's collision area</param>
	private void OnAreaEntered(Area2D area)
	{
		// get the name of the area that collided with the player
		string areaName = area.Name;
		
		// if the area name contains raindrop then we play the audio for when a drop is caught.
		// each time a Raindrop is created it has a unique name, and therefore Equals should not be used here
		if (areaName.Contains("Raindrop"))
		{
			// this next line prints the name of the Raindrop that has collided with the bucket
			// as you can see from the output of this line, that names are unique even though we are spawning the same raindrop scene
			// this line is printed to the output window in the Godot editor or the console window if running in an
			// IDE like Rider or Visual Studio
			GD.Print($"Raindrop Name: {area.Name}");		
			
			// play the drop caught sound
			_audioPlayer.Play(0);
		}
	}
}
