using UnityEngine;
using System.Collections;

public class DataFinder : MonoBehaviour {

    public DataTable dataTable;

    public DataRow FindById(int index) {
        return dataTable[index];
    }
}
