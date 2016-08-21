using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MUNIA.Controllers {
	public class BufferedController : IController {
		private readonly IController _real;
		private readonly Stopwatch _sw = Stopwatch.StartNew();
		private CircularBuffer<TimedControllerState> _q;
		private TimeSpan _delay;
		private TimedControllerState _lastState;
		public string Name { get; }

		public BufferedController(IController real, TimeSpan delay) {
			_real = real;
			_real.StateUpdated += RealOnStateUpdated;
			Delay = delay;
		}

		public TimeSpan Delay {
			get { return _delay; }
			set {
				if (_delay != value) {
					_delay = value;
					_q = new CircularBuffer<TimedControllerState>(Math.Max(10, (int)_delay.TotalMilliseconds / 10));
				}
			}
		}

		private void RealOnStateUpdated(object sender, EventArgs eventArgs) {
			var state = _real.GetState();
			var ts = new TimedControllerState(_sw.Elapsed, state.Axes, state.Buttons);
			_q.Enqueue(ts);
			Debug.WriteLine("Packet #{0} enqueued @ {1}", ts.PacketNum, ts.Time.TotalMilliseconds);
		}

		public ControllerState GetState() {
			// find the first enqueued state that's older than delay
			for (int i = _q.Count - 1; i >= 0; i--) {
				if (_sw.Elapsed - _q[i].Time > Delay) {
					var ret = _q[i];
					if (ret != _lastState) {
						_lastState = ret;
						var elapsed = _sw.Elapsed;
						Debug.WriteLine("Packet #{0} dequeued @ {1}, margin {2}ms", ret.PacketNum, elapsed.TotalMilliseconds, (elapsed - ret.Time - Delay).TotalMilliseconds);
						// found the state, everything older is useless
						for (; i > 0; i--) _q.Dequeue();
						OnStateUpdated();
					}
					return ret;
				}
			}
			return null; // this can happen up to Delay time after the first input arrived
		}

		public bool IsActive => _real.IsActive;
		public bool IsAvailable => _real.IsAvailable;
		public string DevicePath => _real.DevicePath;
		public ControllerType Type => _real.Type;
		public bool Activate() { return _real.Activate(); }
		public void Deactivate() { _real.Deactivate(); }

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

		internal static uint Counter = 0;
		internal readonly uint PacketNum = Counter++;
	}

}
