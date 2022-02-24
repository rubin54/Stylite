using Grid;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject socket;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            MissionController.Instance.CurrentlyRunning = socket.activeInHierarchy;
            socket.SetActive(!socket.activeInHierarchy);

            if(socket.activeInHierarchy)
            {
                foreach (var cell in CellGrid.Instance.Cells)
                {
                    cell.DehighlightCell();
                }
            }
        }
    }
}
