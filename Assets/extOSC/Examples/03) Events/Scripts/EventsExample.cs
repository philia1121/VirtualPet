﻿/* Copyright (c) 2020 ExT (V.Sigalkin) */

using UnityEngine;
using UnityEngine.UI;

namespace extOSC.Examples
{
	public class EventsExample : MonoBehaviour
	{
		#region Public Vars
		public GameObject OSCManager;

		public OSCTransmitter[] Transmitters;

		[Header("UI Settings")]
		public Text TextRotate;

		public Text TextScale;

		public Text TextPosition;

		#endregion

		#region Private Vars

		private const string _rotateAddress = "/example/3/rotate";

		private const string _scaleAddress = "/example/3/scale";

		private const string _positionAddress = "/example/3/position";

		private Vector3 _position = Vector3.zero;

		#endregion

		#region Unity Methods

		protected virtual void Start()
		{
			if(Transmitters.Length == 0)
				Transmitters = OSCManager.GetComponents<OSCTransmitter>();
			TextScale.text = $"{Vector3.one}";
			TextRotate.text = $"{Vector3.zero}";
			TextPosition.text = $"{Vector3.zero}";
		}

		#endregion

		#region Public Methods

		public void SendRotate(float value)
		{
			value = OSCUtilities.Map(value, 0, 1, 0, 360);

			var vector = new Vector3(0, 0, value);

			SendVector(_rotateAddress, vector);

			TextRotate.text = vector.ToString();
		}

		public void SendScale(float value)
		{
			value = OSCUtilities.Map(value, 0, 1, 1, 5);

			var vector = new Vector3(value, value, value);

			SendVector(_scaleAddress, vector);

			TextScale.text = vector.ToString();
		}

		public void SendPosition(Vector2 value)
		{
			_position.x = OSCUtilities.Map(value.x, -1, 1, -100, 100);
			_position.y = OSCUtilities.Map(value.y, -1, 1, -100, 100);

			SendVector(_positionAddress, _position);

			TextPosition.text = _position.ToString();
		}

		#endregion

		#region Private Methods

		private void SendVector(string address, Vector3 vector)
		{
			var message = new OSCMessage(address);

			message.AddValue(OSCValue.Float(vector.x));
			message.AddValue(OSCValue.Float(vector.y));
			message.AddValue(OSCValue.Float(vector.z));

			if (Transmitters.Length > 0 )
			foreach(var item in Transmitters)
			{
				item.Send(message);
			}
		}

		#endregion
	}
}