using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    public SlotBehaviour prefabSlot;
    public Transform parent;

    void Start()
    {
        CreateSlots();
    }

    void CreateSlots()
    {
        prefabSlot.gameObject.SetActive(true);

        var center = Instantiate(prefabSlot, BoardService.instance.roundCenter);//center 1
        center.transform.SetParent(parent);
        center.gameObject.name = "center";

        BoardService.instance.center = center;

        for (int i = 0; i < 6; i++)
        {
            var deg = i * 60 - 180;
            var x = Mathf.Cos(deg * Mathf.Deg2Rad) * BoardService.instance.radius * 1;
            var z = Mathf.Sin(-deg * Mathf.Deg2Rad) * BoardService.instance.radius * 1;
            var slot = Instantiate(prefabSlot, BoardService.instance.roundCenter.position + new Vector3(x, 0, z), Quaternion.identity);//ring 1
            slot.transform.SetParent(parent);
            slot.gameObject.name = "r1-" + i;
            BoardService.instance.ring1.Add(slot);
        }

        for (int i = 0; i < 12; i++)
        {
            var deg = i * 30 - 180;
            var x = Mathf.Cos(deg * Mathf.Deg2Rad) * BoardService.instance.radius * 2;
            var z = Mathf.Sin(-deg * Mathf.Deg2Rad) * BoardService.instance.radius * 2;
            var slot = Instantiate(prefabSlot, BoardService.instance.roundCenter.position + new Vector3(x, 0, z), Quaternion.identity);//ring 2
            slot.transform.SetParent(parent);
            slot.gameObject.name = "r2-" + i;
            BoardService.instance.ring2.Add(slot);
        }

        for (int i = 0; i < 18; i++)
        {
            var deg = i * 20 - 180;
            var x = Mathf.Cos(deg * Mathf.Deg2Rad) * BoardService.instance.radius * 3;
            var z = Mathf.Sin(-deg * Mathf.Deg2Rad) * BoardService.instance.radius * 3;
            var slot = Instantiate(prefabSlot, BoardService.instance.roundCenter.position + new Vector3(x, 0, z), Quaternion.identity);//ring 3
            slot.transform.SetParent(parent);
            slot.gameObject.name = "r3-" + i;
            BoardService.instance.ring3.Add(slot);
        }

        BoardService.instance.InitSlots();

        prefabSlot.gameObject.SetActive(false);
    }
}
