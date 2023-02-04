using UnityEngine;

[System.Serializable]
public class EnemySkinController
{
    public Renderer body;

    public GameObject[] equipments;

    public void RandomSkinAndEquipment()
    {
        for (int i = 0; i < equipments.Length; i++)
        {
            int random = Random.Range(0, 2);

            if (random == 0)
            {
                equipments[i].SetActive(false);
            }
            else
            {
                equipments[i].SetActive(true);
            }
        }

        body.material = EnemySkinHandler.Instance.skinMaterials[Random.Range(0, EnemySkinHandler.Instance.skinMaterials.Length)];
    }
}
