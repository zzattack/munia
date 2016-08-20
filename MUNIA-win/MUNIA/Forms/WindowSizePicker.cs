using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MUNIA.Forms {
	public partial class WindowSizePicker : Form {
		private List<SceneCollection> _sceneCollections;
		public Size ChosenSize;
		private Size _preload;

		private WindowSizePicker() {
			InitializeComponent();
			LoadObsSceneCollection();
		}

		private void LoadObsSceneCollection() {
			_sceneCollections = new List<SceneCollection>();
			
			string path = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"obs-studio", "basic", "scenes");

			foreach (string js in Directory.EnumerateFiles(path, "*.json")) {
				var collection = new SceneCollection();
				dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(Path.Combine(path, js)));
				collection.Name = json.name;
				foreach (dynamic s in json.scene_order) {
					collection.Scenes.Add(new Scene() { Name = s.name});
				}
				foreach (dynamic src in json.sources) {
					string sceneName = src.name;
					var scene = collection.Scenes.FirstOrDefault(s => s.Name == sceneName);
					if (scene == null) continue;

					JArray items = src.settings.items;
					foreach (dynamic item in items) {
						scene.Source.Add(new Source {
							Name = item.name,
							Size = new Size((int) item.bounds.x, (int) item.bounds.y)
						});
					}
				}
				_sceneCollections.Add(collection);
			}

			gbOBS.Enabled = _sceneCollections.Any();
			cbSceneCollection.Items.AddRange(_sceneCollections.ToArray());
		}

		public WindowSizePicker(Size preload) : this() {
			_preload = preload;
			nudWidth.Value = preload.Width;
			nudHeight.Value = preload.Height;
		}

		private void BtnAccept_Click(object sender, EventArgs e) {
			ChosenSize.Width = (int)nudWidth.Value;
			ChosenSize.Height = (int)nudHeight.Value;
		}

		private void nudWidth_Enter(object sender, EventArgs e) {
			nudWidth.Select(0, nudWidth.Text.Length);
		}

		private void nudHeight_Enter(object sender, EventArgs e) {
			nudHeight.Select(0, nudHeight.Text.Length);
		}

		private void cbSceneCollection_SelectedIndexChanged(object sender, EventArgs e) {
			cbScene.Items.Clear();
			var collection = cbSceneCollection.SelectedItem as SceneCollection;
			if (collection != null)
				cbScene.Items.AddRange(collection.Scenes.ToArray());
		}

		private void cbScene_SelectedIndexChanged(object sender, EventArgs e) {
			cbSource.Items.Clear();
			var scene = cbScene.SelectedItem as Scene;
			if (scene != null)
				cbSource.Items.AddRange(scene.Source.ToArray());
		}

		private void cbSource_SelectedIndexChanged(object sender, EventArgs e) {
			var source = cbSource.SelectedItem as Source;
			if (source != null) {
				nudWidth.Value = source.Size.Width;
				nudHeight.Value = source.Size.Height;
			}
		}

		private void btnReset_Click(object sender, EventArgs e) {
			nudWidth.Value = _preload.Width;
			nudHeight.Value = _preload.Height;
		}
	}

	public class SceneCollection {
		public string Name;
		public List<Scene> Scenes = new List<Scene>();
		public override string ToString() => Name;
	}

	public class Scene {
		public string Name;
		public List<Source> Source = new List<Source>();
		public override string ToString() => Name;
	}

	public class Source {
		public string Name;
		public Size Size;
		public override string ToString() => Name;
	}

}
