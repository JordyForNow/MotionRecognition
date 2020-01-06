namespace MotionRecognition
{
    public interface IInputFactory<T> where T : notnull
    {
        double[] GetNeuralInput(T settings);
    }
}
