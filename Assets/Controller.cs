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
using System.IO;
public class Controller : MonoBehaviour
{

    public float speed =50.0f;
    public float rotationSpeed = 100.0f;
    public float DistanciaDeVisivilidad = 200;
    List<string> Valors = new List<string>();
    StreamWriter tdf;
    void Start()
    {
        string path = Application.dataPath + "/TrainingData.txt";
        tdf = File.CreateText(path);
    }
    void OnApplicationQuit()
    {
        Debug.Log(Valors.Count);
        foreach (string td in Valors)
        {
            tdf.WriteLine(td);
        }
        tdf.Close();
    }
    void Update()
    {
        float translationInput = Input.GetAxis("Vertical") * speed;
        float rotationInput = Input.GetAxis("Horizontal") * rotationSpeed;
       // float translation=0;
      //  float rotation=0;
   float     translation = Time.deltaTime *speed*translationInput;
       float rotation = Time.deltaTime* rotationSpeed*rotationInput;

        transform.Translate(0, 0, translation);

        transform.Rotate(0, rotation, 0);
        Debug.DrawRay(transform.position, this.transform.forward * DistanciaDeVisivilidad, Color.red);
        Debug.DrawRay(transform.position, this.transform.right * DistanciaDeVisivilidad, Color.red);
        RaycastHit hit;

        float fDist = 0, rDist = 0,
            lDist = 0, r45Dist = 0, 
            l45Dist = 0;
        if (Physics.Raycast(transform.position, this.transform.forward, out hit, DistanciaDeVisivilidad))
        {
            fDist =1- Round( hit.distance/DistanciaDeVisivilidad);
        }
        if (Physics.Raycast(transform.position, this.transform.right, out hit, DistanciaDeVisivilidad))
        {
            rDist = 1 - Round(hit.distance / DistanciaDeVisivilidad);
        }
        if (Physics.Raycast(transform.position, -this.transform.right, out hit, DistanciaDeVisivilidad))
        {
            lDist = 1 - Round(hit.distance / DistanciaDeVisivilidad);
        }
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(45,Vector3.up)* this.transform.right, out hit, DistanciaDeVisivilidad))
        {
            r45Dist = 1 - Round(hit.distance / DistanciaDeVisivilidad);
        }
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(45, Vector3.up) * -this.transform.right, out hit, DistanciaDeVisivilidad))
        {
            l45Dist = 1 - Round(hit.distance / DistanciaDeVisivilidad);
        }
        
        string td = fDist + "," + rDist + "," + lDist + "," + r45Dist + "," + l45Dist + "," + Round( translationInput) + "," + Round( rotationInput);
        if (!Valors.Contains(td))
        {
            Valors.Add(td);
        }
        Valors.Add(td);
    }
    float Round(float x)
    {                         //retorna el valor de un número redondeado al entero más cercano. //Especifica cómo los métodos de redondeo matemáticos deben procesar un número que está comprendido entre dos números.
        return (float)System.Math.Round(x, System.MidpointRounding.AwayFromZero) / 2.0f;
    }
}
