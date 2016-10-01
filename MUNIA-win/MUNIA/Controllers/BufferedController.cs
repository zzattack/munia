using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MUNIA.Util;

namespace MUNIA.Controllers {
	public class BufferedController : IController {
		private readonly IController _real;
		private readonly Stopwatch _sw = Stopwatch.StartNew();
		private CircularBuffer<TimedControllerState> _q;
		private TimeSpan _delay;
		private TimedControllerState _lastState;
		private ControllerState _state;

		public enum DelayMethod {
			Queue,
			Task
		}

		public DelayMethod Method { get; set; }

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
		

		private TimedControllerState realState;
		private bool realStateUpdated;

		private void RealOnStateUpdated(object sender, EventArgs eventArgs) {
			var newState = _real.GetState();
			if (Method == DelayMethod.Task) {
				Task.Delay(Delay).ContinueWith(t => {
					_state = newState;
					OnStateUpdated();
				});
			}
			else {
				var rts = new TimedControllerState(_sw.Elapsed, newState.Axes, newState.Buttons, newState.Hats);
				realStateUpdated = true;
				realState = rts;
				Debug.WriteLine("Packet #{0} enqueued @ {1}", rts.PacketNum, rts.Time.TotalMilliseconds);
			}
		}

		public ControllerState GetState() {
			// see if new real state came in since last check
			if (realStateUpdated) {
				_q.Enqueue(realState);
				realStateUpdated = false;
			}

			if (Method == DelayMethod.Task) return _state;
			else if (_q.Count == 0) return null;

			// find the first enqueued state that's older than delay
			for (int i = _q.Count - 1; i >= 0; i--) {
				if (_sw.Elapsed - _q[i].Time >= Delay) {
					var ret = _q[i];
					if (ret != _lastState) {
						_lastState = ret;
						// var elapsed = _sw.Elapsed;
						// Debug.WriteLine("Packet #{0} dequeued @ {1}, margin {2}ms", ret.PacketNum, elapsed.TotalMilliseconds, (elapsed - ret.Time - Delay).TotalMilliseconds);
						// found the state, everything older is useless
						for (; i > 0; i--) _q.Dequeue();
						OnStateUpdated();
					}
					return ret;
				}
			}
			return null; // this can happen up to Delay time after the first input arrived
		}

		public string Name => _real.Name;
		public bool IsActive => _real.IsActive;
		public bool IsAvailable => _real.IsAvailable;
		public string DevicePath => _real.DevicePath;

		public ControllerType Type => _real.Type;
		public bool Activate() { return _real.Activate(); }
		public void Deactivate() { _real.Deactivate(); }

		public event EventHandler StateUpdated;
		protected virtual void OnStateUpdated() { StateUpdated?.Invoke(this, EventArgs.Empty); }
	}

	public class TimedControllerState : ControllerState {
		public TimeSpan Time { get; }
		public TimedControllerState(TimeSpan t, List<int> axes, List<bool> buttons, List<Hat> hats) : base(axes, buttons, hats) {
			Time = t;
		}

		internal static uint Counter = 0;
		internal readonly uint PacketNum = Counter++;
		internal bool Used = false;
	}

}
