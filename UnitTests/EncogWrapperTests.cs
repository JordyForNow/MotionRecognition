using NUnit.Framework;
using MotionRecognition;
using Encog.Engine.Network.Activation;
using System.IO;

namespace UnitTests
{
	public class EncogWrapperTests
	{

		NetworkContainer container;
		NetworkContainer container2;
		static string dataOutLocation = @"../../../DataOut/";
		static string dataOutName = "TestModel";

		CountNetworkTrainSettings trainSettings = new CountNetworkTrainSettings
		{
			correctInputDirectory = @"../../../CorrectData",
			incorrectInputDirectory = @"../../../IncorrectData",

			outputDirectory = dataOutLocation,
			outputName = dataOutName,

			sampleCount = 10
		};


		[SetUp]
		public void Setup()
		{

			if (File.Exists(dataOutLocation + dataOutName + ".eg"))
			{
				File.Delete(dataOutLocation + dataOutName + ".eg");
			}

		}

		[Test, Order(1)]
		public void InstantiationTest()
		{
			container = new NetworkContainer();
			EncogWrapper.Instantiate(ref container);

			Assert.IsNotNull(container.network);
		}

		[Test, Order(2)]
		public void AddLayer()
		{
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

			EncogWrapper.FinalizeNetwork(ref container);

			var Layers = container.network.Structure.Flat.LayerCounts;

			Assert.IsTrue(Layers.Length == 3);
		}

		[Test, Order(3)]
		public void SaveAndRestoreModelFS()
		{
			NetworkContainer container2 = new NetworkContainer();
			EncogWrapper.SaveNetworkToFS(ref container, "./unittest.bin");
			EncogWrapper.LoadNetworkFromFS(ref container2, "./unittest.bin");

			var Layers = container.network.Structure.Flat.LayerCounts;

			var Layers2 = container2.network.Structure.Flat.LayerCounts;

			Assert.AreEqual(Layers, Layers2);
		}

		[Test, Order(4)]
		public void CountPrepareNetworkBeforePrepareData()
		{
			Assert.Throws<IncorrectActionOrderException>(() => { CountNetworkTrainController.PrepareNetwork(ref container2, ref trainSettings); });
		}

		[Test, Order(5)]
		public void CountTrainBeforePrepareData()
		{
			Assert.Throws<IncorrectActionOrderException>(() => { CountNetworkTrainController.Train(ref container2, ref trainSettings); });
		}

		[Test, Order(6)]
		public void CountIncorrectInputDirectory()
		{
			CountNetworkTrainSettings trainSettings2 = new CountNetworkTrainSettings
			{
				correctInputDirectory = @"../NoLocation",
				incorrectInputDirectory = @"../../../IncorrectData",

				outputDirectory = dataOutLocation,
				outputName = dataOutName,

				sampleCount = 10
			};

			Assert.Throws<System.IO.DirectoryNotFoundException>(() => { CountNetworkTrainController.PrepareData(ref container2, ref trainSettings2); });
		}

		[Test, Order(7)]
		public void CountPrepareData()
		{
			try
			{
				CountNetworkTrainController.PrepareData(ref container2, ref trainSettings);
			}
			catch
			{
				Assert.IsTrue(false);
			}

			Assert.IsTrue(true);
		}

		[Test, Order(8)]
		public void CountPrepareDataAfterPrepareData()
		{
			Assert.Throws<IncorrectActionOrderException>(() => { CountNetworkTrainController.PrepareData(ref container2, ref trainSettings); });
		}

		[Test, Order(9)]
		public void CountTrainBeforePrepareNetwork()
		{
			Assert.Throws<IncorrectActionOrderException>(() => { CountNetworkTrainController.Train(ref container2, ref trainSettings); });
		}

		[Test, Order(10)]
		public void CountPrepareNetwork()
		{
			try
			{
				CountNetworkTrainController.PrepareNetwork(ref container2, ref trainSettings);
			}
			catch
			{
				Assert.IsTrue(false);
			}

			Assert.IsTrue(true);
		}

		[Test, Order(11)]
		public void CountTrainAndSaveNetwork()
		{
			CountNetworkTrainController.Train(ref container2, ref trainSettings);

			Assert.IsTrue(File.Exists(dataOutLocation + dataOutName + ".eg"));
		}
	}
}
