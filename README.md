# Motion Recognition Library

#### ![](https://github.com/JordyForNow/KBS-SE3_VR-Rehabilitation-Data/actions/building.yml/badge.svg)

## Installation
To install the Motion Recognition library you should download the latest release from the [releases](https://github.com/JordyForNow/KBS-SE3_VR-Rehabilitation-Data/releases) page.
A release includes the MotionRecognition and the MotionRecognitionHelper DLL's. This helper is optional and has some support functions like manipulating data (CSV files) or visualize the input for the neural network.

Once downloaded you can add the DLL's to your .NET project, they are not available in NuGet and will require a manual installation. (Check the Microsoft documention to see how to add DLLs manually)

#### Dependencies
* [encog-dotnet-core](https://github.com/jeffheaton/encog-dotnet-core) is needed for the neural net, it's available in the NuGet Package Manager. To use this package you will need to run it on the windows platform.
* .NET core 2.2.103 or higher

## Setup
To get started with the neural network you will need a NetworkContainer, this will hold and manage the neural net.

```
NetworkContainer container = new NetworkContainer
{
	verbose = true
};
```
### Training a network
The library currently has two methods of using data to train and use a Neural Net. ImageBased and CountBased methods, the image based method transforms the input into a 3D-matrix and uses this as input for the network. The count based method retrieves a specific count of samples from the original input, it does this by calculating an interval at which it takes data out from the original set. 

For both methods a seperate controller has been created, to use either of the methods the corresponding controller can be picked. (ImageNetworkController or CountNetworkController) 

In this example we will use the CountNetwork because it has proven to be more reliable than the ImageNetwork. We already created a container so we can now setup a controller, which besides the already created container, will need settings:

```
CountNetworkTrainSettings trainSettings = new CountNetworkTrainSettings
{
  correctInputDirectory = @"/path/to/CorrectData",
  incorrectInputDirectory = @"/path/to/IncorrectTestData",

  outputDirectory = @"/path/to/DataOut",
  outputName = "NetworkName",

  // number of input entries
	sampleCount = 10
};
```

The TrainController is now ready to be used, it is a static object so it can be called directly. It has three main functions to setup and train the network. Because we chose a CountNetwork we will use the CountNetworkTrainController.

Firstly, prepare the data. This will take the input data and transform it to a usable format.
```
CountNetworkTrainController.PrepareData(ref container, ref trainSettings);
```

After this the network itself can be setup.
```
CountNetworkTrainController.PrepareNetwork(ref container, ref trainSettings);
```

And then finally the train function will train the network with the input data, if verbose was set to true in the container the console will show the progress off the container with the network.
```
CountNetworkTrainController.Train(ref container, ref trainSettings);
```

### Making a prediction
After the network has been trained it can be used to predict if supplied data is correct or false compared to the input data. Ofcourse the PredictController also needs settings and a container. 
```
CountNetworkPredictSettings predictSettings = new CountNetworkPredictSettings
{
  trainedNetwork = @"/path/to/network.eg",
  predictData = @"/path/to/predict/data.csv",

  // number of input entries
	networkInputSize = 10
};
```

Using these settings the controller can be prepared and predict an outcome.
```
CountNetworkPredictController.PreparePredictor(ref container, ref predictSettings);
Console.WriteLine(CountNetworkPredictController.Predict(ref container, ref predictSettings));
```



