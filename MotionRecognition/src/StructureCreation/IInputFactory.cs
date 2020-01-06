namespace MotionRecognition
{
    public struct InputFactorySettings {

    }
    public interface IInputFactory
    {
        double[] GetNeuralInput(InputFactorySettings settings);
    }
}
