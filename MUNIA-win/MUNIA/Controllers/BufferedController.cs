using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MUNIA.Controllers {
	public class BufferedController : IController {
		private readonly IController _real;
		private readonly Stopwatch _sw = Stopwatch.StartNew();
		private readonly Queue<ControllerState> _q = new Queue<ControllerState>();
		
		public BufferedController(IController real) {
			_real = real;
			_real.StateUpdated += RealOnStateUpdated;
		}

		public List<int> Axes { get; } // todo: look back in queue
		public List<bool> Buttons { get; }

		private void RealOnStateUpdated(object sender, EventArgs eventArgs) {
			// todo: update queue
			
		}

		private void CleanQueue() {
			// todo: remove stuff that is older than what we'd ever request
		}
		
		public event EventHandler StateUpdated;
		public bool IsActive => _real.IsActive;
		public bool IsAvailable => _real.IsAvailable;
		public string DevicePath => _real.DevicePath;
		public string Name { get; }

		public void Activate() {
			_real.Activate();
		}
		public void Deactivate() {
			_real.Deactivate();
		}

		// todo: look back in queue
		public TimeSpan Delay { get; set; }

		protected bool Parse(byte[] ev) {
			throw new InvalidOperationException();
		}
	}

	public class ControllerState {
		public ControllerState(TimeSpan t, IController c) {
			Time = t;
			Axes.AddRange(c.Axes);
			Buttons.AddRange(c.Buttons);
		}

		public TimeSpan Time { get; private set; }
		public List<int> Axes { get; } = new List<int>();
		public List<bool> Buttons { get; } = new List<bool>();
	}

}
