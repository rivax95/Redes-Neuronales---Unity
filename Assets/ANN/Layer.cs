//                                          ▂ ▃ ▅ ▆ █ ZEN █ ▆ ▅ ▃ ▂ 
//                                        ..........<(+_+)>...........
// .cs (//)
//Autor: Alejandro Rivas                 alejandrotejemundos@hotmail.es
//Desc:
//Mod : 
//Rev :
//..............................................................................................\\
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour {

    public int numNeuronas;
    public List<Neurona> neuronas = new List<Neurona>();
    public Layer(int nNeuronas, int numNeuronasInputs)
    {
        numNeuronas = nNeuronas;
        for (int i = 0; i < nNeuronas; i++)
        {
            neuronas.Add(new Neurona(numNeuronasInputs)); // creamos esas neuronas y al crearlas le damos un numero de inputs
        }
    }
}
