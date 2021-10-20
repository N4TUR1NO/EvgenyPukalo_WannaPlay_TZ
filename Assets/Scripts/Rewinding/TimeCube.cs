using System.Collections.Generic;
using UnityEngine;

public class TimeCube : MonoBehaviour 
{
	#region Fields
	
	private	bool _isRewinding;
	private	bool _isRecording;
	private	bool _isCubeAtRest;
	
	private List<PointInTime> _pointsInTime;
	private Rigidbody _rb;

	private PointInTime _startPointInTime;
	
	#endregion

	#region OnEnable/OnDisable
	
	private void OnEnable()
	{
		GameManager.CubeTouch     += StartRecord;
		GameManager.CubesAtRest   += StopRecord;
		GameManager.RewindStarted += StartRewind;
	}

	private void OnDisable()
	{
		GameManager.CubeTouch     -= StartRecord;
		GameManager.CubesAtRest   -= StopRecord;
		GameManager.RewindStarted -= StartRewind;
	}
	
	#endregion

	#region Init
	
	void Start () 
	{
		_pointsInTime = new List<PointInTime>();
		_rb = GetComponent<Rigidbody>();

		_startPointInTime = new PointInTime(transform.position, transform.rotation);
	}
	
	#endregion

	#region RewindingCycle
	
	void FixedUpdate ()
	{
		if (_isRewinding)
			Rewind();
		else if (_isRecording)
			Record();
	}

	void Rewind ()
	{
		if (_pointsInTime.Count > 0)
		{
			PointInTime pointInTime = _pointsInTime[0];
            transform.position = pointInTime.Position;
			transform.rotation = pointInTime.Rotation;
			_pointsInTime.RemoveAt(0);
		} 
		else
		{
			transform.position = _startPointInTime.Position;
			transform.rotation = _startPointInTime.Rotation;
			StopRewind();

			GameManager.CanShoot = true;
		}
		
	}
	
	private void Record ()
	{
		if (_rb.velocity == Vector3.zero && !_isCubeAtRest)
		{
			GameManager.IncCubesAtRest();
			_isCubeAtRest = true;
		}
		_pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
	}
	
	#endregion
	
	#region Start/Stop methods
	
	private void StartRewind ()
	{
		_isRewinding    = true;
		_rb.isKinematic = true;
	}

	private void StopRewind ()
	{
		_isRewinding    = false;
		_rb.isKinematic = false;
	}

	private void StartRecord()
	{
		_isCubeAtRest = false;
		_isRecording  = true;
	}

	private void StopRecord() => _isRecording = false;

	#endregion
	
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.GetComponent<Bullet>())
		{
			GameManager.CubeWasTouch();
		}
	}
}