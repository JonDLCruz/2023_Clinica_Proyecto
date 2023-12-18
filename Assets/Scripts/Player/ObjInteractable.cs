using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ObjetosClinica", menuName = "Resources/DB/InteractableObjects", order = 1)]
public class ObjInteractable : ScriptableObject
{
    [SerializeField]
    public struct Objeto
    {
        public int ID;
        public string Name;
        public string Descr;
        public string VideoPath;
        public string AnimPath;
        public string Photopath;
    }
    public Objeto[] objetosClinica;
    private void OnValidate()
    {
        if (objetosClinica != null)
        {
            for (int i = 0; i < objetosClinica.Length; i++) {
                if (objetosClinica[i].ID !=1)
                {
                    objetosClinica[i].ID = i;
                }
            }
        }
    }
}
