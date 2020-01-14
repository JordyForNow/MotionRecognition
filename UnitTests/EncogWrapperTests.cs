using NUnit.Framework;
using MotionRecognition;
using Encog.Engine.Network.Activation;

namespace UnitTests
{
	public class EncogWrapperTests
	{

		NetworkContainer container;

		[SetUp]
		public void Setup()
		{
			container = new NetworkContainer();
			EncogWrapper.Instantiate(ref container);

			EncogLayerSettings inputLayerSettings = new EncogLayerSettings();
			inputLayerSettings.activationFunction = null;
			inputLayerSettings.hasBias = true;
			inputLayerSettings.neuronCount = 100;

			EncogLayerSettings hiddenLayerOneSettings = new EncogLayerSettings();
			hiddenLayerOneSettings.activationFunction = new ActivationElliott();
			hiddenLayerOneSettings.hasBias = true;
			hiddenLayerOneSettings.neuronCount = 100;

			EncogLayerSettings outputLayerSettings = new EncogLayerSettings();
			outputLayerSettings.activationFunction = new ActivationElliott();
			outputLayerSettings.hasBias = false;
			outputLayerSettings.neuronCount = 1;

			EncogWrapper.AddLayer(ref container, ref inputLayerSettings);

			EncogWrapper.AddLayer(ref container, ref hiddenLayerOneSettings);

			EncogWrapper.AddLayer(ref container, ref outputLayerSettings);

			EncogWrapper.finalizeNetwork(ref container);
		}

		[Test]
		public void InstantiationTest()
		{
			Assert.IsNotNull(container.network);
		}

		[Test]
		public void AddLayer()
		{
			var Layers = container.network.Structure.Flat.LayerCounts;
			
			Assert.IsTrue(Layers.Length == 3);
		}

		[Test]
		public void SaveAndRestoreModelFS()
		{
			NetworkContainer container2 = new NetworkContainer();
			EncogWrapper.SaveNetworkToFS(ref container, "./unittest.bin");
			EncogWrapper.LoadNetworkFromFS(ref container2, "./unittest.bin");

			var Layers = container.network.Structure.Flat.LayerCounts;

			var Layers2 = container2.network.Structure.Flat.LayerCounts;
			
			Assert.AreEqual(Layers, Layers2);
		}
	}
}
