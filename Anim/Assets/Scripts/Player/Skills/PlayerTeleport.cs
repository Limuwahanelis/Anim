using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] InputActionReference cursorPos;
    [SerializeField] GameObject tester;
    [SerializeField] GameObject objectToTeleport;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        tester.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = cursorPos.action.ReadValue<Vector2>();

        mousePos.z = Camera.main.nearClipPlane;
        Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 200))
        {
            tester.transform.position = hit.point;
        }
    }

    public void Perform()
    {
        if(enabled) objectToTeleport.transform.position = tester.transform.position;

    }
    private void OnDisable()
    {
        tester.SetActive(false);
    }
}
