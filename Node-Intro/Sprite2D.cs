// /*
//  * Project: 101-scripting-cs
//  *
//  * Date:   02 19, 2023 2:07 PM
//  *
//  * An Astralbytes, Brunasystems Production.
//  * Author: Phosphor(Jordan Grignon, horuuendillus@gmail.com)
//  */

using Godot;

public partial class Sprite2D : Godot.Sprite2D
{
	// half the size of the image texture for the sprite
	private Vector2 _halfOfImageSize;

	// size of the current Godot viewport
	private Vector2 _screenSize;

	// how far to move
	Vector2 velocity = new Vector2(100,50);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// print current sprite location in the viewport
		GD.Print("C# Sprite Location:" + Position.ToString());
		
		// set the screen size for ease of access
		_screenSize = GetViewportRect().Size;
		
		// set the half size of the image so we can check out of bounds based on
		// the edge of the image instead of the center
		_halfOfImageSize = Texture.GetSize() / 2;
		
		// move the sprite to the center of the viewport
		Position = _screenSize / 2;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// move the sprite an amount of pixels equal to velocity * elapsed time
		Position = Position + velocity * (float)delta;
		
		// edges of image instead of center
		float maximumXvalue = _screenSize.X - _halfOfImageSize.X;
		float maximumYvalue = _screenSize.Y - _halfOfImageSize.Y;

		// load the current position in the viewport to a variable
		Vector2 currentPosition = Position;
		
		// check the sprite location to see if it is out of bounds
		// if it is have it change direction
		if (currentPosition.X >= maximumXvalue)
		{
			currentPosition.X = maximumXvalue;
			velocity.X *= -1;
		}
		else if (currentPosition.X < _halfOfImageSize.X)
		{
			currentPosition.X = _halfOfImageSize.X;
			velocity.X *= -1;
		}
		else if (currentPosition.Y >= maximumYvalue)
		{
			currentPosition.Y = maximumYvalue;
			velocity.Y *= -1;
		}
		else if (currentPosition.Y < _halfOfImageSize.Y)
		{
			currentPosition.Y = _halfOfImageSize.Y;
			velocity.Y *= -1;
		}

		// set the Position value so that the screen is updated with
		// the new position
		Position = currentPosition;

	}
}
