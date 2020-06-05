//                                          ▂ ▃ ▅ ▆ █ ZEN █ ▆ ▅ ▃ ▂ 
//                                        ..........<(+_+)>...........
// .cs (//)
//Autor: Alejandro Rivas                 alejandrotejemundos@hotmail.es
//Desc:
//Mod : Artificial neuron network
//Rev :
//..............................................................................................\\
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANN {
    public int numInputs;
    public int numOutputs;
    public int numHidden;
    public int numNPerHidden;
    public double alpha; // va a determinar como de rapido va a prender 0-1
    List<Layer> layers = new List<Layer>();
    public ANN(int nI, int nO, int nH, int nPH, double a)
    {
        numInputs = nI;
        numOutputs = nO;
        numHidden = nH;
        numNPerHidden = nPH;
        alpha = a;

        if (numHidden > 0)
        {
            layers.Add(new Layer(numNPerHidden, numInputs));

            for (int i = 0; i < numHidden - 1; i++)
            {
                layers.Add(new Layer(numNPerHidden, numNPerHidden));
            }

            layers.Add(new Layer(numOutputs, numNPerHidden));
        }
        else
        {
            layers.Add(new Layer(numOutputs, numInputs));
        }
    }
    //Funcion de calculo de "error" acuerdate de recojer los valores del sesgo, para evitar los margenes de error pequeños
    //public List<double> go(List<double> inputValues, List<double> desiredOutput) //esto esta descrito en la libreta, todo lo que tiene que hacer paso a paso, NO LO BORRES.
    //{

    //    List<double> inputs = new List<double>();
    //    List<double> outputs = new List<double>();

    //    if(inputValues.Count != numInputs)
    //    {
    //        Debug.Log("ERROR: EL NUMERO DE ENTRADAS/ INPUTS ES MAYOR QUE " + numInputs);
    //        return outputs;
    //    }

    //    inputs = new List<double>(inputValues);
    //    //recorremos cada una de las capas
    //    for(int i = 0; i < numHidden + 1; i++) // por eso es que le asignamos +1 al for, recuerda que la capa 0 es nuestra entrada
    //    {
    //            if(i > 0) //capa 0
    //            {
    //                inputs = new List<double>(outputs);
    //            }
    //            outputs.Clear(); //para que podamos llenarlo de nuevo

    //            for(int j = 0; j < layers[i].numNeuronas; j++) // va adar vueltas al numero de neuronas de la capa
    //            {
    //                double N = 0;
    //                layers[i].neuronas[j].inputs.Clear(); // con cada neurona vamos a calcular su peso multiplicado poir su entrada para cada una de sus entradas.

    //                for(int k = 0; k < layers[i].neuronas[j].numInputs; k++)
    //                {
    //                    layers[i].neuronas[j].inputs.Add(inputs[k]); // por cada entrada de la neurona estamos añadiendo en la entrada anterior, es decir se recojer valor de j, que es el for 2º anidacion
    //                    N += layers[i].neuronas[j].wights[k] * inputs[k];
    //                }

    //                N -= layers[i].neuronas[j].bias; //NOTEE lo va a hacer calcular un valor negativo de todos modos....
    //                layers[i].neuronas[j].output = ActivationFunction(N);
    //                outputs.Add(layers[i].neuronas[j].output);
    //            }
    //    }

    //    UpdateWeights(outputs, desiredOutput);

    //    return outputs;
    //}

    public List<double> Train(List<double> inputValues, List<double> desiredOutput)
    {
        List<double> outputValues = new List<double>();
        outputValues = CalcOutput(inputValues, desiredOutput);
        UpdateWeights(outputValues, desiredOutput);
        return outputValues;
    }

    public List<double> CalcOutput(List<double> inputValues, List<double> desiredOutput)
    {
        List<double> inputs = new List<double>();
        List<double> outputValues = new List<double>();
        int currentInput = 0;

        if (inputValues.Count != numInputs)
        {
            Debug.Log("ERROR: Number of Inputs must be " + numInputs);
            return outputValues;
        }

        inputs = new List<double>(inputValues);
        for (int i = 0; i < numHidden + 1; i++)
        {
            if (i > 0)
            {
                inputs = new List<double>(outputValues);
            }
            outputValues.Clear();

            for (int j = 0; j < layers[i].numNeuronas; j++)
            {
                double N = 0;
                layers[i].neuronas[j].inputs.Clear();

                for (int k = 0; k < layers[i].neuronas[j].numInputs; k++)
                {
                    layers[i].neuronas[j].inputs.Add(inputs[currentInput]);
                    N += layers[i].neuronas[j].wights[k] * inputs[currentInput];
                    currentInput++;
                }

                N -= layers[i].neuronas[j].bias;

                if (i == numHidden)
                    layers[i].neuronas[j].output = ActivationFunction(N);
                else
                    layers[i].neuronas[j].output = ActivationFunction(N);

                outputValues.Add(layers[i].neuronas[j].output);
                currentInput = 0;
            }
        }
        return outputValues;
    }

    public string PrintWeights()
    {
        string weightStr = "";
        foreach (Layer l in layers)
        {
            foreach (Neurona n in l.neuronas)
            {
                foreach (double w in n.wights)
                {
                    weightStr += w + ",";
                }
                weightStr += n.bias + ",";
            }
        }
        return weightStr;
    }

    public void LoadWeights(string weightStr)
    {
        if (weightStr == "") return;
        string[] weightValues = weightStr.Split(',');
        int w = 0;
        foreach (Layer l in layers)
        {
            foreach (Neurona n in l.neuronas)
            {
                for (int i = 0; i < n.wights.Count; i++)
                {
                    n.wights[i] = System.Convert.ToDouble(weightValues[w]);
                    w++;
                }
                n.bias = System.Convert.ToDouble(weightValues[w]);
                w++;
            }
        }
    }
	

    void UpdateWeights(List<double> outputs, List<double> desiredOutput)
    {
        double error;
        for (int i = numHidden; i >= 0; i--)
        {
            for (int j = 0; j < layers[i].numNeuronas; j++)
            {
                if (i == numHidden)
                {
                    error = desiredOutput[j] - outputs[j];
                    layers[i].neuronas[j].errorGardient = outputs[j] * (1 - outputs[j]) * error; // es mas simple de lo que aparente, recuerda, la salida solo ahi q multiplicarla por la anterior y este por el error
                    //layers[i].neuronas[j].errorGardient = outputs[j] * (outputs[j]) * error; MAL
                    // es.wikipedia.org/wiki/Delta_rule BIEN <- ESTO LO EXPLICA TODO
                }
                else
                {
                    
                    layers[i].neuronas[j].errorGardient = layers[i].neuronas[j].output * (1 - layers[i].neuronas[j].output);
                    double errorGradSum = 0;
                    for (int p = 0; p < layers[i + 1].numNeuronas; p++)
                    {
                        errorGradSum += layers[i + 1].neuronas[p].errorGardient * layers[i + 1].neuronas[p].wights[j];
                    }
                    layers[i].neuronas[j].errorGardient *= errorGradSum;
                }
                for (int k = 0; k < layers[i].neuronas[j].numInputs; k++) //recorre cada neurona
                {
                    if (i == numHidden)
                    {
                        error = desiredOutput[j] - outputs[j];
                        layers[i].neuronas[j].wights[k] += alpha * layers[i].neuronas[j].inputs[k] * error;
                    }
                    else
                    {
                        layers[i].neuronas[j].wights[k] += alpha * layers[i].neuronas[j].inputs[k] * layers[i].neuronas[j].errorGardient;
                    }
                }
                layers[i].neuronas[j].bias += alpha * -1 * layers[i].neuronas[j].errorGardient;
            }

        }

    }





    //para todas las funciones de activacion de las listas
    //fijate hasta que punto es el codigo reutilizable que simplemente cambiando las funciones de activacion podemos reutilizarlo para absolutamente todo, no se me ocurre nada que no pueda utilizarse para el ambito de RDN
    //en.wikipedia.org/wiki/Activation_function
    double ActivationFunction(double value)
    {
        return Sigmoid(value);
    }

    double Step(double value) //(aka binary step)
    {
        if (value < 0) return 0;
        else return 1;
    }
    double TanH(double value)
    {
        double k = (double)System.Math.Exp(-2 * value);
        return 2 / (1.0f + k) - 1;
    }

    double ReLu(double value)
    {
        if (value > 0) return value;
        else return 0;
    }

    double LeakyReLu(double value)
    {
        if (value < 0) return 0.01 * value;
        else return value;
    }

    double Sigmoid(double value) //(aka logistic softstep) // metodo extraido del repositorio de git de rdn
    {
        double k = (double)System.Math.Exp(value);
        return k / (1.0f + k);
    }
    //public void Actualiza()
    //{
    //    pub
    //}














	
} // fin de la clase ___________________________________________________________________________________________________________________________________
