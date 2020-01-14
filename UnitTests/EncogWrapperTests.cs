using NUnit.Framework;
using MotionRecognition;
using Encog.Engine.Network.Activation;

namespace UnitTests
{
    public class EncogWrapperTests
    {

        NetworkContainer container;


        [SetUp]
        public void Setup() { }

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

            EncogWrapper.finalizeNetwork(ref container);

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
    }
}
