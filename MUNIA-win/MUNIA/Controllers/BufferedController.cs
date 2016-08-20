using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MUNIA.Controllers {
	public class BufferedController : IController {
		private readonly IController _real;
		private readonly Stopwatch _sw = Stopwatch.StartNew();
		private CircularBuffer<TimedControllerState> _q;
		private TimeSpan _delay;
		public string Name { get; }
		
		public BufferedController(IController real) {
			_real = real;
			_real.StateUpdated += RealOnStateUpdated;
		}


		public TimeSpan Delay {
			get { return _delay; }
			set {
				if (_delay != value) {
				_delay = value;
					_q = new CircularBuffer<TimedControllerState>((int) _delay.TotalMilliseconds/10);
				}
			}
		}
		

		private void RealOnStateUpdated(object sender, EventArgs eventArgs) {
			var state = _real.GetState();
			_q.Enqueue(new TimedControllerState(_sw.Elapsed, state.Axes, state.Buttons));
		}
		
		public ControllerState GetState() {
			if (_q == null || _q.Count == 0) return null;

			// find the first enqueued state that's older than delay
			for (int i = _q.Count - 1; i >= 0; i--) {
				if (_sw.Elapsed - _q[i].Time > Delay) {
					var ret = _q[i];
					// found the state, everything older is useless
					for (; i > 0; i--) _q.Dequeue();
					return ret;
				}
			}
			return null;
		}

		public bool IsActive => _real.IsActive;
		public bool IsAvailable => _real.IsAvailable;
		public string DevicePath => _real.DevicePath;

		public void Activate() {
			_real.Activate();
		}
		public void Deactivate() {
			_real.Deactivate();
		}
		
		public event EventHandler StateUpdated;
		protected virtual void OnStateUpdated() { StateUpdated?.Invoke(this, EventArgs.Empty); }
	}

	public class ControllerState {
		public ControllerState(List<int> axes, List<bool> buttons) {
			Axes.AddRange(axes);
			Buttons.AddRange(buttons);
		}

		public List<int> Axes { get; } = new List<int>();
		public List<bool> Buttons { get; } = new List<bool>();
	}

	public class TimedControllerState : ControllerState {
		public TimeSpan Time { get; private set; }
		public TimedControllerState(TimeSpan t, List<int> axes, List<bool> buttons) : base(axes, buttons) {
			Time = t;
		}
	}

}
