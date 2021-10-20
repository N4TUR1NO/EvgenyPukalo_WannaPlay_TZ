using UnityEngine;

public class PointInTime {

	public Vector3    Position { get; }
	public Quaternion Rotation { get; }

	public PointInTime (Vector3 position, Quaternion rotation)
	{
		Position = position;
		Rotation = rotation;
	}
}