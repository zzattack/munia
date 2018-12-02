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
		private ObjectPool<TimedControllerState> _pool;
		private TimeSpan _delay;
		private TimedControllerState _lastState;
		private ControllerState _taskDelayedState;
		private ControllerState _realState;
		private bool _realStateUpdated;
		private PollingController _poller;

		public enum DelayMethod { Queue, Task }

		public DelayMethod Method { get; set; } = DelayMethod.Queue; // queue is more efficient, task is more simple

		public BufferedController(IController real, TimeSpan delay) {
			_real = real;
			if (real.RequiresPolling) {
				_poller = new PollingController(real, 1000 / 60);
				_poller.StateUpdated += RealOnStateUpdated;
			}
			else {
				_real.StateUpdated += RealOnStateUpdated;
			}
			Delay = delay;
		}

		public TimeSpan Delay {
			get { return _delay; }
			set {
				if (_delay != value) {
					_delay = value;
					_q = new CircularBuffer<TimedControllerState>(Math.Max(10, (int)_delay.TotalMilliseconds / 3));
					_pool = new ObjectPool<TimedControllerState>(_q.Capacity);
				}
			}
		}

		private void RealOnStateUpdated(object sender, EventArgs eventArgs) {
			var newState = (sender as IController).GetState();
			if (Method == DelayMethod.Task) {
				Task.Delay(Delay).ContinueWith(t => {
					_taskDelayedState = newState;
					OnStateUpdated();
				});
			}
			else if (_pool != null) {
				_realStateUpdated = true;
				_realState = newState;
				// Debug.WriteLine("Packet #{0} enqueued @ {1}", rts.PacketNum, rts.Time.TotalMilliseconds);
			}
		}

		public ControllerState GetState() {
			// see if new real state came in since last check
			if (_realStateUpdated) {
				var rts = _pool.Get();
				rts.SetState(_sw.Elapsed, _realState.Axes, _realState.Buttons, _realState.Hats);
				_q.Enqueue(rts);
				_realStateUpdated = false;
			}

			if (Method == DelayMethod.Task) return _taskDelayedState;
			else if (_q.Count == 0) return _lastState;


			// find the first enqueued state that's not older than delay,
			// then the previous is our candidate
			TimedControllerState ret = _q.Peek();
			while (_q.Count > 0 &&  (_sw.Elapsed - _q.Peek().Time) >= Delay) {
				ret = _q.Dequeue();
				_pool.Return(ret);
			}
			
			if (ret != null && !ReferenceEquals(ret, _lastState) && (_sw.Elapsed - ret.Time) >= Delay) {
				// Debug.WriteLine("Packet dequeued @ {0}, margin {1}ms",_sw.Elapsed.TotalMilliseconds, (_sw.Elapsed - ret.Time - Delay).TotalMilliseconds);
				_lastState = ret;
				// found the state, everything older is useless
				OnStateUpdated();
			}

			return _lastState; // this can happen up to Delay time after the first input arrived*/
		}

		public string Name => _real.Name;

		public bool IsAvailable => _real.IsAvailable;
		public string DevicePath => _real.DevicePath;

		public ControllerType Type => _real.Type;

		public bool RequiresPolling => false;

		public bool Activate() { return _real.Activate(); }
		public void Deactivate() { _real.Deactivate(); }
		public bool IsAxisTrigger(int axisNum) => _real.IsAxisTrigger(axisNum);

		public event EventHandler StateUpdated;
		protected virtual void OnStateUpdated() { StateUpdated?.Invoke(this, EventArgs.Empty); }
		public void Dispose() {
			_real.Dispose();
		}
	}

	public class TimedControllerState : ControllerState {
		public TimeSpan Time { get; private set; }
		public TimedControllerState() : base(new List<double>(), new List<bool>(), new List<Hat>()) { }

		public TimedControllerState(TimeSpan t, List<double> axes, List<bool> buttons, List<Hat> hats) : base(axes, buttons, hats) {
			Time = t;
		}

		internal bool Used = false;

		public void SetState(TimeSpan elapsed, List<double> axes, List<bool> buttons, List<Hat> hats) {
			Time = elapsed;
			Axes.EnsureSize(axes.Count);
			Buttons.EnsureSize(buttons.Count);
			Hats.EnsureSize(hats.Count);

			for (int i = 0; i < axes.Count; i++) Axes[i] = axes[i];
			for (int i = 0; i < buttons.Count; i++) Buttons[i] = buttons[i];
			for (int i = 0; i < hats.Count; i++) Hats[i] = hats[i];
			Used = false;
		}
	}

	public class ObjectPool<T> {
		private CircularBuffer<T> _availables;
		private CircularBuffer<T> _backup;
		public ObjectPool(int capacity) { Initialize(capacity); }

		public void Initialize(int capacity) {
			_availables = new CircularBuffer<T>(capacity);
			_backup = new CircularBuffer<T>(capacity);
			for (int i = 0; i < capacity; i++) _backup.Enqueue(Activator.CreateInstance<T>());
			Reclaim();
		}

		public void Reclaim() {
			_availables.Clear();
			foreach (var item in _backup)
				_availables.Enqueue(item);
		}

		public T Get() {
			if (_availables.Count == 0) {
				Reclaim();
				Debug.WriteLine("Everything is given out, reclaiming. Perhaps raise capacity!");
			}
			return _availables.Dequeue();
		}
		public void Return(T item) { _availables.Enqueue(item); }

	}

}
