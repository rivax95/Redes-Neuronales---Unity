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

public class Neurona : MonoBehaviour {

    public int numInputs;
    public double bias;
    public double output;
    public double errorGardient;
    public List<double> wights = new List<double>();
    public List<double> inputs = new List<double>();

    public Neurona(int nInputs)
    {
        bias = Random.RandomRange(-1.0f, 1.0f);
        numInputs = nInputs;
        for (int i = 0; i < nInputs; i++)
        {
            wights.Add(Random.RandomRange(-1.0f,1.0f));
        }
    }
}
