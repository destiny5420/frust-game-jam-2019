using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiState : MonoBehaviour
{
    public int Uistate = 1;
    int Uistate_old ;
    public GameObject[] UI;

    public void ButtonDown(int aa)
    {
        Uistate_old = Uistate; Uistate = aa;
    }
    void GetButton()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { Uistate_old = Uistate; Uistate = 1; }//正常順位
        if (Input.GetKeyDown(KeyCode.Alpha2)) { Uistate_old = Uistate; Uistate = 2; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { Uistate_old = Uistate; Uistate = 3; }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { Uistate_old = Uistate; Uistate = 4; }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { Uistate_old = Uistate; Uistate = 5; }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { Uistate_old = Uistate; Uistate = 6; }

        if (Uistate_old != Uistate) {    //-1 > 0~5
            UI[Uistate_old-1].SetActive(false);
            UI[Uistate-1].SetActive(true);
            Uistate_old = Uistate ;
        }

    }
    private void Update()
    {
        GetButton();
    }
}
